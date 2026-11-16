using System.IO;
using Microsoft.EntityFrameworkCore;

namespace GestionClub
{
    // Le DbContext fait le lien entre nos classes C# et la base SQLite (approche Code First).
    // Chaque DbSet correspond à une table.
    public class ClubContext : DbContext
    {
        public DbSet<Personne> Personnes { get; set; }
        public DbSet<Membre> Membres { get; set; }
        public DbSet<Entraineur> Entraineurs { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<SeanceEntrainement> Seances { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Inscription> Inscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + CheminBase());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Héritage : Membre et Entraineur dans UNE seule table "Personnes",
            // la colonne "TypePersonne" indique le type de chaque ligne (stratégie TPH = "généralisation")
            modelBuilder.Entity<Personne>()
                .ToTable("Personnes")
                .HasDiscriminator<string>("TypePersonne")
                .HasValue<Membre>("Membre")
                .HasValue<Entraineur>("Entraineur");

            modelBuilder.Entity<SeanceEntrainement>(e =>
            {
                // Une séance est animée par un entraîneur (clé étrangère EntraineurId)
                // Restrict : impossible de supprimer un entraîneur qui a des séances
                e.HasOne(s => s.Entraineur)
                 .WithMany()
                 .HasForeignKey("EntraineurId")
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);

                // Une séance concerne une discipline (clé étrangère DisciplineId)
                e.HasOne(s => s.Discipline)
                 .WithMany()
                 .HasForeignKey("DisciplineId")
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);

                // Séance <-> Membre (plusieurs-à-plusieurs) : table de liaison "Participations"
                e.HasMany(s => s.Participants)
                 .WithMany()
                 .UsingEntity(j => j.ToTable("Participations"));
            });

            modelBuilder.Entity<Competition>(e =>
            {
                e.HasOne(c => c.Discipline)
                 .WithMany()
                 .HasForeignKey("DisciplineId")
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Inscription = classe d'association entre Membre et Competition.
            // Clé primaire composée (MembreId, CompetitionId) : un membre une seule fois par compétition.
            modelBuilder.Entity<Inscription>(e =>
            {
                e.Property<int>("MembreId");
                e.Property<int>("CompetitionId");
                e.HasKey("MembreId", "CompetitionId");

                e.HasOne(i => i.Membre)
                 .WithMany()
                 .HasForeignKey("MembreId")
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(i => i.Competition)
                 .WithMany(c => c.Inscriptions)
                 .HasForeignKey("CompetitionId")
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }

        // Place club.db dans le dossier du projet (celui qui contient le .csproj) :
        // l'application et les migrations utilisent le même fichier,
        // et il est poussé sur GitHub avec le code.
        public static string CheminBase()
        {
            string depart = Path.GetDirectoryName(typeof(ClubContext).Assembly.Location);
            DirectoryInfo dossier = new DirectoryInfo(depart);

            while (dossier != null && dossier.GetFiles("*.csproj").Length == 0)
            {
                dossier = dossier.Parent;
            }

            string racine = dossier != null ? dossier.FullName : depart;
            return Path.Combine(racine, "club.db");
        }
    }
}
