using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// First state. 'Main menu'.
    /// </summary>
    public class PreGameAsteroidsState : AsteroidsState
    {
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
            HUDManager.Instance.ToggleContainer(gameState);

            GameManager.Instance.ResetScore();

            SpawnManager.Instance.enabled = false;

            PoolManager.DisableAllPools();

            Player.Instance.gameObject.SetActive(false);
            Player.Instance.transform.position = Vector2.zero;
            Player.Instance.ResetPlayerStats();
            Player.Instance.hittable.ResetLives();

        }

        public override void StateUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.SetGameState(GameState.Game);
            }
        }
    }
}