using System;

namespace GestionClub
{
    public abstract class Personne
    {
        private int id;
        private string nom;
        private string prenom;
        private DateTime dateNaissance;
        private string email;
        private string telephone;

        protected Personne(int id, string nom, string prenom, DateTime dateNaissance, string email, string telephone)
        {
            this.id = id;
            this.nom = nom;
            this.prenom = prenom;
            this.dateNaissance = dateNaissance;
            this.email = email;
            this.telephone = telephone;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public string Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }

        public DateTime DateNaissance
        {
            get { return dateNaissance; }
            set { dateNaissance = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        public virtual string AfficherInfos()
        {
            return $"{id} - {prenom} {nom} | Né(e) le {dateNaissance:dd/MM/yyyy} | {email} | {telephone}";
        }
    }
}
