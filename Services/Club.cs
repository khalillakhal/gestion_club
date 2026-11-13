using System;
using System.Collections.Generic;

namespace GestionClub
{
    public class Club
    {
        private string nom;
        private string adresse;

        private List<Membre> membres;
        private List<Entraineur> entraineurs;
        private List<Discipline> disciplines;
        private List<SeanceEntrainement> seances;
        private List<Competition> competitions;

        public Club(string nom, string adresse)
        {
            this.nom = nom;
            this.adresse = adresse;
            this.membres = new List<Membre>();
            this.entraineurs = new List<Entraineur>();
            this.disciplines = new List<Discipline>();
            this.seances = new List<SeanceEntrainement>();
            this.competitions = new List<Competition>();
        }

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public string Adresse
        {
            get { return adresse; }
            set { adresse = value; }
        }

        public List<Membre> Membres
        {
            get { return membres; }
        }

        public List<Entraineur> Entraineurs
        {
            get { return entraineurs; }
        }

        public List<Discipline> Disciplines
        {
            get { return disciplines; }
        }

        public List<SeanceEntrainement> Seances
        {
            get { return seances; }
        }

        public List<Competition> Competitions
        {
            get { return competitions; }
        }

        public void AjouterMembre(Membre m)
        {
            if (m == null)
            {
                throw new ArgumentNullException(nameof(m));
            }

            if (RechercherMembre(m.Id) != null)
            {
                throw new InvalidOperationException($"Un membre avec l'identifiant {m.Id} existe déjà.");
            }

            membres.Add(m);
        }

        public void SupprimerMembre(int id)
        {
            Membre m = RechercherMembre(id);

            if (m == null)
            {
                throw new InvalidOperationException($"Aucun membre avec l'identifiant {id}.");
            }

            membres.Remove(m);
        }

        public Membre RechercherMembre(int id)
        {
            foreach (Membre m in membres)
            {
                if (m.Id == id)
                {
                    return m;
                }
            }

            return null;
        }

        public void AjouterSeance(SeanceEntrainement s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            seances.Add(s);
        }

        public void AjouterCompetition(Competition c)
        {
            if (c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }

            competitions.Add(c);
        }
    }
}
