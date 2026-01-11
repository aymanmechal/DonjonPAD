using System;
using System.Collections.Generic;
using ProjetPAD.Models;

namespace ProjetPAD.Game
{
    public class EventManager
    {
        private readonly List<GameEvent> events = new();
        private readonly Random rng = new Random();

        public EventManager()
        {
            // Une dizaine d'événements simples
            events.Add(new GameEvent("Trouvaille", "Vous trouvez une petite bourse d'or.", 20, 0, 0, 0));
            events.Add(new GameEvent("Pillage de rats", "Des rats mangent vos réserves.", 0, -5, 0, 0));
            events.Add(new GameEvent("Bagarre à la taverne", "Vos héros se chamaillent et se blessent légèrement.", 0, 0, -5, 5));
            events.Add(new GameEvent("Don du village", "Un village reconnaissant vous offre des vivres.", 0, 10, 0, 0));
            events.Add(new GameEvent("Ambiance détendue", "Soirée paisible, tout le monde se repose mieux.", 0, 0, 3, -5));
            events.Add(new GameEvent("Nourriture avariée", "Une partie des vivres est perdue.", 0, -8, 0, 0));
            events.Add(new GameEvent("Trouble nocturne", "Un bruit inquiétant empêche le repos.", 0, 0, 0, 7));
            events.Add(new GameEvent("Herboriste", "Un herboriste soigne quelques blessures.", 0, 0, 7, 0));
            events.Add(new GameEvent("Vol à la tire", "On vous dérobe quelques pièces.", -10, 0, 0, 0));
            events.Add(new GameEvent("Festin improvisé", "Un festin redonne des forces à tous.", -5, -5, 5, -10));
        }

        public GameEvent? TryRollEvent(int percentChance)
        {
            if (percentChance <= 0) return null;
            if (percentChance > 100) percentChance = 100;

            int roll = rng.Next(0, 100); // 0..99
            if (roll >= percentChance)
                return null;

            if (events.Count == 0) return null;

            int idx = rng.Next(0, events.Count);
            return events[idx];
        }
    }
}
