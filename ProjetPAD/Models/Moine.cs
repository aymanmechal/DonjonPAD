namespace ProjetPAD.Models
{
    public class Moine : Hero
    {
        public Moine(string name)
            : base(name, maxHealth: 75, salary: 12, power: 35)
        { recruitCost = 50;
        }
        
        public override void Rest()
        {
            ReduceFatigue(12);
            Heal(7);
        }
    }
}