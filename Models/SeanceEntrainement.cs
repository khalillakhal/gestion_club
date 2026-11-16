using System;
using System.Collections.Generic;

namespace GestionClub
{
    public class SeanceEntrainement
    {
        private int id;   // généré automatiquement par la base de données
        private DateTime date;
        private TimeSpan duree;
        private string lieu;
        private bool annulee;

        private Entraineur entraineur;
        private Discipline discipline;
        private List<Membre> participants = new List<Membre>();

        // Constructeur vide pour Entity Framework Core
        private SeanceEntrainement()
        {
        }

        public SeanceEntrainement(DateTime date, TimeSpan duree, string lieu, Entraineur entraineur, Discipline discipline)
        {
            this.date = date;
            this.duree = duree;
            this.lieu = lieu;
            this.annulee = false;
            this.entraineur = entraineur;
            this.discipline = discipline;
        }

        public int Id
        {
            get { return id; }
            private set { id = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public TimeSpan Duree
        {
            get { return duree; }
            set { duree = value; }
        }

        public string Lieu
        {
            get { return lieu; }
            set { lieu = value; }
        }

        public bool Annulee
        {
            get { return annulee; }
            private set { annulee = value; }
        }

        public Entraineur Entraineur
        {
            get { return entraineur; }
            set { entraineur = value; }
        }

        public Discipline Discipline
        {
            get { return discipline; }
            set { discipline = value; }
        }

        public IReadOnlyList<Membre> Participants
        {
            get { return participants; }
        }

        public void AjouterParticipant(Membre m)
        {
            if (m == null)
            {
                throw new ArgumentNullException(nameof(m));
            }

            if (annulee)
            {
                throw new InvalidOperationException("Impossible d'ajouter un participant à une séance annulée.");
            }

            if (participants.Contains(m))
            {
                throw new InvalidOperationException("Ce membre participe déjà à cette séance.");
            }

            participants.Add(m);
        }

        public void Annuler()
        {
            annulee = true;
        }

        public override string ToString()
        {
            string etat = annulee ? " [ANNULÉE]" : "";
            return $"Séance #{id} - {discipline} le {date:dd/MM/yyyy HH:mm} ({duree.TotalMinutes} min) à {lieu} " +
                   $"| Entraîneur : {entraineur.Prenom} {entraineur.Nom} | {participants.Count} participant(s){etat}";
        }
    }
}
