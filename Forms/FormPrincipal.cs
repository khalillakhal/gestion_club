using System.Drawing;
using System.Windows.Forms;

namespace GestionClub
{
    // Fenêtre d'accueil : un bouton par interface
    public class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            Text = "Club Sportif de Carthage";
            ClientSize = new Size(420, 300);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label titre = new Label
            {
                Text = "Gestion du club sportif",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(85, 30)
            };
            Controls.Add(titre);

            Button btnMembres = new Button { Text = "Gestion des membres", Location = new Point(110, 95), Size = new Size(200, 40) };
            btnMembres.Click += (s, e) => new FormMembres().ShowDialog();
            Controls.Add(btnMembres);

            Button btnSeances = new Button { Text = "Planning des séances", Location = new Point(110, 150), Size = new Size(200, 40) };
            btnSeances.Click += (s, e) => new FormSeances().ShowDialog();
            Controls.Add(btnSeances);

            Button btnQuitter = new Button { Text = "Quitter", Location = new Point(110, 205), Size = new Size(200, 40) };
            btnQuitter.Click += (s, e) => Application.Exit();
            Controls.Add(btnQuitter);
        }
    }
}
