namespace ProjetPAD.Models
{
    public class Roublard : Hero
    {
        public Roublard(string name)
            : base(name, maxHealth: 60, salary: 5, power: 30)
        { recruitCost = 50;
        }
        
        public override void Rest()
        {
            ReduceFatigue(12);
            Heal(7);
        }
    }
}