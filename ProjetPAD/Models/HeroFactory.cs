using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetPAD.Models
{
    public static class HeroFactory
    {
        private static Random rng = new Random();

        private static List<Func<Hero>> pool = new()
        {
            () => new Guerrier("Guerrier"),
            () => new Mage("Mage"),
            () => new Rodeur("Rodeur"),
            () => new Paladin("Paladin"),
            () => new Assassin("Assassin"),
            () => new Druide("Druide"),
        };

        public static List<Hero> GetRandomHeroes(int count)
        {
            return pool
                .OrderBy(x => rng.Next())
                .Take(count)
                .Select(create => create())
                .ToList();
        }
    }
}