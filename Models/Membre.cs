using System;

namespace GestionClub
{
    public class Membre : Personne
    {
        private DateTime dateInscription;
        private bool cotisationPayee;

        public Membre(int id, string nom, string prenom, DateTime dateNaissance, string email, string telephone)
            : base(id, nom, prenom, dateNaissance, email, telephone)
        {
            dateInscription = DateTime.Now;
            cotisationPayee = false;
        }

        public DateTime DateInscription
        {
            get { return dateInscription; }
            set { dateInscription = value; }
        }

        public bool CotisationPayee
        {
            get { return cotisationPayee; }
            set { cotisationPayee = value; }
        }

        public void PayerCotisation()
        {
            cotisationPayee = true;
        }

        public override string AfficherInfos()
        {
            string etat = cotisationPayee ? "payée" : "impayée";
            return base.AfficherInfos() + $" | Inscrit le {dateInscription:dd/MM/yyyy} | Cotisation {etat}";
        }
    }
}
