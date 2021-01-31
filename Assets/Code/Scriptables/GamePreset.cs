using UnityEngine;

namespace Asteroids.Scriptable
{
    /// <summary>
    /// Holds main game settings
    /// </summary>
    [CreateAssetMenu(fileName = "NewGamePreset", menuName = "Asteroids/GamePreset")]
    public class GamePreset : ScriptableObject
    {
        [Range(1, 5)] public int playerLives;
        public float motionSpeed;
        public float rotationSpeed;
        public float shootScatterAmount;
        public float shootFrequency;
        public float projectilesLifeTime;
        public float timeScale;

        [Header("Misc")] 
        [Tooltip("Some spawned objects are pushed towards center. " +
                 "This parameter defines the offset from center for the objects to be pushed towards")]
        public float spawnedCenterTargetOffset = 5f;
        public float playerResetTime = 3f;
        public float startSpawnFrequency = 3f;
    }
}