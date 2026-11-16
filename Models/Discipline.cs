namespace GestionClub
{
    public class Discipline
    {
        private int id;
        private string nom;

        // Constructeur vide pour Entity Framework Core
        private Discipline()
        {
        }

        public Discipline(int id, string nom)
        {
            this.id = id;
            this.nom = nom;
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

        public override string ToString()
        {
            return nom;
        }
    }
}
