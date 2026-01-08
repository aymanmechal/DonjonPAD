using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjetPAD.Game;
using ProjetPAD.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjetPAD.UI
{
    public partial class MainWindow : Window
    {
        private bool _isUpdatingUI = false;
        private GameManager gm;

        public MainWindow()
        {
            InitializeComponent();
            gm = GameManager.GetInstance();

            UpdateUI();
            UpdatePhaseUI(); // 🔥 important au lancement
        }

        // =====================================================
        // 🔁 MISE À JOUR DES HÉROS
        // =====================================================
        public void UpdateHeroesList()
        {
            var guild = gm.GetPlayerGuild();
            HeroesList.ItemsSource = new Avalonia.Collections.AvaloniaList<Hero>(guild.GetHeroes());
            GoldText.Text = $"Or : {guild.GetGold()}"; // actualise aussi l'or
        }

        private void OnHeroesClick(object? sender, RoutedEventArgs e)
        {
            var heroWindow = new HeroWindow(this); // passer MainWindow comme parent
            heroWindow.Show();
        }

        // =====================================================
        // 🔁 UI GLOBALE
        // =====================================================
        private void UpdateUI()
        {
            _isUpdatingUI = true;
            var guild = gm.GetPlayerGuild();

            GoldText.Text = $"Or : {guild.GetGold()}";
            FoodText.Text = $"Nourriture : {guild.GetFood()}";
            PhaseText.Text = $"Phase : {gm.GetCurrentPhase()}";

            HeroesList.ItemsSource = new Avalonia.Collections.AvaloniaList<Hero>(guild.GetHeroes());
            MissionsList.ItemsSource = new Avalonia.Collections.AvaloniaList<Mission>(guild.GetMissions());

            ChanceText.Text = "";
            ResultText.Text = "";

            _isUpdatingUI = false;
        }

        private void UpdatePhaseUI()
        {
            RecruitmentButton.IsVisible = gm.GetCurrentPhase() == 0;
        }

        // =====================================================
        // 🛡️ RECRUTEMENT
        // =====================================================
        private void OnRecruitmentClick(object? sender, RoutedEventArgs e)
        {
            var recruitmentWindow = new Recruitment();

            // 🔥 RAFRAÎCHIR L’UI QUAND ON FERME LA FENÊTRE
            recruitmentWindow.Closed += (_, __) =>
            {
                UpdateUI();
                UpdatePhaseUI();
            };

            recruitmentWindow.ShowDialog(this);
        }

        // =====================================================
        // 🎯 SÉLECTION
        // =====================================================
        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingUI) return;
            UpdateChancePreview();
        }

        private void UpdateChancePreview()
        {
            var missions = gm.GetPlayerGuild().GetMissions();

            if (MissionsList.SelectedIndex < 0 || MissionsList.SelectedIndex >= missions.Count)
            {
                ChanceText.Text = "";
                return;
            }

            if (HeroesList.SelectedItems == null || HeroesList.SelectedItems.Count == 0)
            {
                ChanceText.Text = "Sélectionne des héros pour voir les chances.";
                return;
            }

            Mission mission = missions[MissionsList.SelectedIndex];
            List<Hero> selectedHeroes = HeroesList.SelectedItems.Cast<Hero>()
                .Where(h => !h.IsOnMission())
                .ToList();

            if (selectedHeroes.Count == 0)
            {
                ChanceText.Text = "Tous les héros sélectionnés sont déjà en mission.";
                return;
            }

            double chance = mission.GetSuccessChanceForHeroes(selectedHeroes);
            ChanceText.Text = $"Chance de réussite : {chance:0}%";
        }

        // =====================================================
        // ⏭️ PHASES
        // =====================================================
        private void OnNextPhase(object? sender, RoutedEventArgs e)
        {
            gm.NextPhase();
            UpdateUI();
            UpdatePhaseUI();
        }

        // =====================================================
        // ⚔️ MISSIONS
        // =====================================================
        private void OnDoMission(object? sender, RoutedEventArgs e)
        {
            if (HeroesList.SelectedItems == null || HeroesList.SelectedItems.Count == 0 || MissionsList.SelectedIndex < 0)
            {
                ResultText.Text = "Sélectionne au moins un héros et une mission.";
                return;
            }

            var allHeroes = gm.GetPlayerGuild().GetHeroes();
            List<int> heroIndexes = new();

            foreach (Hero hero in HeroesList.SelectedItems.Cast<Hero>())
                if (!hero.IsOnMission())
                    heroIndexes.Add(allHeroes.IndexOf(hero));

            if (heroIndexes.Count == 0)
            {
                ResultText.Text = "Tous les héros sélectionnés sont déjà en mission.";
                return;
            }

            bool started = gm.StartMission(MissionsList.SelectedIndex, heroIndexes);

            ResultText.Text = started
                ? "Mission lancée ! Les héros sont partis en mission."
                : "Impossible de lancer la mission.";

            UpdateUI();
        }

        // =====================================================
        // 🧙 AUTRES FENÊTRES
        // =====================================================
        private void OnMerchant(object? sender, RoutedEventArgs e)
        {
            if (gm.GetCurrentPhase() == 1)
                new MerchantWindow().Show();
            else
                ResultText.Text = "Le marchand n'est disponible qu'en phase 1.";
        }
    }
}

