namespace ProjetPAD.Models
{
    public class Guerrier : Hero
    {
        public Guerrier(string name)
        
            : base(name, maxHealth: 120, salary: 15, power: 20)
        
        {
            recruitCost = 50;
        }

        public override void Rest()
        {
            ReduceFatigue(8);
            Heal(6);
        }
        
    }
}