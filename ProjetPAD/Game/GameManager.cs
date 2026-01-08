using System;
using System.Collections.Generic;
using System.Linq;
using ProjetPAD.Models;

namespace ProjetPAD.Game
{
    public class GameManager
    {
        private static GameManager? instance;

        private Guild playerGuild;
        private int currentPhase;

        // 🎯 Missions possibles par phase
        private Dictionary<int, List<Mission>> missionPools = new();

        // ⏳ Missions en cours
        private List<Mission> activeMissions = new();

        private Random rng = new Random();

        public static GameManager GetInstance()
        {
            instance ??= new GameManager();
            return instance;
        }

        private GameManager()
        {
            playerGuild = new Guild("Guilde");
            currentPhase = 0;

            // ✅ HÉROS DE DÉPART (équipe active)
            playerGuild.AddHeroToActive(new Guerrier("Guerrier"));
            playerGuild.AddHeroToActive(new Mage("Mage"));
            playerGuild.AddHeroToActive(new Rodeur("Archer"));

            InitMissionPools();
            GeneratePhaseMissions();
        }

        // =======================
        // 🎯 MISSIONS
        // =======================

        private void InitMissionPools()
        {
            missionPools[0] = new List<Mission>
            {
                new Mission("Chasse aux loups", 8, 10, 15, 1),
                new Mission("Escorter un fermier", 6, 8, 10, 1),
                new Mission("Nettoyer une cave", 7, 12, 12, 2),
                new Mission("Patrouille locale", 5, 6, 8, 1),
                new Mission("Livraison urgente", 6, 10, 10, 2),
                new Mission("Exploration bois", 9, 15, 20, 2),
                new Mission("Récolte dangereuse", 7, 12, 15, 1),
                new Mission("Garde de nuit", 8, 10, 18, 1),
                new Mission("Protection village", 10, 20, 25, 3),
                new Mission("Ruines anciennes", 12, 25, 30, 3),
            };

            missionPools[1] = new List<Mission>
            {
                new Mission("Chasser une bête", 14, 30, 40, 2),
                new Mission("Donjon mineur", 16, 40, 50, 3),
                new Mission("Protection caravane", 15, 35, 45, 2),
                new Mission("Sabotage bandits", 18, 50, 60, 3),
                new Mission("Espionnage", 13, 20, 30, 1),
                new Mission("Nettoyage marais", 17, 45, 55, 3),
                new Mission("Fortin abandonné", 19, 60, 70, 3),
                new Mission("Traque hors-la-loi", 16, 40, 50, 2),
                new Mission("Reliques perdues", 20, 70, 80, 4),
                new Mission("Poste avancé", 18, 55, 65, 2),
            };

            missionPools[2] = new List<Mission>
            {
                new Mission("Dragon mineur", 25, 120, 150, 4),
                new Mission("Donjon ancien", 22, 90, 110, 4),
                new Mission("Invasion gobeline", 20, 80, 100, 3),
                new Mission("Forteresse ennemie", 26, 130, 160, 5),
                new Mission("Rituel interdit", 24, 100, 120, 4),
                new Mission("Chasse légendaire", 23, 95, 115, 3),
                new Mission("Siège prolongé", 28, 150, 180, 5),
                new Mission("Artefact maudit", 21, 85, 105, 4),
                new Mission("Protéger la capitale", 60, 200, 250, 5),
                new Mission("Boss final", 80, 300, 400, 6),
            };
        }

        private void GeneratePhaseMissions()
        {
            if (!missionPools.ContainsKey(currentPhase))
                return;

            playerGuild.SetMissions(
                missionPools[currentPhase]
                    .OrderBy(x => rng.Next())
                    .Take(3)
                    .ToList()
            );
        }

        // =======================
        // ⏳ PHASES
        // =======================

        public void NextPhase()
        {
            UpdateActiveMissions();

            switch (currentPhase)
            {
                case 0:
                    PhaseMatin();
                    currentPhase = 1;
                    break;
                case 1:
                    PhaseApresMidi();
                    currentPhase = 2;
                    break;
                default:
                    PhaseSoir();
                    currentPhase = 0;
                    break;
            }

            GeneratePhaseMissions();
        }

        private void UpdateActiveMissions()
        {
            for (int i = activeMissions.Count - 1; i >= 0; i--)
            {
                Mission mission = activeMissions[i];
                mission.AdvancePhase();

                if (mission.IsFinished())
                {
                    MissionResult result = mission.Resolve();
                    playerGuild.ResolveMissionRewards(result);
                    activeMissions.RemoveAt(i);
                }
            }
        }

        // =======================
        // 🌅 PHASES
        // =======================

        private void PhaseMatin()
        {
            playerGuild.PaySalaries();
            playerGuild.FeedHeroes();
            playerGuild.RemoveDeadHeroes();
        }

        private void PhaseApresMidi()
        {
            // phase neutre
        }

        private void PhaseSoir()
        {
            foreach (Hero hero in playerGuild.GetHeroes())
            {
                if (!hero.IsOnMission())
                    hero.Rest();
            }

            playerGuild.RemoveDeadHeroes();
        }

        // =======================
        // 🚀 LANCER UNE MISSION
        // =======================

        public bool StartMission(int missionIndex, List<int> heroIndexes)
        {
            var missions = playerGuild.GetMissions();
            var heroes = playerGuild.GetHeroes();

            if (missionIndex < 0 || missionIndex >= missions.Count)
                return false;

            List<Hero> selectedHeroes = new();

            foreach (int index in heroIndexes)
            {
                if (index >= 0 && index < heroes.Count && !heroes[index].IsOnMission())
                    selectedHeroes.Add(heroes[index]);
            }

            if (selectedHeroes.Count == 0)
                return false;

            Mission mission = missions[missionIndex];
            mission.AssignHeroes(selectedHeroes);

            activeMissions.Add(mission);
            missions.Remove(mission);

            return true;
        }

        // =======================
        // 🔎 GETTERS
        // =======================

        public Guild GetPlayerGuild() => playerGuild;
        public int GetCurrentPhase() => currentPhase;
        public bool CheckGameOver() => playerGuild.IsDefeated();

        public IReadOnlyList<Hero> GetActiveHeroes()
            => playerGuild.GetHeroes();

        public IReadOnlyList<Hero> GetReserveHeroes()
            => playerGuild.GetReserveHeroes();
    }
}
