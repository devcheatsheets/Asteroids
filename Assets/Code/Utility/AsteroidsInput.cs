using UnityEngine;

namespace Asteroids.Utility
{
    /// <summary>
    /// Handles complex/lengthy input calculations
    /// </summary>
    public static class AsteroidsInput 
    {
        /// <summary>
        /// current viewport position of the mouse cursor
        /// </summary>
        public static Vector2 cursorPosition
        {
            get
            {
                return (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }
        }
        
        /// <summary>
        /// Generates a rotation to adjust for in order for the rotateable to look at the target point
        /// </summary>
        /// <param name="rotateable">Object that needs to be rotated</param>
        /// <param name="targetPosition">Position to rotate towards</param>
        /// <param name="lookAxis">Local 'forward' axis/direction</param>
        /// <returns></returns>
        public static Quaternion LookRotation(Transform rotateable, Vector2 targetPosition, Axis lookAxis)
        {
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(rotateable.position);
            
            float angle = Math.AngleBetweenTwoPoints(positionOnScreen, targetPosition);

            var _lookRotation = Quaternion.Euler(new Vector3(0f, 0f, Math.NormalizedAngle(angle, lookAxis)));

            return _lookRotation;
        }
    }
}
