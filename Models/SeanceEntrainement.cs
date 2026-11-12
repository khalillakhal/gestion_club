using System;
using System.Collections.Generic;

namespace GestionClub
{
    public class SeanceEntrainement
    {
        private static int prochainId = 1;

        private int id;
        private DateTime date;
        private TimeSpan duree;
        private string lieu;
        private bool annulee;

        private Entraineur entraineur;
        private Discipline discipline;
        private List<Membre> participants;

        public SeanceEntrainement(DateTime date, TimeSpan duree, string lieu, Entraineur entraineur, Discipline discipline)
        {
            this.id = prochainId++;
            this.date = date;
            this.duree = duree;
            this.lieu = lieu;
            this.annulee = false;
            this.entraineur = entraineur;
            this.discipline = discipline;
            this.participants = new List<Membre>();
        }

        public int Id
        {
            get { return id; }
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
        }

        public Entraineur Entraineur
        {
            get { return entraineur; }
        }

        public Discipline Discipline
        {
            get { return discipline; }
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
