namespace ProjetPAD.Models
{
    public class Mission
    {
        private string name;
        private int difficulty;
        private int rewardFood;
        private int rewardGold;

        public Mission(string name, int difficulty, int rewardFood, int rewardGold)
        {
            this.name = name;
            this.difficulty = difficulty < 0 ? 0 : difficulty;
            this.rewardFood = rewardFood;
            this.rewardGold = rewardGold;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string value)
        {
            name = value;
        }

        public int GetDifficulty()
        {
            return difficulty;
        }

        public void SetDifficulty(int value)
        {
            difficulty = value < 0 ? 0 : value;
        }

        public int GetRewardFood()
        {
            return rewardFood;
        }

        public void SetRewardFood(int value)
        {
            rewardFood = value;
        }

        public int GetRewardGold()
        {
            return rewardGold;
        }

        public void SetRewardGold(int value)
        {
            rewardGold = value;
        }

        public MissionResult Resolve(Hero hero)
        {
            bool success;
            int damageTaken;
            int foodGained;
            int goldGained;

            if (hero.GetPower() >= difficulty)
            {
                success = true;
                damageTaken = difficulty / 2;
                if (damageTaken < 1) damageTaken = 1;

                foodGained = rewardFood;
                goldGained = rewardGold;
            }
            else
            {
                success = false;
                damageTaken = difficulty;
                if (damageTaken < 1) damageTaken = 1;

                foodGained = 0;
                goldGained = 0;
            }

            hero.TakeDamage(damageTaken);
            hero.AddFatigue(5);

            return new MissionResult(success, damageTaken, foodGained, goldGained);
        }
    }
}
