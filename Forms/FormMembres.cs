using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GestionClub
{
    // Interface CRUD des membres : Créer, Lire (liste), Modifier, Supprimer
    public class FormMembres : Form
    {
        private const int AgeMinimum = 5;   // âge minimum pour s'inscrire au club
        private readonly ClubContext db = new ClubContext();
        private readonly Club club;
        private Membre membreSelectionne;   // membre cliqué dans la liste (null si aucun)

        private TextBox txtNom, txtPrenom, txtEmail, txtTelephone, txtRecherche;
        private DateTimePicker dtpNaissance, dtpInscription;
        private CheckBox chkCotisation;
        private DataGridView dgvMembres;

        public FormMembres()
        {
            club = new Club("Club Sportif de Carthage", "Carthage, Tunis", db);
            InitialiserInterface();
            ChargerMembres();
        }

        // ---------------------------------------------------------------
        // Construction de l'interface
        // ---------------------------------------------------------------
        private void InitialiserInterface()
        {
            Text = "Gestion des membres";
            ClientSize = new Size(1100, 540);
            StartPosition = FormStartPosition.CenterScreen;

            GroupBox grp = new GroupBox
            {
                Text = "Informations du membre",
                Location = new Point(12, 12),
                Size = new Size(330, 515)
            };
            Controls.Add(grp);

            int y = 30;
            txtNom = AjouterTextBox(grp, "Nom :", ref y);
            txtPrenom = AjouterTextBox(grp, "Prénom :", ref y);
            dtpNaissance = AjouterDate(grp, "Né(e) le :", ref y);
            txtEmail = AjouterTextBox(grp, "Email :", ref y);
            txtTelephone = AjouterTextBox(grp, "Téléphone :", ref y);
            dtpInscription = AjouterDate(grp, "Inscrit le :", ref y);

            // Lecture seule : la cotisation ne change que par le bouton "Payer la cotisation"
            chkCotisation = new CheckBox { Text = "Cotisation payée", Location = new Point(130, y), AutoSize = true, AutoCheck = false };
            grp.Controls.Add(chkCotisation);
            y += 45;

            grp.Controls.Add(CreerBouton("Ajouter", 10, y, BtnAjouter_Click));
            grp.Controls.Add(CreerBouton("Modifier", 170, y, BtnModifier_Click));
            y += 42;
            grp.Controls.Add(CreerBouton("Supprimer", 10, y, BtnSupprimer_Click));
            grp.Controls.Add(CreerBouton("Vider les champs", 170, y, (s, e) => ViderChamps()));
            y += 42;
            grp.Controls.Add(CreerBouton("Payer la cotisation", 10, y, BtnPayer_Click));

            // Zone de recherche
            Controls.Add(new Label { Text = "Rechercher (nom ou prénom) :", Location = new Point(355, 18), AutoSize = true });
            txtRecherche = new TextBox { Location = new Point(540, 15), Width = 250 };
            txtRecherche.TextChanged += (s, e) => ChargerMembres();
            Controls.Add(txtRecherche);

            // Liste des membres
            dgvMembres = new DataGridView
            {
                Location = new Point(355, 50),
                Size = new Size(730, 477),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            dgvMembres.CellClick += DgvMembres_CellClick;
            Controls.Add(dgvMembres);
        }

        private TextBox AjouterTextBox(Control parent, string libelle, ref int y)
        {
            parent.Controls.Add(new Label { Text = libelle, Location = new Point(10, y + 3), AutoSize = true });
            TextBox txt = new TextBox { Location = new Point(130, y), Width = 185 };
            parent.Controls.Add(txt);
            y += 35;
            return txt;
        }

        private DateTimePicker AjouterDate(Control parent, string libelle, ref int y)
        {
            parent.Controls.Add(new Label { Text = libelle, Location = new Point(10, y + 3), AutoSize = true });
            DateTimePicker dtp = new DateTimePicker { Location = new Point(130, y), Width = 185, Format = DateTimePickerFormat.Short };
            parent.Controls.Add(dtp);
            y += 35;
            return dtp;
        }

        private Button CreerBouton(string texte, int x, int y, EventHandler action)
        {
            Button b = new Button { Text = texte, Location = new Point(x, y), Size = new Size(150, 34) };
            b.Click += action;
            return b;
        }

        // ---------------------------------------------------------------
        // Lecture : afficher les membres dans la grille
        // ---------------------------------------------------------------
        private void ChargerMembres()
        {
            List<Membre> membres = club.Membres;

            string filtre = txtRecherche.Text.Trim().ToLower();
            if (filtre != "")
                membres = membres.FindAll(m => m.Nom.ToLower().Contains(filtre) || m.Prenom.ToLower().Contains(filtre));

            // On n'affiche que les colonnes utiles (pas les listes de navigation)
            dgvMembres.DataSource = membres.Select(m => new
            {
                m.Id,
                m.Nom,
                Prénom = m.Prenom,
                Naissance = m.DateNaissance.ToString("dd/MM/yyyy"),
                m.Email,
                Téléphone = m.Telephone,
                Inscription = m.DateInscription.ToString("dd/MM/yyyy"),
                Cotisation = m.CotisationPayee ? "Payée" : "Non payée"
            }).ToList();
        }

        // Clic sur une ligne : on remplit le formulaire avec le membre choisi
        private void DgvMembres_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = (int)dgvMembres.Rows[e.RowIndex].Cells["Id"].Value;
            membreSelectionne = club.RechercherMembre(id);
            if (membreSelectionne == null) return;

            txtNom.Text = membreSelectionne.Nom;
            txtPrenom.Text = membreSelectionne.Prenom;
            dtpNaissance.Value = membreSelectionne.DateNaissance;
            txtEmail.Text = membreSelectionne.Email;
            txtTelephone.Text = membreSelectionne.Telephone;
            dtpInscription.Value = membreSelectionne.DateInscription;
            chkCotisation.Checked = membreSelectionne.CotisationPayee;
        }

        // ---------------------------------------------------------------
        // Création / Modification / Suppression
        // ---------------------------------------------------------------
        private void BtnAjouter_Click(object sender, EventArgs e)
        {
            if (!FormulaireValide(0)) return;

            try
            {
                // Id = 0 : c'est la base de données qui va générer l'identifiant
                Membre m = new Membre(0, txtNom.Text.Trim(), txtPrenom.Text.Trim(),
                    dtpNaissance.Value.Date, txtEmail.Text.Trim(), txtTelephone.Text.Trim());
                RemplirDepuisFormulaire(m);   // date d'inscription + cotisation
                club.AjouterMembre(m);

                ChargerMembres();
                ViderChamps();
                MessageBox.Show("Membre ajouté.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }
        }

        private void BtnModifier_Click(object sender, EventArgs e)
        {
            if (membreSelectionne == null)
            {
                MessageBox.Show("Sélectionnez d'abord un membre dans la liste.");
                return;
            }
            if (!FormulaireValide(membreSelectionne.Id)) return;

            try
            {
                RemplirDepuisFormulaire(membreSelectionne);
                club.ModifierMembre(membreSelectionne);

                ChargerMembres();
                MessageBox.Show("Membre modifié.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }
        }

        private void BtnSupprimer_Click(object sender, EventArgs e)
        {
            if (membreSelectionne == null)
            {
                MessageBox.Show("Sélectionnez d'abord un membre dans la liste.");
                return;
            }

            DialogResult reponse = MessageBox.Show(
                $"Supprimer {membreSelectionne} ?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (reponse != DialogResult.Yes) return;

            try
            {
                club.SupprimerMembre(membreSelectionne.Id);
                ChargerMembres();
                ViderChamps();
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }
        }

        private void BtnPayer_Click(object sender, EventArgs e)
        {
            if (membreSelectionne == null)
            {
                MessageBox.Show("Sélectionnez d'abord un membre dans la liste.");
                return;
            }

            if (membreSelectionne.CotisationPayee)
            {
                MessageBox.Show($"La cotisation de {membreSelectionne} est déjà payée.",
                    "Cotisation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                membreSelectionne.PayerCotisation();       // méthode du diagramme de classes
                club.ModifierMembre(membreSelectionne);
                chkCotisation.Checked = true;
                ChargerMembres();
                MessageBox.Show($"Cotisation de {membreSelectionne} enregistrée.",
                    "Cotisation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AfficherErreur(ex);
            }
        }

        // ---------------------------------------------------------------
        // Outils
        // ---------------------------------------------------------------
        // idIgnore : Id du membre en cours de modification (0 pour un ajout)
        private bool FormulaireValide(int idIgnore)
        {
            string nom = txtNom.Text.Trim();
            string prenom = txtPrenom.Text.Trim();
            string email = txtEmail.Text.Trim();
            DateTime naissance = dtpNaissance.Value.Date;
            DateTime inscription = dtpInscription.Value.Date;

            if (nom == "" || prenom == "")
            {
                MessageBox.Show("Le nom et le prénom sont obligatoires.");
                return false;
            }

            if (email != "" && !email.Contains("@"))
            {
                MessageBox.Show("L'email n'est pas valide.");
                return false;
            }

            // Date de naissance : dans le passé et âge minimum
            if (naissance > DateTime.Today.AddYears(-AgeMinimum))
            {
                MessageBox.Show($"Date de naissance incorrecte : le membre doit avoir au moins {AgeMinimum} ans.");
                return false;
            }

            // Date d'inscription : après la naissance et pas dans le futur
            if (inscription < naissance)
            {
                MessageBox.Show("La date d'inscription ne peut pas être antérieure à la date de naissance.");
                return false;
            }
            if (inscription > DateTime.Today)
            {
                MessageBox.Show("La date d'inscription ne peut pas être dans le futur.");
                return false;
            }

            // Doublons : même email, ou mêmes nom + prénom + date de naissance
            foreach (Membre m in club.Membres)
            {
                if (m.Id == idIgnore)
                    continue;

                if (email != "" && m.Email != null &&
                    string.Equals(m.Email.Trim(), email, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show($"Cet email est déjà utilisé par {m}.", "Doublon",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (string.Equals(m.Nom.Trim(), nom, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(m.Prenom.Trim(), prenom, StringComparison.OrdinalIgnoreCase) &&
                    m.DateNaissance.Date == naissance)
                {
                    MessageBox.Show($"{m} (né(e) le {naissance:dd/MM/yyyy}) est déjà inscrit(e) au club.", "Doublon",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private void RemplirDepuisFormulaire(Membre m)
        {
            m.Nom = txtNom.Text.Trim();
            m.Prenom = txtPrenom.Text.Trim();
            m.DateNaissance = dtpNaissance.Value.Date;
            m.Email = txtEmail.Text.Trim();
            m.Telephone = txtTelephone.Text.Trim();
            m.DateInscription = dtpInscription.Value.Date;
        }

        private void ViderChamps()
        {
            membreSelectionne = null;
            txtNom.Clear();
            txtPrenom.Clear();
            txtEmail.Clear();
            txtTelephone.Clear();
            dtpNaissance.Value = new DateTime(2000, 1, 1);
            dtpInscription.Value = DateTime.Today;
            chkCotisation.Checked = false;
            dgvMembres.ClearSelection();
        }

        private void AfficherErreur(Exception ex)
        {
            string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            MessageBox.Show("Erreur : " + message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // On libère la connexion à la base quand la fenêtre se ferme
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db.Dispose();
            base.OnFormClosed(e);
        }
    }
}
