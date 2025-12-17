using System.Collections.Generic;

namespace ProjetPAD.Models;

public class Guild
{
    // Champs privés
    private string name;
    private List<Hero> heroes;
    private int gold;
    private int food;
    private List<Mission> missions;

    // Constructeur
    public Guild(string name)
    {
        this.name = name;
        this.heroes = new List<Hero>();
        this.gold = 100;   // valeur initiale
        this.food = 50;    // valeur initiale
        this.missions = new List<Mission>();
    }

    // Getters / Setters
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

    public void SetGold(int value)
    {
        gold = value;
    }

    public int GetFood()
    {
        return food;
    }

    public void SetFood(int value)
    {
        food = value;
    }

    public List<Mission> GetMissions()
    {
        return missions;
    }

    // Méthodes de gestion
    public void AddHero(Hero hero)
    {
        heroes.Add(hero);
    }

    public void RemoveHero(Hero hero)
    {
        heroes.Remove(hero);
    }

    public void AddMission(Mission mission)
    {
        missions.Add(mission);
    }

    public void RemoveMission(Mission mission)
    {
        missions.Remove(mission);
    }

    public void PaySalaries()
    {
        foreach (Hero hero in heroes)
        {
            gold = gold - hero.GetSalary();
        }
    }

    public void FeedHeroes()
    {
        food = food - heroes.Count;
    }

    public void ResolveMission(Mission mission, Hero hero)
    {
        MissionResult result = mission.Resolve(hero);

        if (result.Success)
        {
            food = food + result.FoodGained;
            gold = gold + result.GoldGained;
        }

        // Le héros a déjà pris des dégâts et de la fatigue dans Resolve()
    }

    // Vérification des conditions de défaite
    public bool IsDefeated()
    {
        // Tous les héros morts
        bool allDead = true;
        foreach (Hero hero in heroes)
        {
            if (hero.GetHealth() > 0)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            return true;
        }

        // Pas assez de nourriture
        if (food < 0)
        {
            return true;
        }

        // Plus de ressources critiques
        if (gold < 0)
        {
            return true;
        }

        return false;
    }
}