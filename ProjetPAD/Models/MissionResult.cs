namespace ProjetPAD.Models {
    public class MissionResult {
        // Champs privés
        private bool success;
        private int damageTaken;
        private int foodGained;
        private int goldGained;

        // Constructeur
        public MissionResult(bool success, int damageTaken, int foodGained, int goldGained) {
            this.success = success;
            this.damageTaken = damageTaken;
            this.foodGained = foodGained;
            this.goldGained = goldGained;
        }

        // Getters / Setters
        public bool GetSuccess() {
            return success;
        }

        public void SetSuccess(bool value) {
            success = value;
        }

        public int GetDamageTaken() {
            return damageTaken;
        }

        public void SetDamageTaken(int value) {
            damageTaken = value;
        }

        public int GetFoodGained() {
            return foodGained;
        }

        public void SetFoodGained(int value) {
            foodGained = value;
        }

        public int GetGoldGained() {
            return goldGained;
        }

        public void SetGoldGained(int value) {
            goldGained = value;
        }
    }
}