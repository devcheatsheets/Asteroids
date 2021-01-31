using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Player lives equal to 0
    /// </summary>
    public class GameOverAsteroidsState : AsteroidsState
    {
        public GameOverAsteroidsState()
        {
            gameState = GameState.GameOver;
        }
        
        public override void ClearState()
        {
            GameManager.Instance.StopAllCoroutines();

            Player.Instance.gameObject.SetActive(false);
            Player.Instance.transform.position = Vector2.zero;
            Player.Instance.displayable.meshRenderer.enabled = true;
            Player.Instance.collider.enabled = true;

            SpawnManager.Instance.enabled = false;
            SpawnManager.Instance.spawnFrequency = GameManager.Instance.gamePreset.startSpawnFrequency;

            PoolManager.DisableAllPools();
            
            if(AsteroidsEvents.onResetAll != null)
                AsteroidsEvents.onResetAll();
        }

        public override void InitState()
        {
            HUDManager.Instance.ToggleContainer(gameState);
            HUDManager.Instance.DisplayGameOverStats();

            Player.Instance.gameObject.SetActive(false);
            SpawnManager.Instance.enabled = false;
        }

        public override void StateUpdate()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.SetGameState(GameState.PreGame);
            }
            if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.SetGameState(GameState.Game);
            }
        }
    }
}