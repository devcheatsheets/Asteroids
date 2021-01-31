using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Pause in the middle of the game
    /// </summary>
    public class PauseAsteroidsState : AsteroidsState
    {
        private float _timeScale;
        
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
            HUDManager.Instance.ToggleContainer(GameState.Pause);

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