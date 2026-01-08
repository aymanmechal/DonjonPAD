namespace ProjetPAD.Models
{
    public class Clerc : Hero
    {
        public Clerc(string name)
            : base(name, maxHealth: 130, salary: 12, power: 17)
        { recruitCost = 50;
        }
        
        public override void Rest()
        {
            ReduceFatigue(12);
            Heal(7);
        }
    }
}