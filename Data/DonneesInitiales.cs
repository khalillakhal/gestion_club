using System;
using System.Linq;

namespace GestionClub
{
    // Ajoute quelques données de départ si la base est vide
    // (mêmes données que l'ancien programme console de l'objectif 2).
    // Id = 0 : c'est la base de données qui génère l'identifiant.
    public static class DonneesInitiales
    {
        public static void Remplir(ClubContext db)
        {
            if (!db.Disciplines.Any())
            {
                db.Disciplines.Add(new Discipline(0, "Football"));
                db.Disciplines.Add(new Discipline(0, "Basketball"));
                db.Disciplines.Add(new Discipline(0, "Natation"));
                db.SaveChanges();
            }
            // Au moins un entraîneur spécialisé pour chaque discipline
            AjouterEntraineurSiAbsent(db, "Ben Salah", "Karim", new DateTime(1985, 4, 12), "karim@club.tn", "20123456", "Football");
            AjouterEntraineurSiAbsent(db, "Mejri", "Sami", new DateTime(1988, 6, 25), "sami@club.tn", "98765432", "Basketball");
            AjouterEntraineurSiAbsent(db, "Jlassi", "Nadia", new DateTime(1990, 9, 3), "nadia@club.tn", "22654321", "Natation");
            if (!db.Membres.Any())
            {
                Membre m1 = new Membre(0, "Trabelsi", "Ahmed", new DateTime(2003, 5, 20), "ahmed@mail.tn", "55111222");
                m1.PayerCotisation();
                Membre m2 = new Membre(0, "Gharbi", "Sara", new DateTime(2004, 9, 3), "sara@mail.tn", "55333444");

                db.Membres.Add(m1);
                db.Membres.Add(m2);
                db.SaveChanges();
            }
        }
        // Ajoute l'entraîneur seulement s'il n'existe aucun entraîneur de cette spécialité
        private static void AjouterEntraineurSiAbsent(ClubContext db, string nom, string prenom, DateTime naissance,
                                                      string email, string telephone, string specialite)
        {
            if (!db.Entraineurs.Any(e => e.Specialite == specialite))
            {
                db.Entraineurs.Add(new Entraineur(0, nom, prenom, naissance, email, telephone, specialite));
                db.SaveChanges();
            }
        }
    }
}
