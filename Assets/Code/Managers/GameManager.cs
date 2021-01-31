#pragma warning disable CS0649

using System.Collections.Generic;
using UnityEngine;
using Asteroids.Utility;
using Asteroids.Scriptable;

namespace Asteroids
{
    /// <summary>
    /// A global entry point to the general settings, vital objects and components, and general routines.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// Return current score. Readonly.
        /// </summary>
        /// <value></value>
        public int currentScore
        {
            get
            {
                return _currentScore;
            }
        }

        [Tooltip("Types of Debug.Logs that shoulds be displayed")]
        [SerializeField] private LogLevel _logLevel;
        
        [Tooltip("Use GamePresets to save global settings into a file and switch between them")]
        public GamePreset gamePreset;

        #region Private
        private AsteroidsState _currentGameState;
        private int _currentScore = 0;
        #endregion

        #region Cache
        private Dictionary<GameState, AsteroidsState> _statesLib;
        #endregion

        /// <summary>
        /// Initializes dictionaries to access frequently requested objects faster.
        /// </summary>
        private void InitCache()
        {
            // Initialize the dictionary to access game states faster
            _statesLib = new Dictionary<GameState, AsteroidsState>()
            {
                {GameState.PreGame, new PreGameAsteroidsState()},
                {GameState.Game, new GameAsteroidsState()},
                {GameState.Pause, new PauseAsteroidsState()},
                {GameState.GameOver, new GameOverAsteroidsState()}
            };
        }

        /// <summary>
        /// Returns true or false depending on the log level defined by the User and the log level of the current message
        /// </summary>
        /// <param name="level">Seriousness (log level) of a message</param>
        /// <returns></returns>
        public bool Log(LogLevel level)
        {
            switch(_logLevel)
            {
                case LogLevel.OnlyErrors:
                    return level == LogLevel.OnlyErrors;
                case LogLevel.ErrorsAndWarnings:
                    return level == LogLevel.OnlyErrors || level == LogLevel.ErrorsAndWarnings;
                case LogLevel.All:
                    return level == LogLevel.OnlyErrors || level == LogLevel.ErrorsAndWarnings || level == LogLevel.All;
            }

            return false;
        }

        /// <summary>
        /// Set current score to 0
        /// </summary>
        public void ResetScore()
        {
            _currentScore = 0;
            HUDManager.Instance.DisplayScore(currentScore);
        }
        
        /// <summary>
        /// Adds to the current score and updates HUD
        /// </summary>
        /// <param name="amount">Amount to add. Can be negative.</param>
        public void AddScore(int amount)
        {
            _currentScore += amount;
            HUDManager.Instance.DisplayScore(currentScore);
        }

        /// <summary>
        /// Sets the game state to the user-provided one
        /// </summary>
        /// <param name="newState">New state enum value</param>
        public void SetGameState(GameState newState)
        {
            // If the game state is already the same as provided by user, simply do nothing
            if(_currentGameState != null && _currentGameState.gameState == newState)
            {
                return;
            }
            
            if(_currentGameState != null)
            {
                _currentGameState.ClearState();
            }
            _currentGameState = _statesLib[newState];
            _currentGameState.InitState();
            
            if(Log(LogLevel.All))
                Debug.Log("<GameManager> Setting game state to " + _currentGameState.gameState);
            
        }

        #region Unity Callbacks
        private void Start() 
        {
            InitCache();
            SetGameState(GameState.PreGame);

            AsteroidsEvents.onResetAll += ResetScore;
        }

        private void Update() 
        {
            _currentGameState.StateUpdate();
        }
        #endregion 
    }
}