using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjetPAD.Game;
using ProjetPAD.Models;
using System.Linq;

namespace ProjetPAD.UI
{
    
    public partial class HeroWindow : Window
    {
        private GameManager gm;

        public HeroWindow()
        {
            InitializeComponent();
            gm = GameManager.GetInstance();
            UpdateUI();
        }
        private MainWindow _mainWindow;

        public HeroWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            gm = GameManager.GetInstance();
            _mainWindow = mainWindow;
            UpdateUI();
        }


        private void UpdateUI()
        {
            var guild = gm.GetPlayerGuild();

            ActiveHeroesList.ItemsSource =
                guild.GetHeroes().Select(h =>
                    $"{h.GetName()} | PV {h.GetHealth()} | Fatigue {h.GetFatigue()}").ToList();

            ReserveHeroesList.ItemsSource =
                guild.GetReserveHeroes().Select(h =>
                    $"{h.GetName()} | PV {h.GetHealth()}").ToList();

            PotionList.ItemsSource =
                guild.GetPotions().Select(p => p.GetName()).ToList();
        }

        // ===================
        // HÉROS
        // ===================

        private void OnActivateHero(object? sender, RoutedEventArgs e)
        {
            int index = ReserveHeroesList.SelectedIndex;
            if (index < 0) return;

            var guild = gm.GetPlayerGuild();
            var hero = guild.GetReserveHeroes()[index];

            if (hero.IsOnMission())
            {
                InfoText.Text = "Héros en mission.";
                return;
            }

            bool success = guild.AddHeroToActive(hero);
            InfoText.Text = success
                ? "Héros activé."
                : "Équipe déjà complète (4).";

            UpdateUI();
        }

        private void OnMoveToReserve(object? sender, RoutedEventArgs e)
        {
            int index = ActiveHeroesList.SelectedIndex;
            if (index < 0) return;

            var guild = gm.GetPlayerGuild();
            var hero = guild.GetHeroes()[index];

            if (hero.IsOnMission())
            {
                InfoText.Text = "Impossible : héros en mission.";
                return;
            }

            guild.MoveHeroToReserve(hero);
            InfoText.Text = "Héros déplacé en réserve.";
            UpdateUI();
        }

        // ===================
        // POTIONS
        // ===================

        private void OnUsePotion(object? sender, RoutedEventArgs e)
        {
            int heroIndex = ActiveHeroesList.SelectedIndex;
            int potionIndex = PotionList.SelectedIndex;

            if (heroIndex < 0 || potionIndex < 0) return;

            var guild = gm.GetPlayerGuild();
            guild.UsePotion(
                guild.GetPotions()[potionIndex],
                guild.GetHeroes()[heroIndex]);

            InfoText.Text = "Potion utilisée !";
            UpdateUI();
        }
    }
}
