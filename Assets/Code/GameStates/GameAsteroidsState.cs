using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Main state. Actual game.
    /// </summary>
    public class GameAsteroidsState : AsteroidsState
    {
        private HUDManager _hudManager;
        private Player _player;
        private SpawnManager _spawnManager;
        private GameManager _gameManager;

        public void Init(HUDManager hudManager, Player player, SpawnManager spawnManager, GameManager gameManager)
        {
            _hudManager = hudManager;
            _player = player;
            _spawnManager = spawnManager;
            _gameManager = gameManager;
        }

        public GameAsteroidsState()
        {
            gameState = GameState.Game;
        }

        public override void ClearState()
        {
            // Nothing to clear here so far
        }

        public override void InitState()
        {
            _hudManager.ToggleContainer(gameState);

            _player.gameObject.SetActive(true);
            _spawnManager.enabled = true;
        }

        public override void StateUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                _gameManager.SetGameState(GameState.Pause);
            }
        }
    }
}