#pragma warning disable CS0649

using System.Collections.Generic;
using UnityEngine;
using Asteroids.Utility;
using Asteroids.Scriptable;
using System;

namespace Asteroids
{
    /// <summary>
    /// A global entry point to the general settings, vital objects and components, and general routines.
    /// </summary>
    public class GameManager : MonoBehaviour
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

        [Tooltip("Use GamePresets to save global settings into a file and switch between them")]
        public GamePreset gamePreset;

        [SerializeField] private HUDManager _hudManager;
        [SerializeField] private SpawnManager _spawnManager;
        [SerializeField] private Player _player;
        [SerializeField] private LogManager _logManager;
        [SerializeField] private PoolsManager _poolsManager;
        [SerializeField] private Borders _borders;

        #region Private
        private AsteroidsState _currentGameState;
        private int _currentScore = 0;
        #endregion

        #region Cache
        private Dictionary<GameState, AsteroidsState> _statesLib;
        #endregion

        public Action onResetAll;

        private void Init()
        {
            DataManager.Init(this);
            _hudManager.Init(_logManager);
            _poolsManager.Init(_logManager);
            _spawnManager.Init(_logManager, this, _player, _poolsManager, _hudManager, _borders);
            _borders.Init(_logManager);
            _player.Init(_poolsManager, _hudManager, _borders, _logManager, this);
        }

        /// <summary>
        /// Initializes dictionaries to access frequently requested objects faster.
        /// </summary>
        private void InitCache()
        {
            var preGameState = new PreGameAsteroidsState();
            preGameState.Init(_hudManager, _player, _spawnManager, this, _poolsManager);

            var gameState = new GameAsteroidsState();
            gameState.Init(_hudManager, _player, _spawnManager, this);

            var pauseState = new PauseAsteroidsState();
            pauseState.Init(_hudManager, _spawnManager, this);

            var gameOverState = new GameOverAsteroidsState();
            gameOverState.Init(_hudManager, _player, _spawnManager, this, _poolsManager);

            // Initialize the dictionary to access game states faster
            _statesLib = new Dictionary<GameState, AsteroidsState>()
            {
                {GameState.PreGame, preGameState},
                {GameState.Game, gameState},
                {GameState.Pause, pauseState},
                {GameState.GameOver, gameOverState}
            };
        }

        /// <summary>
        /// Set current score to 0
        /// </summary>
        public void ResetScore()
        {
            _currentScore = 0;
            _hudManager.DisplayScore(currentScore);
        }

        /// <summary>
        /// Adds to the current score and updates HUD
        /// </summary>
        /// <param name="amount">Amount to add. Can be negative.</param>
        public void AddScore(int amount)
        {
            _currentScore += amount;
            _hudManager.DisplayScore(currentScore);
        }

        /// <summary>
        /// Sets the game state to the user-provided one
        /// </summary>
        /// <param name="newState">New state enum value</param>
        public void SetGameState(GameState newState)
        {
            // If the game state is already the same as provided by user, simply do nothing
            if (_currentGameState != null && _currentGameState.gameState == newState)
            {
                return;
            }

            if (_currentGameState != null)
            {
                _currentGameState.ClearState();
            }
            _currentGameState = _statesLib[newState];
            _currentGameState.InitState();

            Debug.Log("<GameManager> Setting game state to " + _currentGameState.gameState);

        }

        #region Unity Callbacks
        private void Start()
        {
            Init();
            InitCache();
            SetGameState(GameState.PreGame);

            onResetAll += ResetScore;
        }

        void OnEnable()
        {
            _player.onLivesEqualsZero += Player_OnLivesEqualsZero;
        }

        void OnDisable()
        {
            _player.onLivesEqualsZero -= Player_OnLivesEqualsZero;
        }

        private void Update()
        {
            _currentGameState.StateUpdate();
        }

        #endregion 

        private void Player_OnLivesEqualsZero()
        {
            SetGameState(GameState.GameOver);
        }

        public void ResetAll()
        {
            onResetAll?.Invoke();
        }
    }
}