namespace ProjetPAD.Models
{
    public class Rodeur : Hero
    {
        public Rodeur(string name)
            : base(name, maxHealth: 100, salary: 12, power: 18)
        { recruitCost = 50;
        }
        
        public override void Rest()
        {
            ReduceFatigue(12);
            Heal(7);
        }
    }
}