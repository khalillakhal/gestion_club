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

            if (!db.Entraineurs.Any())
            {
                db.Entraineurs.Add(new Entraineur(0, "Ben Salah", "Karim", new DateTime(1985, 4, 12), "karim@club.tn", "20123456", "Football"));
                db.Entraineurs.Add(new Entraineur(0, "Jlassi", "Nadia", new DateTime(1990, 9, 3), "nadia@club.tn", "22654321", "Natation"));
                db.SaveChanges();
            }

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
    }
}
