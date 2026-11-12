using System;

namespace GestionClub
{
    public class Inscription
    {
        private DateTime dateInscription;
        private string resultat;

        private Membre membre;
        private Competition competition;

        public Inscription(Membre membre, Competition competition)
        {
            this.membre = membre;
            this.competition = competition;
            this.dateInscription = DateTime.Now;
            this.resultat = null;
        }

        public DateTime DateInscription
        {
            get { return dateInscription; }
            set { dateInscription = value; }
        }

        public string Resultat
        {
            get { return resultat; }
            set { resultat = value; }
        }

        public Membre Membre
        {
            get { return membre; }
        }

        public Competition Competition
        {
            get { return competition; }
        }
    }
}
