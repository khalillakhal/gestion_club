using System;

namespace GestionClub
{
    public class Entraineur : Personne
    {
        private string specialite;

        public Entraineur(int id, string nom, string prenom, DateTime dateNaissance, string email, string telephone, string specialite)
            : base(id, nom, prenom, dateNaissance, email, telephone)
        {
            this.specialite = specialite;
        }

        public string Specialite
        {
            get { return specialite; }
            set { specialite = value; }
        }

        public SeanceEntrainement PlanifierSeance(DateTime date, TimeSpan duree, string lieu, Discipline discipline)
        {
            if (discipline == null)
            {
                throw new ArgumentNullException(nameof(discipline));
            }

            return new SeanceEntrainement(date, duree, lieu, this, discipline);
        }

        public override string AfficherInfos()
        {
            return base.AfficherInfos() + $" | Spécialité : {specialite}";
        }
    }
}
