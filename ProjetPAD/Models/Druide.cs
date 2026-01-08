namespace ProjetPAD.Models
{
    public class Druide : Hero
    {
        public Druide(string name)
            : base(name, maxHealth: 110, salary: 12, power: 22)
        { recruitCost = 50;
        }
        
        public override void Rest()
        {
            ReduceFatigue(12);
            Heal(7);
        }
    }
}