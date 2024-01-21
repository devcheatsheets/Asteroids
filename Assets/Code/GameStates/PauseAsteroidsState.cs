using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Pause in the middle of the game
    /// </summary>
    public class PauseAsteroidsState : AsteroidsState
    {
        private HUDManager _hudManager;
        private SpawnManager _spawnManager;
        private GameManager _gameManager;

        private float _timeScale;

        public void Init(HUDManager hudManager, SpawnManager spawnManager, GameManager gameManager)
        {
            _hudManager = hudManager;
            _spawnManager = spawnManager;
            _gameManager = gameManager;
        }
        
        public PauseAsteroidsState()
        {
            gameState = GameState.Pause;
        }

        public override void ClearState()
        {
            Time.timeScale = _timeScale;
        }

        public override void InitState()
        {
            _timeScale = Time.timeScale;
            Time.timeScale = 0f;
            _hudManager.ToggleContainer(GameState.Pause);

            _spawnManager.enabled = false;
        }

        public override void StateUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                _gameManager.SetGameState(GameState.PreGame);
            }
            if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
            {
                _gameManager.SetGameState(GameState.Game);
            }
        }
    }
}