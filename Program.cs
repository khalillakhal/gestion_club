using System;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace GestionClub
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                using (ClubContext db = new ClubContext())
                {
                    // Applique les migrations pas encore appliquées (crée club.db si besoin)
                    db.Database.Migrate();

                    // Données de départ si la base est vide
                    DonneesInitiales.Remplir(db);
                }
            }
            catch (Exception ex)
            {
                string detail = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                MessageBox.Show(
                    "Impossible d'ouvrir la base de données.\n\n" +
                    "Avez-vous créé la migration ? Dans la Console du Gestionnaire de package :\n" +
                    "   Add-Migration InitialCreate\n" +
                    "   Update-Database\n\n" +
                    "Détail : " + detail,
                    "Base de données", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new FormPrincipal());
        }
    }
}
