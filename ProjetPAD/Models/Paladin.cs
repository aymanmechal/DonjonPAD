namespace ProjetPAD.Models
{
    public class Paladin : Hero
    {
        public Paladin(string name)
            : base(name, maxHealth: 150, salary: 12, power: 10)
        { recruitCost = 50;
        }
        
        public override void Rest()
        {
            ReduceFatigue(12);
            Heal(7);
        }
    }
}