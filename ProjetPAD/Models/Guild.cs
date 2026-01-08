using System.Collections.Generic;

namespace ProjetPAD.Models
{
    public class Guild
    {
        private string name;

        // Équipe active
        private List<Hero> heroes;

        // Réserve
        private List<Hero> reserveHeroes;

        private List<Mission> missions;
        private List<Potion> potions;

        private int gold;
        private int food;

        public void SpendGold(int amount)
        {
            gold -= amount;
            if (gold < 0)
                gold = 0;
        }

        public Guild(string name)
        {
            this.name = name;
            heroes = new List<Hero>();
            reserveHeroes = new List<Hero>();
            missions = new List<Mission>();
            potions = new List<Potion>();
            gold = 100;
            food = 50;
        }

        // ===================
        // GETTERS
        // ===================

        public string GetName() => name;
        public List<Hero> GetHeroes() => heroes;
        public List<Hero> GetReserveHeroes() => reserveHeroes;
        public List<Mission> GetMissions() => missions;
        public List<Potion> GetPotions() => potions;

        public int GetGold() => gold;
        public int GetFood() => food;

        public void SetMissions(List<Mission> missions)
        {
            this.missions = missions;
        }

        // ===================
        // HÉROS
        // ===================

        // Héros de départ
        public void AddHero(Hero hero)
        {
            heroes.Add(hero);
        }

        // 🔥 Réserve → Actif
        public bool AddHeroToActive(Hero hero)
        {
            if (heroes.Count >= 4)
                return false;

            if (!reserveHeroes.Contains(hero))
                return false;

            reserveHeroes.Remove(hero);
            heroes.Add(hero);
            return true;
        }

        // 🔥 Actif → Réserve
        public void MoveHeroToReserve(Hero hero)
        {
            if (hero.IsOnMission())
                return;

            heroes.Remove(hero);
            reserveHeroes.Add(hero);
        }

        // Recrutement
        public void AddHeroToReserve(Hero hero)
        {
            reserveHeroes.Add(hero);
        }

        public void RemoveDeadHeroes()
        {
            heroes.RemoveAll(h => h.GetHealth() <= 0);
        }

        // ===================
        // MISSIONS
        // ===================

        public void ResolveMissionRewards(MissionResult result)
        {
            gold += result.GetGoldGained();
            food += result.GetFoodGained();
            RemoveDeadHeroes();
        }

        // ===================
        // ÉCONOMIE
        // ===================

        public void PaySalaries()
        {
            foreach (Hero hero in heroes)
                gold -= hero.GetSalary();
        }

        public void FeedHeroes()
        {
            foreach (Hero hero in heroes)
            {
                if (food > 0)
                    food--;
                else
                    hero.TakeDamage(5);
            }
        }

        // ===================
        // POTIONS
        // ===================

        public bool BuyPotion(Potion potion)
        {
            if (gold < potion.GetPrice())
                return false;

            gold -= potion.GetPrice();
            potions.Add(potion);
            return true;
        }

        public void UsePotion(Potion potion, Hero hero)
        {
            if (!potions.Contains(potion))
                return;

            hero.Heal(potion.GetHealAmount());
            potions.Remove(potion);
        }

        // ===================
        // DÉFAITE
        // ===================

        public bool IsDefeated()
        {
            if (heroes.Count == 0)
                return true;

            if (gold < 0 || food < 0)
                return true;

            return false;
        }
    }
}
