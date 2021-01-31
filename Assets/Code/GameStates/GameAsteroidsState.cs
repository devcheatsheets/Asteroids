using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Main state. Actual game.
    /// </summary>
    public class GameAsteroidsState : AsteroidsState
    {
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
            HUDManager.Instance.ToggleContainer(gameState);

            Player.Instance.gameObject.SetActive(true);
            SpawnManager.Instance.enabled = true;
        }

        public override void StateUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.SetGameState(GameState.Pause);
            }
        }
    }
}