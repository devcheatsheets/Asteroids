using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Handles persistent data
    /// </summary>
    public class DataManager
    {
        private static string _highestScoreKey = "highestScore";

        public static int highestScore
        {
            get
            {
                if(!PlayerPrefs.HasKey(_highestScoreKey))
                {
                    PlayerPrefs.SetInt(_highestScoreKey, 0);
                }
                return PlayerPrefs.GetInt(_highestScoreKey);
            }
            set
            {
                PlayerPrefs.SetInt(_highestScoreKey, value);
            }
        }

        /// <summary>
        /// Returns highest score. Updates the value if current score is higher than the previous highest score
        /// </summary>
        /// <returns></returns>
        public static int GetHighestScore()
        {
            var gameManager = GameManager.Instance;
            if(gameManager.currentScore > highestScore)
            {
                highestScore = gameManager.currentScore;
            }
            return highestScore;
        }
    }
}