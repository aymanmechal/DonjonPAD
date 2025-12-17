namespace ProjetPAD.Models
{
    public class Archer : Hero
    {
        // Constructeur
        public Archer(string name)
            : base(name, maxHealth: 100, salary: 12, power: 18)
        {
        }

        // Exemple de spécialisation du repos
        public override void Rest()
        {
            ReduceFatigue(12);
            Heal(7);
        }
    }
}