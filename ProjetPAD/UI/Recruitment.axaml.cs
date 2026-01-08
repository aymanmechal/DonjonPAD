using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjetPAD.Game;
using ProjetPAD.Models;
using System.Collections.Generic;

namespace ProjetPAD.UI
{
    public partial class Recruitment : Window
    {
        private GameManager gm;
        private List<Hero> availableHeroes;

        public Recruitment()
        {
            InitializeComponent();

            gm = GameManager.GetInstance();
            availableHeroes = HeroFactory.GetRandomHeroes(3);

            HeroesList.ItemsSource = new Avalonia.Collections.AvaloniaList<Hero>(availableHeroes);
        }

        private void OnRecruit(object? sender, RoutedEventArgs e)
        {
            if (HeroesList.SelectedItem is not Hero hero)
                return;

            var guild = gm.GetPlayerGuild();

            // Vérifie que le héros n'est pas déjà dans la réserve ou actif
            if (guild.GetHeroes().Contains(hero) || guild.GetReserveHeroes().Contains(hero))
                return;

            // Vérifie si le joueur a assez d'or
            if (guild.GetGold() < hero.GetRecruitCost())
                return;

            // Ajoute le héros à la réserve et retire l'or
            guild.AddHeroToReserve(hero);
            guild.SpendGold(hero.GetRecruitCost());

            // Supprime le héros de la liste des disponibles
            availableHeroes.Remove(hero);

            // Rafraîchit la ListBox
            HeroesList.ItemsSource = new Avalonia.Collections.AvaloniaList<Hero>(availableHeroes);

            // Fermer la fenêtre si plus de héros disponibles
            if (availableHeroes.Count == 0)
                Close();
        }
    }
}