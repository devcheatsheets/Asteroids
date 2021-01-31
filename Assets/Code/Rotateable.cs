using UnityEngine;
using Asteroids.Utility;

namespace Asteroids
{
    /// <summary>
    /// Enables behaviour related to controllable rotation
    /// </summary>
    public class Rotateable : BasicCached
    {
        public float rotationSpeed = 5f;
        
        [Tooltip("The axis that is used as 'forward'")]
        public Axis lookAxis = Axis.Y;

        public TargetMode targetMode = TargetMode.CursorPosition;

        public Transform target;
        
        /// <summary>
        /// Rotates the object towards a given point in 2D space
        /// </summary>
        /// <param name="targetPosition">Position to rotate towards</param>
        private void RotateTowards(Vector2 targetPosition)
        {
            var lookRotation = AsteroidsInput.LookRotation(transform, targetPosition, lookAxis);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        #region Unity Callbacks

        public void Update()
        {
            switch (targetMode)
            {
                case TargetMode.CursorPosition:
                {
                    RotateTowards(AsteroidsInput.cursorPosition);
                    break;
                }
                case TargetMode.Custom:
                {
                    if(target)
                        RotateTowards((Vector2)target.position);
                    break;
                }
                case TargetMode.Player:
                {
                    var player = Player.Instance;
                    if(player && player.gameObject.activeInHierarchy && Player.Instance.playerAttackable)
                    {
                        RotateTowards(
                            (Vector2)Camera.main.WorldToViewportPoint(player.transform.position)
                        );
                    }
                    break;
                }
            }
        }

        #endregion
    }
}
