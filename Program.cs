using System;

namespace GestionClub
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Club club = new Club("Club Sportif de Carthage", "Carthage, Tunis");

            Discipline football = new Discipline(1, "Football");
            club.Disciplines.Add(football);

            Entraineur coach = new Entraineur(100, "Ben Salah", "Karim", new DateTime(1985, 4, 12), "karim@club.tn", "20123456", "Football");
            club.Entraineurs.Add(coach);

            Membre m1 = new Membre(1, "Trabelsi", "Ahmed", new DateTime(2003, 5, 20), "ahmed@mail.tn", "55111222");
            Membre m2 = new Membre(2, "Gharbi", "Sara", new DateTime(2004, 9, 3), "sara@mail.tn", "55333444");
            club.AjouterMembre(m1);
            club.AjouterMembre(m2);
            m1.PayerCotisation();

            SeanceEntrainement seance = coach.PlanifierSeance(new DateTime(2026, 11, 20, 18, 0, 0), TimeSpan.FromMinutes(90), "Stade municipal", football);
            seance.AjouterParticipant(m1);
            seance.AjouterParticipant(m2);
            club.AjouterSeance(seance);

            Competition tournoi = new Competition(1, "Tournoi de printemps", new DateTime(2027, 3, 15), "Tunis", "Senior", football);
            club.AjouterCompetition(tournoi);
            tournoi.InscrireMembre(m1);
            tournoi.EnregistrerResultat(m1, "1ère place");

            Console.WriteLine("=== " + club.Nom + " ===");
            Console.WriteLine();

            Console.WriteLine("--- Membres ---");
            foreach (Membre m in club.Membres)
            {
                Console.WriteLine(m.AfficherInfos());
            }

            Console.WriteLine();
            Console.WriteLine("--- Planning des entraînements ---");
            foreach (SeanceEntrainement s in club.Seances)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine();
            Console.WriteLine("--- Compétitions ---");
            foreach (Competition c in club.Competitions)
            {
                Console.WriteLine(c);
                foreach (Inscription i in c.Inscriptions)
                {
                    Console.WriteLine($"   {i.Membre.Prenom} {i.Membre.Nom} -> {i.Resultat ?? "pas de résultat"}");
                }
            }
        }
    }
}
