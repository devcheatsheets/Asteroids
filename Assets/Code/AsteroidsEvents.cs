using System;

namespace Asteroids
{
    /// <summary>
    /// Responsible for holding events that should be accessible globally
    /// </summary>
    public static class AsteroidsEvents
    {
        /// <summary>
        /// Used when all the main settings should be reset
        /// </summary>
        public static Action onResetAll;
    }
}