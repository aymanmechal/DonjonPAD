using System.Collections.Generic;

namespace ProjetPAD.Models
{
    public class Guild
    {
        private string name;
        private List<Hero> heroes;
        private int gold;
        private int food;
        private List<Mission> missions;

        public Guild(string name)
        {
            this.name = name;
            heroes = new List<Hero>();
            missions = new List<Mission>();
            gold = 100;
            food = 50;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string value)
        {
            name = value;
        }

        public List<Hero> GetHeroes()
        {
            return heroes;
        }

        public int GetGold()
        {
            return gold;
        }

        public int GetFood()
        {
            return food;
        }

        public List<Mission> GetMissions()
        {
            return missions;
        }

        public void AddHero(Hero hero)
        {
            heroes.Add(hero);
        }

        public void RemoveDeadHeroes()
        {
            for (int i = heroes.Count - 1; i >= 0; i--)
            {
                if (heroes[i].GetHealth() <= 0)
                {
                    heroes.RemoveAt(i);
                }
            }
        }

        public void AddMission(Mission mission)
        {
            missions.Add(mission);
        }

        public void PaySalaries()
        {
            foreach (Hero hero in heroes)
            {
                gold -= hero.GetSalary();
            }
        }

        public void FeedHeroes()
        {
            foreach (Hero hero in heroes)
            {
                if (food > 0)
                {
                    food--;
                }
                else
                {
                    hero.TakeDamage(5);
                }
            }
        }

        public MissionResult ResolveMission(Mission mission, Hero hero)
        {
            MissionResult result = mission.Resolve(hero);

            gold += result.GetGoldGained();
            food += result.GetFoodGained();

            RemoveDeadHeroes();

            return result;
        }

        public bool IsDefeated()
        {
            if (heroes.Count == 0)
                return true;

            if (food < 0 || gold < 0)
                return true;

            return false;
        }
    }
}
