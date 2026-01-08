namespace ProjetPAD.Models
{
    public class Mage : Hero
    {
        
        public Mage(string name)
            : base(name, maxHealth: 80, salary: 20, power: 25)
        { recruitCost = 50;
        }

        public override void Rest()
        {
            ReduceFatigue(15);
            Heal(10);
        }
    }
}