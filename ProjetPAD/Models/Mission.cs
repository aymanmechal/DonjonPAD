using System;
using System.Collections.Generic;

namespace ProjetPAD.Models
{
    public class Mission
    {
        private string name;
        private int difficulty;
        private int rewardFood;
        private int rewardGold;
        public int RewardFood => rewardFood;
        public int RewardGold => rewardGold;
        public string Name { get => name; }
        public int Difficulty { get => difficulty; }
        public string Description { get; set; } = "Pas de description";


        // 🆕 Gestion du temps
        private int duration;          // durée totale (en phases)
        private int remainingPhases;   // phases restantes

        // 🆕 Héros envoyés
        private List<Hero> assignedHeroes = new();

        public Mission(string name, int difficulty, int rewardFood, int rewardGold, int duration)
        {
            this.name = name;
            this.difficulty = difficulty < 0 ? 0 : difficulty;
            this.rewardFood = rewardFood;
            this.rewardGold = rewardGold;

            this.duration = duration <= 0 ? 1 : duration;
            this.remainingPhases = this.duration;
        }

        // =======================
        // ⏳ TEMPS
        // =======================

        public void AdvancePhase()
        {
            remainingPhases--;
        }

        public bool IsFinished()
        {
            return remainingPhases <= 0;
        }

        public int GetRemainingPhases()
        {
            return remainingPhases;
        }

        // =======================
        // 👥 HÉROS
        // =======================

        public void AssignHeroes(List<Hero> heroes)
        {
            assignedHeroes = heroes;

            foreach (Hero hero in heroes)
            {
                hero.SetOnMission(true);
            }
        }

        public List<Hero> GetAssignedHeroes()
        {
            return assignedHeroes;
        }

        // =======================
        // 🎯 CHANCES DE SUCCÈS
        // =======================

        public double GetSuccessChance(List<Hero> heroes)
        {
            if (heroes == null || heroes.Count == 0)
                return 0;

            int totalPower = 0;
            foreach (Hero hero in heroes)
                totalPower += hero.GetPower();

            if (difficulty <= 0)
                return 95;

            double chance = ((double)totalPower / difficulty) * 50;
            return Math.Clamp(chance, 10, 95);
        }

        // =======================
        // 🏁 RÉSOLUTION FINALE
        // =======================

        public MissionResult Resolve()
        {
            if (assignedHeroes == null || assignedHeroes.Count == 0)
                return new MissionResult(false, 0, 0, 0);

            int totalPower = 0;
            foreach (Hero hero in assignedHeroes)
                totalPower += hero.GetPower();

            if (difficulty <= 0)
                difficulty = 1;

            double successChance = ((double)totalPower / difficulty) * 50;
            successChance = Math.Clamp(successChance, 10, 95);

            Random rand = new Random();
            bool success = rand.NextDouble() * 100 < successChance;

            int damageTaken;
            int foodGained = 0;
            int goldGained = 0;

            if (success)
            {
                foodGained = rewardFood;
                goldGained = rewardGold;
                damageTaken = difficulty / assignedHeroes.Count;
            }
            else
            {
                damageTaken = difficulty;
            }

            foreach (Hero hero in assignedHeroes)
            {
                hero.TakeDamage(damageTaken);
                hero.AddFatigue(5);
                hero.SetOnMission(false); // 🆕 retour de mission
            }

            return new MissionResult(success, damageTaken, foodGained, goldGained);
        }
        public double GetSuccessChanceForHeroes(List<Hero> heroes)
        {
            return GetSuccessChance(heroes);
        }


        // =======================
        // 🔎 GETTERS
        // =======================

        public string GetName() => name;
        public int GetDifficulty() => difficulty;
        public int GetRewardFood() => rewardFood;
        public int GetRewardGold() => rewardGold;
        public int GetDuration() => duration;
    }
}
