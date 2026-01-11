using System;
using System.Collections.Generic;

namespace ProjetPAD.Models
{
    // Modèle d'événement de jeu minimaliste
    public class GameEvent
    {
        public string Name { get; }
        public string Description { get; }
        public int GoldDelta { get; }
        public int FoodDelta { get; }
        public int HealthDelta { get; }
        public int FatigueDelta { get; }

        public GameEvent(string name, string description, int goldDelta, int foodDelta, int healthDelta, int fatigueDelta)
        {
            Name = name;
            Description = description;
            GoldDelta = goldDelta;
            FoodDelta = foodDelta;
            HealthDelta = healthDelta;
            FatigueDelta = fatigueDelta;
        }

        // Applique les deltas à la guilde et aux héros
        // Choix simple: Health/Fatigue s'appliquent à tous les héros de l'équipe active
        public void Apply(Guild guild)
        {
            // Or et nourriture sur la guilde (clamp à 0 garanti par Guild)
            guild.AddGold(GoldDelta);
            guild.AddFood(FoodDelta);

            // Santé et fatigue à tous les héros actifs
            List<Hero> heroes = guild.GetHeroes();
            foreach (var hero in heroes)
            {
                if (HealthDelta < 0)
                    hero.TakeDamage(Math.Abs(HealthDelta));
                else if (HealthDelta > 0)
                    hero.Heal(HealthDelta);

                if (FatigueDelta < 0)
                    hero.ReduceFatigue(Math.Abs(FatigueDelta));
                else if (FatigueDelta > 0)
                    hero.AddFatigue(FatigueDelta);
            }
        }
    }
}
