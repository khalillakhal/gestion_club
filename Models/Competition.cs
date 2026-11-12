using System;
using System.Collections.Generic;

namespace GestionClub
{
    public class Competition
    {
        private int id;
        private string nom;
        private DateTime date;
        private string lieu;
        private string categorie;

        private Discipline discipline;
        private List<Inscription> inscriptions;

        public Competition(int id, string nom, DateTime date, string lieu, string categorie, Discipline discipline)
        {
            this.id = id;
            this.nom = nom;
            this.date = date;
            this.lieu = lieu;
            this.categorie = categorie;
            this.discipline = discipline;
            this.inscriptions = new List<Inscription>();
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

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public string Lieu
        {
            get { return lieu; }
            set { lieu = value; }
        }

        public string Categorie
        {
            get { return categorie; }
            set { categorie = value; }
        }

        public Discipline Discipline
        {
            get { return discipline; }
        }

        public IReadOnlyList<Inscription> Inscriptions
        {
            get { return inscriptions; }
        }

        public Inscription InscrireMembre(Membre m)
        {
            if (m == null)
            {
                throw new ArgumentNullException(nameof(m));
            }

            foreach (Inscription i in inscriptions)
            {
                if (i.Membre == m)
                {
                    throw new InvalidOperationException("Ce membre est déjà inscrit à cette compétition.");
                }
            }

            Inscription nouvelle = new Inscription(m, this);
            inscriptions.Add(nouvelle);
            return nouvelle;
        }

        public void EnregistrerResultat(Membre m, string resultat)
        {
            foreach (Inscription i in inscriptions)
            {
                if (i.Membre == m)
                {
                    i.Resultat = resultat;
                    return;
                }
            }

            throw new InvalidOperationException("Ce membre n'est pas inscrit à cette compétition.");
        }

        public override string ToString()
        {
            return $"Compétition #{id} - {nom} ({discipline}, {categorie}) le {date:dd/MM/yyyy} à {lieu} | {inscriptions.Count} inscrit(s)";
        }
    }
}
