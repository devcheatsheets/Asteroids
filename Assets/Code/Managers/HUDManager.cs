using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asteroids.Utility;

namespace Asteroids
{
    /// <summary>
    /// Responsible for handling UI
    /// </summary>
    public class HUDManager : Singleton<HUDManager>
    {
        public GameObject preGameContainer;
        public GameObject gamePauseContainer;
        public GameObject gamePlayContainer;
        public GameObject gameOverContainer;
        [Space]
        public Text scoreText;
        [Tooltip("Images that indicate currently remaining player's lives")]
        public List<GameObject> lifeIcons = new List<GameObject>();

        public Text messageText;

        [Space]
        public Text gameOverScoreText;
        public Text highestScoreText;

        /// <summary>
        /// Update the score text with the current value of the score
        /// </summary>
        /// <param name="score">Score to display</param>
        public void DisplayScore(int score)
        {
            scoreText.text = score.ToString();
        }

        /// <summary>
        /// Update HUD with player's current lives 
        /// </summary>
        /// <param name="curLives">Player's remaining lives</param>
        public void DisplayLives(int curLives)
        {
            if(curLives < 0 || curLives > 5)
            {
                if(GameManager.Instance.Log(LogLevel.ErrorsAndWarnings))
                {
                    Debug.LogWarning("<HUDManager> Invalid 'curLives' passed. Has to be between 0 and 5", gameObject);
                }
                curLives = Mathf.Clamp(curLives, 0, 5);
            }

            for(int i = 0; i < 5; i++)
            {
                lifeIcons[i].SetActive(curLives > i);
            }
        }

        /// <summary>
        /// Display a temporary message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="duration"></param>
        public void DisplayMessage(string message, float duration = 2f)
        {
            StartCoroutine(DisplayMessageEnum(message, duration));
        }

        /// <summary>
        /// Handles displaying and hiding a temporary message
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="duration">How long is the message going to be visible for</param>
        /// <returns></returns>
        private IEnumerator DisplayMessageEnum(string message, float duration)
        {
            messageText.text = message;
            var c = messageText.color;
            messageText.color = new Color(c.r, c.g, c.b, 1f);
            yield return new WaitForSeconds(duration);
            messageText.color = new Color(c.r, c.g, c.b, 0f);
        }

        /// <summary>
        /// Turn on a proper container depending on the current state of the game
        /// </summary>
        /// <param name="gameState">Game state that the container is associated with</param>
        public void ToggleContainer(GameState gameState)
        {
            preGameContainer.SetActive(gameState == GameState.PreGame);
            gamePlayContainer.SetActive(gameState == GameState.Game);
            gamePauseContainer.SetActive(gameState == GameState.Pause);
            gameOverContainer.SetActive(gameState == GameState.GameOver);
        }

        /// <summary>
        /// Display current score and highest score in the game over container
        /// </summary>
        public void DisplayGameOverStats()
        {
            gameOverScoreText.text = GameManager.Instance.currentScore.ToString();
            highestScoreText.text = "Highest score: " + DataManager.GetHighestScore();
        }
    }
}