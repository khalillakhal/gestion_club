using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GestionClub
{
    // Objectif 3 : le Club ne garde plus ses données dans des listes en mémoire,
    // il les lit et les enregistre dans la base SQLite via le ClubContext.
    public class Club
    {
        private string nom;
        private string adresse;
        private ClubContext db;

        public Club(string nom, string adresse, ClubContext db)
        {
            this.nom = nom;
            this.adresse = adresse;
            this.db = db;
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

        // ===================== Lecture =====================

        public List<Membre> Membres
        {
            get { return db.Membres.OrderBy(m => m.Nom).ThenBy(m => m.Prenom).ToList(); }
        }

        public List<Entraineur> Entraineurs
        {
            get { return db.Entraineurs.OrderBy(e => e.Nom).ToList(); }
        }

        public List<Discipline> Disciplines
        {
            get { return db.Disciplines.OrderBy(d => d.Nom).ToList(); }
        }

        public List<SeanceEntrainement> Seances
        {
            get
            {
                // Include : charge aussi l'entraîneur, la discipline et les participants
                return db.Seances
                    .Include(s => s.Entraineur)
                    .Include(s => s.Discipline)
                    .Include(s => s.Participants)
                    .OrderBy(s => s.Date)
                    .ToList();
            }
        }

        public List<Competition> Competitions
        {
            get
            {
                return db.Competitions
                    .Include(c => c.Discipline)
                    .Include(c => c.Inscriptions)
                    .OrderBy(c => c.Date)
                    .ToList();
            }
        }

        // ===================== Membres =====================

        public void AjouterMembre(Membre m)
        {
            if (m == null)
            {
                throw new ArgumentNullException(nameof(m));
            }

            db.Membres.Add(m);
            db.SaveChanges();           // INSERT (l'Id est généré par la base)
        }

        public void ModifierMembre(Membre m)
        {
            if (m == null)
            {
                throw new ArgumentNullException(nameof(m));
            }

            // Le membre a été lu avec ce même contexte : EF Core a suivi les modifications.
            // Update() ne sert que si l'objet ne vient pas de ce contexte.
            if (db.Entry(m).State == EntityState.Detached)
            {
                db.Membres.Update(m);
            }
            db.SaveChanges();           // UPDATE
        }

        public void SupprimerMembre(int id)
        {
            Membre m = RechercherMembre(id);

            if (m == null)
            {
                throw new InvalidOperationException($"Aucun membre avec l'identifiant {id}.");
            }

            db.Membres.Remove(m);
            db.SaveChanges();           // DELETE
        }

        public Membre RechercherMembre(int id)
        {
            return db.Membres.Find(id); // null si introuvable
        }

        // ===================== Séances =====================

        public SeanceEntrainement RechercherSeance(int id)
        {
            return db.Seances
                .Include(s => s.Entraineur)
                .Include(s => s.Discipline)
                .Include(s => s.Participants)
                .FirstOrDefault(s => s.Id == id);
        }

        public void AjouterSeance(SeanceEntrainement s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            db.Seances.Add(s);
            db.SaveChanges();
        }

        public void ModifierSeance(SeanceEntrainement s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (db.Entry(s).State == EntityState.Detached)
            {
                db.Seances.Update(s);
            }
            db.SaveChanges();
        }

        public void SupprimerSeance(int id)
        {
            SeanceEntrainement s = db.Seances.Find(id);

            if (s == null)
            {
                throw new InvalidOperationException($"Aucune séance avec l'identifiant {id}.");
            }

            db.Seances.Remove(s);
            db.SaveChanges();
        }

        // ===================== Compétitions =====================

        public void AjouterCompetition(Competition c)
        {
            if (c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }

            db.Competitions.Add(c);
            db.SaveChanges();
        }
    }
}
