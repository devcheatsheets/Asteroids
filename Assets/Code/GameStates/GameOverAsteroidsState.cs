using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Player lives equal to 0
    /// </summary>
    public class GameOverAsteroidsState : AsteroidsState
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

        public GameOverAsteroidsState()
        {
            gameState = GameState.GameOver;
        }
        
        public override void ClearState()
        {
            _gameManager.StopAllCoroutines();

            _player.gameObject.SetActive(false);
            _player.transform.position = Vector2.zero;
            _player.displayable.meshRenderer.enabled = true;
            _player.collider.enabled = true;

            _spawnManager.enabled = false;
            _spawnManager.spawnFrequency = _gameManager.gamePreset.startSpawnFrequency;

            _poolsManager.DisableAllPools();
            
            _gameManager.ResetAll();
        }

        public override void InitState()
        {
            _hudManager.ToggleContainer(gameState);
            _hudManager.DisplayGameOverStats();

            _player.gameObject.SetActive(false);
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