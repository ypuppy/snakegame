namespace SnakeGame
{
    enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    struct DifficultySettings
    {
        public int EnemySpawnThreshold;     // points per enemy
        public int SpecialFoodProbability;  // % chance for special food

        public DifficultySettings(int enemySpawnThreshold, int specialFoodProbability)
        {
            EnemySpawnThreshold = enemySpawnThreshold;
            SpecialFoodProbability = specialFoodProbability;
        }
    }
}
