using UnityEngine;

namespace Asteroids.Scriptable
{
    /// <summary>
    /// A power up effect settings
    /// </summary>
    [CreateAssetMenu(fileName = "NewPowerUpEffect", menuName = "Asteroids/PowerUpEffect")]
    public class PowerUpEffect : ScriptableObject
    {
        public float playerMotionSpeedBoost = 0f;
        public float playerRotationSpeedBoost = 0f;
        public float playerShootAccuracyBoost = 0f;
        public float projectilesLifeTimeBoost = 0f;
        public float shootFrequencyBoost = 0f;
        [Range(-0.8f, 0.8f)] public float timeScaleBoost = 0f;

        public float effectTime = 0f;
    }
}