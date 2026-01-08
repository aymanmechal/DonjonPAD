using System.ComponentModel;

namespace ProjetPAD.Models
{
    public class Hero : INotifyPropertyChanged
    {
        // =====================================================
        // Champs privés
        // =====================================================
        private string name;
        private int health;
        private int maxHealth;
        private int fatigue;
        private int salary;
        private int power;
        private bool isOnMission;
        public int Salary => salary;
        public string ClassName => GetType().Name;
        public int RecruitCost => recruitCost;

        
        protected int recruitCost;

        public int GetRecruitCost()
        {
            return recruitCost;
        }


        // =====================================================
        // Constructeur
        // =====================================================
        public Hero(string name, int maxHealth, int salary, int power)
        {
            this.name = name;
            this.maxHealth = maxHealth;
            this.health = maxHealth;
            this.salary = salary;
            this.power = power;
            this.fatigue = 0;
            this.isOnMission = false;
        }

        // =====================================================
        // ? Notification UI
        // =====================================================
        public event PropertyChangedEventHandler? PropertyChanged;

        private void Notify(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        // =====================================================
        // ? MÉTHODES EXISTANTES (ON NE LES SUPPRIME PAS)
        // =====================================================
        public string GetName() => name;
        public int GetHealth() => health;
        public int GetMaxHealth() => maxHealth;
        public int GetFatigue() => fatigue;
        public int GetSalary() => salary;
        public int GetPower() => power;

        public bool IsOnMission() => isOnMission;

        public void SetOnMission(bool value)
        {
            isOnMission = value;
            Notify(nameof(IsOnMissionProperty));
        }

        // =====================================================
        // ? PROPRIÉTÉS POUR AVALONIA (EN PLUS)
        // =====================================================
        public string Name => name;
        public int Health => health;
        public int MaxHealth => maxHealth;
        public int Fatigue => fatigue;
        public int Power => power;
        public bool IsOnMissionProperty => isOnMission;

        // =====================================================
        // Logique métier (notifiée)
        // =====================================================
        public void TakeDamage(int amount)
        {
            health -= amount;
            if (health < 0) health = 0;
            Notify(nameof(Health));
        }

        public void Heal(int amount)
        {
            health += amount;
            if (health > maxHealth) health = maxHealth;
            Notify(nameof(Health));
        }

        public void AddFatigue(int amount)
        {
            fatigue += amount;
            Notify(nameof(Fatigue));
        }

        public void ReduceFatigue(int amount)
        {
            fatigue -= amount;
            if (fatigue < 0) fatigue = 0;
            Notify(nameof(Fatigue));
        }

        public virtual void Rest()
        {
            ReduceFatigue(10);
            Heal(5);
        }
    }
}
