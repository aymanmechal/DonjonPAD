namespace ProjetPAD.Models
{
    public class Hero
    {
        // Champs privés
        private string name;
        private int health;
        private int maxHealth;
        private int fatigue;
        private int salary;
        private int power;

        // Constructeur
        public Hero(string name, int maxHealth, int salary, int power)
        {
            this.name = name;
            this.maxHealth = maxHealth;
            this.health = maxHealth;
            this.salary = salary;
            this.power = power;
            this.fatigue = 0;
        }

        // Getters / Setters
        public string GetName()
        {
            return name;
        }

        public void SetName(string value)
        {
            name = value;
        }

        public int GetHealth()
        {
            return health;
        }

        public void SetHealth(int value)
        {
            health = value;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public void SetMaxHealth(int value)
        {
            maxHealth = value;
        }

        public int GetFatigue()
        {
            return fatigue;
        }

        public void SetFatigue(int value)
        {
            fatigue = value;
        }

        public int GetSalary()
        {
            return salary;
        }

        public void SetSalary(int value)
        {
            salary = value;
        }

        public int GetPower()
        {
            return power;
        }

        public void SetPower(int value)
        {
            power = value;
        }

        // Méthodes
        public void TakeDamage(int amount)
        {
            health = health - amount;
            if (health < 0)
            {
                health = 0;
            }
        }

        public void Heal(int amount)
        {
            health = health + amount;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }

        public void AddFatigue(int amount)
        {
            fatigue = fatigue + amount;
        }

        public void ReduceFatigue(int amount)
        {
            fatigue = fatigue - amount;
            if (fatigue < 0)
            {
                fatigue = 0;
            }
        }

        // Méthode virtuelle
        public virtual void Rest()
        {
            ReduceFatigue(10);
            Heal(5);
        }
    }
}