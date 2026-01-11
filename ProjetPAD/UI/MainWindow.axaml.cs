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
        private async void OnNextPhase(object? sender, RoutedEventArgs e)
        {
            gm.NextPhase();
            RefreshUI();
            await ShowEventPopupIfAnyAsync();
        }

        // =====================================================
        // ⚔️ MISSIONS
        // =====================================================
        private async void OnDoMission(object? sender, RoutedEventArgs e)
        {
            var guild = gm.GetPlayerGuild();
            int missionIndex = MissionsList.SelectedIndex >= 0 ? MissionsList.SelectedIndex : 0;

            var heroes = guild.GetHeroes();
            int heroIndex;
            if (HeroesList.SelectedIndex >= 0)
                heroIndex = HeroesList.SelectedIndex;
            else
                heroIndex = 0;

            // Sécurités de bornes
            if (heroes.Count == 0 || guild.GetMissions().Count == 0)
            {
                ResultText.Text = "Aucun héros ou mission disponible.";
                return;
            }
            if (heroIndex < 0 || heroIndex >= heroes.Count)
                heroIndex = 0;
            if (missionIndex < 0 || missionIndex >= guild.GetMissions().Count)
                missionIndex = 0;

            // Un seul héros et une seule mission
            bool started = gm.StartMission(missionIndex, new List<int> { heroIndex });

            ResultText.Text = started
                ? "Mission lancée !"
                : "Impossible de lancer la mission.";

            RefreshUI();
            await ShowEventPopupIfAnyAsync();
        }

        // Helper pour uniformiser la MAJ de l'UI demandée
        private void RefreshUI()
        {
            UpdateUI();
            UpdatePhaseUI();
        }

        // Popup d'événement simple en code-behind
        private async System.Threading.Tasks.Task ShowEventPopupIfAnyAsync()
        {
            var ev = gm.GetLastEvent();
            if (ev == null)
                return;

            var stack = new StackPanel { Spacing = 8, Margin = new Avalonia.Thickness(12) };
            stack.Children.Add(new TextBlock { Text = ev.Description, TextWrapping = Avalonia.Media.TextWrapping.Wrap });

            string deltaLine(string label, int value)
                => value == 0 ? null : $"{label}: {value:+#;-#;0}";

            var lines = new List<string?>
            {
                deltaLine("GoldDelta", ev.GoldDelta),
                deltaLine("FoodDelta", ev.FoodDelta),
                deltaLine("HealthDelta", ev.HealthDelta),
                deltaLine("FatigueDelta", ev.FatigueDelta)
            };

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    stack.Children.Add(new TextBlock { Text = line });
            }

            var okButton = new Button { Content = "OK", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, Margin = new Avalonia.Thickness(0, 10, 0, 0) };

            var popup = new Window
            {
                Title = ev.Name,
                Width = 360,
                Height = 220,
                CanResize = false,
                Content = new StackPanel { Children = { stack, okButton } }
            };

            okButton.Click += (_, __) => popup.Close();

            await popup.ShowDialog(this);
            gm.ClearLastEvent();
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

