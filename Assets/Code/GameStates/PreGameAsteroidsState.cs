using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// First state. 'Main menu'.
    /// </summary>
    public class PreGameAsteroidsState : AsteroidsState
    {
        private HUDManager _hudManager;
        private Player _player;
        private SpawnManager _spawnManager;
        private GameManager _gameManager;
        private PoolsManager _poolsManager;

        public void Init(HUDManager hudManager, Player player, SpawnManager spawnManager, GameManager gameManager, PoolsManager poolsManager)
        {
            _hudManager = hudManager;
            _player = player;
            _spawnManager = spawnManager;
            _gameManager = gameManager;
            _poolsManager = poolsManager;
        }

        public PreGameAsteroidsState()
        {
            gameState = GameState.PreGame;
        }
        
        public override void ClearState()
        {
            // Nothing to clear here
        }

        public override void InitState()
        {
            _hudManager.ToggleContainer(gameState);

            _gameManager.ResetScore();

            _spawnManager.enabled = false;

            _poolsManager.DisableAllPools();

            _player.gameObject.SetActive(false);
            _player.transform.position = Vector2.zero;
            _player.ResetPlayerStats();
            _player.hittable.ResetLives();

        }

        public override void StateUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
            {
                _gameManager.SetGameState(GameState.Game);
            }
        }
    }
}