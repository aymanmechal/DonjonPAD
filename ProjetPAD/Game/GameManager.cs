using System.Collections.Generic;
using ProjetPAD.Models;

namespace ProjetPAD.Game
{
    public class GameManager
    {
        private static GameManager? instance;
        private Guild playerGuild;
        private int currentPhase;

        public static GameManager GetInstance()
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance!;
        }

        private GameManager()
        {
            playerGuild = new Guild("Guilde");
            currentPhase = 0;
            
            playerGuild.AddHero(new Guerrier("Guerrier"));
            playerGuild.AddHero(new Mage("Mage"));
            playerGuild.AddHero(new Archer("Archer"));

            playerGuild.AddMission(new Mission("Chasse aux loups", 10, 5, 10));
            playerGuild.AddMission(new Mission("Protection caravane", 18, 3, 20));
            playerGuild.AddMission(new Mission("Donjon abandonné", 25, 0, 40));
        }

        public Guild GetPlayerGuild()
        {
            return playerGuild;
        }

        public int GetCurrentPhase()
        {
            return currentPhase;
        }

        public void NextPhase()
        {
            if (currentPhase == 0)
            {
                PhaseMatin();
                currentPhase = 1;
            }
            else if (currentPhase == 1)
            {
                PhaseApresMidi();
                currentPhase = 2;
            }
            else
            {
                PhaseSoir();
                currentPhase = 0;
            }
        }

        public void PhaseMatin()
        {
            playerGuild.PaySalaries();
            playerGuild.FeedHeroes();
            playerGuild.RemoveDeadHeroes();
        }

        public void PhaseApresMidi()
        {
            
        }

        public void PhaseSoir()
        {
            foreach (Hero hero in playerGuild.GetHeroes())
            {
                hero.Rest();
            }

            playerGuild.RemoveDeadHeroes();
        }

        public MissionResult DoMission(int heroIndex, int missionIndex)
        {
            List<Hero> heroes = playerGuild.GetHeroes();
            List<Mission> missions = playerGuild.GetMissions();

            if (heroIndex < 0 || heroIndex >= heroes.Count)
                return new MissionResult(false, 0, 0, 0);

            if (missionIndex < 0 || missionIndex >= missions.Count)
                return new MissionResult(false, 0, 0, 0);

            Hero hero = heroes[heroIndex];
            Mission mission = missions[missionIndex];

            return playerGuild.ResolveMission(mission, hero);
        }

        public bool CheckGameOver()
        {
            return playerGuild.IsDefeated();
        }
    }
}
