namespace ProjetPAD.Models
{
    public class Assassin : Hero
    {
        public Assassin(string name)
            : base(name, maxHealth: 30, salary: 24, power: 40)
        { recruitCost = 50;
        }
        
        public override void Rest()
        {
            ReduceFatigue(12);
            Heal(7);
        }
    }
}