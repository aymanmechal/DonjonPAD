namespace ProjetPAD.Models
{
    public class Potion
    {
        public enum PotionType
        {
            Petite,
            Moyenne,
            Grande
        }

        private PotionType type;
        private int healAmount;
        private int price;

        public Potion(PotionType type)
        {
            this.type = type;

            switch (type)
            {
                case PotionType.Petite:
                    healAmount = 15;
                    price = 10;
                    break;
                case PotionType.Moyenne:
                    healAmount = 30;
                    price = 20;
                    break;
                case PotionType.Grande:
                    healAmount = 60;
                    price = 40;
                    break;
            }
        }

        public PotionType GetPotionType() => type;
        public int GetHealAmount() => healAmount;
        public int GetPrice() => price;
        public string GetName() => $"{type} potion (+{healAmount} PV)";
    }
}