namespace ProjetPAD.Models
{
    public class Mage : Hero
    {
        // Constructeur
        public Mage(string name)
            : base(name, maxHealth: 80, salary: 20, power: 25)
        {
        }

        // Exemple de spécialisation du repos
        public override void Rest()
        {
            ReduceFatigue(15);
            Heal(10);
        }
    }
}