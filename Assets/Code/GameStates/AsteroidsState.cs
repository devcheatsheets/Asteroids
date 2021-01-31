namespace Asteroids
{
    /// <summary>
    /// Base class for a game state
    /// </summary>
    public abstract class AsteroidsState
    {
        public GameState gameState;
        
        /// <summary>
        /// Called once every frame
        /// </summary>
        public abstract void StateUpdate();
        
        /// <summary>
        /// Executed when entering the state
        /// </summary>
        public abstract void InitState();
        
        /// <summary>
        /// Executed when exiting the state
        /// </summary>
        public abstract void ClearState();
    }
}