using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GestionClub
{
    // Interface CRUD des séances d'entraînement.
    // Elle montre les relations : chaque séance est liée à un entraîneur et à une discipline.
    public class FormSeances : Form
    {
        private readonly ClubContext db = new ClubContext();
        private readonly Club club;
        private SeanceEntrainement seanceSelectionnee;

        private ComboBox cmbEntraineur, cmbDiscipline;
        private DateTimePicker dtpDate;
        private NumericUpDown nudDuree;
        private TextBox txtLieu;
        private DataGridView dgvSeances;

        public FormSeances()
        {
            club = new Club("Club Sportif de Carthage", "Carthage, Tunis", db);
            InitialiserInterface();
            ChargerListes();
            ChargerSeances();
        }

        // ---------------------------------------------------------------
        // Construction de l'interface
        // ---------------------------------------------------------------
        private void InitialiserInterface()
        {
            Text = "Planning des séances d'entraînement";
            ClientSize = new Size(1100, 500);
            StartPosition = FormStartPosition.CenterScreen;

            GroupBox grp = new GroupBox
            {
                Text = "Séance",
                Location = new Point(12, 12),
                Size = new Size(330, 475)
            };
            Controls.Add(grp);

            int y = 30;

            grp.Controls.Add(new Label { Text = "Entraîneur :", Location = new Point(10, y + 3), AutoSize = true });
            cmbEntraineur = new ComboBox { Location = new Point(130, y), Width = 185, DropDownStyle = ComboBoxStyle.DropDownList };
            grp.Controls.Add(cmbEntraineur);
            y += 35;

            grp.Controls.Add(new Label { Text = "Discipline :", Location = new Point(10, y + 3), AutoSize = true });
            cmbDiscipline = new ComboBox { Location = new Point(130, y), Width = 185, DropDownStyle = ComboBoxStyle.DropDownList };
            grp.Controls.Add(cmbDiscipline);
            y += 35;

            grp.Controls.Add(new Label { Text = "Date et heure :", Location = new Point(10, y + 3), AutoSize = true });
            dtpDate = new DateTimePicker
            {
                Location = new Point(130, y),
                Width = 185,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm"
            };
            grp.Controls.Add(dtpDate);
            y += 35;

            grp.Controls.Add(new Label { Text = "Durée (min) :", Location = new Point(10, y + 3), AutoSize = true });
            nudDuree = new NumericUpDown { Location = new Point(130, y), Width = 185, Minimum = 15, Maximum = 300, Increment = 15, Value = 90 };
            grp.Controls.Add(nudDuree);
            y += 35;

            grp.Controls.Add(new Label { Text = "Lieu :", Location = new Point(10, y + 3), AutoSize = true });
            txtLieu = new TextBox { Location = new Point(130, y), Width = 185 };
            grp.Controls.Add(txtLieu);
            y += 50;

            grp.Controls.Add(CreerBouton("Planifier", 10, y, BtnAjouter_Click));
            grp.Controls.Add(CreerBouton("Modifier", 170, y, BtnModifier_Click));
            y += 42;
            grp.Controls.Add(CreerBouton("Supprimer", 10, y, BtnSupprimer_Click));
            grp.Controls.Add(CreerBouton("Vider les champs", 170, y, (s, e) => ViderChamps()));
            y += 42;
            grp.Controls.Add(CreerBouton("Annuler la séance", 10, y, BtnAnnulerSeance_Click));

            dgvSeances = new DataGridView
            {
                Location = new Point(355, 12),
                Size = new Size(730, 475),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            dgvSeances.CellClick += DgvSeances_CellClick;
            Controls.Add(dgvSeances);
        }

        private Button CreerBouton(string texte, int x, int y, EventHandler action)
        {
            Button b = new Button { Text = texte, Location = new Point(x, y), Size = new Size(150, 34) };
            b.Click += action;
            return b;
        }

        // ---------------------------------------------------------------
        // Lecture
        // ---------------------------------------------------------------

        // Remplit les listes déroulantes à partir de la base
        // (la ComboBox affiche le ToString() de chaque objet)
        private void ChargerListes()
        {
            cmbEntraineur.DataSource = club.Entraineurs;
            cmbDiscipline.DataSource = club.Disciplines;
        }

        private void ChargerSeances()
        {
            List<SeanceEntrainement> seances = club.Seances;

            dgvSeances.DataSource = seances.Select(s => new
            {
                s.Id,
                Date = s.Date.ToString("dd/MM/yyyy HH:mm"),
                Durée = $"{(int)s.Duree.TotalMinutes} min",
                s.Lieu,
                Discipline = s.Discipline.Nom,
                Entraîneur = s.Entraineur.ToString(),
                Participants = s.Participants.Count,
                Statut = s.Annulee ? "Annulée" : "Prévue"
            }).ToList();
        }

        private void DgvSeances_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = (int)dgvSeances.Rows[e.RowIndex].Cells["Id"].Value;
            seanceSelectionnee = club.RechercherSeance(id);
            if (seanceSelectionnee == null) return;

            // Même contexte => mêmes objets : on peut les sélectionner directement
            cmbEntraineur.SelectedItem = seanceSelectionnee.Entraineur;
            cmbDiscipline.SelectedItem = seanceSelectionnee.Discipline;
            dtpDate.Value = seanceSelectionnee.Date;
            nudDuree.Value = Math.Min(nudDuree.Maximum, Math.Max(nudDuree.Minimum, (decimal)seanceSelectionnee.Duree.TotalMinutes));
            txtLieu.Text = seanceSelectionnee.Lieu;
        }

        // ---------------------------------------------------------------
        // Création / Modification / Suppression
        // ---------------------------------------------------------------
        private void BtnAjouter_Click(object sender, EventArgs e)
        {
            if (!FormulaireValide()) return;
            if (!CreneauDisponible((Entraineur)cmbEntraineur.SelectedItem, txtLieu.Text,
        dtpDate.Value, TimeSpan.FromMinutes((double)nudDuree.Value), 0))
                return;

            try
            {
                Entraineur entraineur = (Entraineur)cmbEntraineur.SelectedItem;
                Discipline discipline = (Discipline)cmbDiscipline.SelectedItem;

                // Méthode du diagramme : c'est l'entraîneur qui planifie la séance
                SeanceEntrainement seance = entraineur.PlanifierSeance(
                    dtpDate.Value,
                    TimeSpan.FromMinutes((double)nudDuree.Value),
                    txtLieu.Text.Trim(),
                    discipline);

                club.AjouterSeance(seance);

                ChargerSeances();
                ViderChamps();
                MessageBox.Show("Séance planifiée.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }
        }

        private void BtnModifier_Click(object sender, EventArgs e)
        {
            if (seanceSelectionnee == null)
            {
                MessageBox.Show("Sélectionnez d'abord une séance dans la liste.");
                return;
            }
            if (!FormulaireValide()) return;
            if (!CreneauDisponible((Entraineur)cmbEntraineur.SelectedItem, txtLieu.Text,
        dtpDate.Value, TimeSpan.FromMinutes((double)nudDuree.Value), seanceSelectionnee.Id))
                return;

            try
            {
                seanceSelectionnee.Entraineur = (Entraineur)cmbEntraineur.SelectedItem;
                seanceSelectionnee.Discipline = (Discipline)cmbDiscipline.SelectedItem;
                seanceSelectionnee.Date = dtpDate.Value;
                seanceSelectionnee.Duree = TimeSpan.FromMinutes((double)nudDuree.Value);
                seanceSelectionnee.Lieu = txtLieu.Text.Trim();

                club.ModifierSeance(seanceSelectionnee);

                ChargerSeances();
                MessageBox.Show("Séance modifiée.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }
        }

        private void BtnSupprimer_Click(object sender, EventArgs e)
        {
            if (seanceSelectionnee == null)
            {
                MessageBox.Show("Sélectionnez d'abord une séance dans la liste.");
                return;
            }

            DialogResult reponse = MessageBox.Show("Supprimer cette séance ?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (reponse != DialogResult.Yes) return;

            try
            {
                club.SupprimerSeance(seanceSelectionnee.Id);
                ChargerSeances();
                ViderChamps();
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }
        }

        private void BtnAnnulerSeance_Click(object sender, EventArgs e)
        {
            if (seanceSelectionnee == null)
            {
                MessageBox.Show("Sélectionnez d'abord une séance dans la liste.");
                return;
            }

            try
            {
                seanceSelectionnee.Annuler();            // méthode du diagramme de classes
                club.ModifierSeance(seanceSelectionnee);
                ChargerSeances();
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }
        }

        // ---------------------------------------------------------------
        // Outils
        // ---------------------------------------------------------------
        // Vérifie qu'aucune autre séance (non annulée) ne se chevauche
        // avec le même entraîneur ou au même lieu.
        // idIgnore : la séance en cours de modification (0 pour une nouvelle séance).
        private bool CreneauDisponible(Entraineur entraineur, string lieu, DateTime debut, TimeSpan duree, int idIgnore)
        {
            DateTime fin = debut + duree;

            foreach (SeanceEntrainement s in club.Seances)
            {
                if (s.Id == idIgnore || s.Annulee)
                    continue;

                DateTime finExistante = s.Date + s.Duree;
                bool chevauche = s.Date < fin && debut < finExistante;
                if (!chevauche)
                    continue;

                if (s.Entraineur.Id == entraineur.Id)
                {
                    MessageBox.Show(
                        $"{entraineur} anime déjà une séance de {s.Discipline} " +
                        $"de {s.Date:HH:mm} à {finExistante:HH:mm} le {s.Date:dd/MM/yyyy} ({s.Lieu}).",
                        "Conflit d'horaire", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.Equals(s.Lieu.Trim(), lieu.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        $"Le lieu « {s.Lieu} » est déjà occupé de {s.Date:HH:mm} à {finExistante:HH:mm} " +
                        $"le {s.Date:dd/MM/yyyy} (séance de {s.Discipline} avec {s.Entraineur}).",
                        "Conflit d'horaire", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }
        private bool FormulaireValide()
        {
            if (cmbEntraineur.SelectedItem == null || cmbDiscipline.SelectedItem == null)
            {
                MessageBox.Show("Choisissez un entraîneur et une discipline.");
                return false;
            }
            if (txtLieu.Text.Trim() == "")
            {
                MessageBox.Show("Le lieu est obligatoire.");
                return false;
            }
            return true;
        }

        private void ViderChamps()
        {
            seanceSelectionnee = null;
            txtLieu.Clear();
            nudDuree.Value = 90;
            dtpDate.Value = DateTime.Now;
            dgvSeances.ClearSelection();
        }

        private void AfficherErreur(Exception ex)
        {
            string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            MessageBox.Show("Erreur : " + message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db.Dispose();
            base.OnFormClosed(e);
        }
    }
}
