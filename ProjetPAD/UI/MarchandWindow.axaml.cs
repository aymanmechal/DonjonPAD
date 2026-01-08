using Avalonia.Controls;
using Avalonia.Interactivity;
using ProjetPAD.Game;
using ProjetPAD.Models;
using System.Collections.Generic;

namespace ProjetPAD.UI
{
    public partial class MerchantWindow : Window
    {
        private GameManager gm;
        private List<Potion> shopPotions;

        public MerchantWindow()
        {
            InitializeComponent();
            gm = GameManager.GetInstance();

            shopPotions = new()
            {
                new Potion(Potion.PotionType.Petite),
                new Potion(Potion.PotionType.Moyenne),
                new Potion(Potion.PotionType.Grande)
            };

            PotionList.ItemsSource = shopPotions.ConvertAll(p =>
                $"{p.GetName()} - {p.GetPrice()} or");
        }

        private void OnBuy(object? sender, RoutedEventArgs e)
        {
            int index = PotionList.SelectedIndex;
            if (index < 0) return;

            Potion potion = shopPotions[index];
            bool success = gm.GetPlayerGuild().BuyPotion(potion);

            InfoText.Text = success ? "Potion achetée !" : "Pas assez d'or.";
        }
    }
}