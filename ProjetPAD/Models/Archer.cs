namespace ProjetPAD.Models
{
    public class Archer : Hero
    {
        public Archer(string name)
            : base(name, maxHealth: 100, salary: 12, power: 18)
        {
        }
        
        public override void Rest()
        {
            ReduceFatigue(12);
            Heal(7);
        }
    }
}