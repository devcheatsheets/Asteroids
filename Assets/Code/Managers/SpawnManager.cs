using System.Collections.Generic;
using UnityEngine;
using Asteroids.Utility;
using Asteroids.Misc;

namespace Asteroids
{
    /// <summary>
    /// Responsible for continuously spawning new objects into the scene
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        /// <summary>
        /// Encapsulates setting of a single spawned type of items
        /// </summary>
        [System.Serializable] public class SpawnItem
        {
            public string name; // Just for a neater display in the Instector
            public bool enabled = true;
            public PoolManager poolManager;
            [Range(0f, 1f)] public float spawnProbability;
        }
        
        // List of all items that can be spawned
        public List<SpawnItem> spawnItems = new List<SpawnItem>();
        
        [Tooltip("Time between spawning in seconds")]
        public float spawnFrequency = 2f;
        public float minFrequency = 0.5f;
        public float frequencyIncreaseDelta = 0.1f;
        public float frequencyIncreaseTime = 1f;

        private LogManager _logManager;
        private GameManager _gameManager;
        private Player _player;
        private PoolsManager _poolsManager;
        private HUDManager _hudManager;
        private Borders _borders;

        #region Cache
        private float _lastTimeSpawned = 0f;
        private float _lastTimeIncreasedFrequency = 0f;
        #endregion

        public void Init(LogManager logManager, GameManager gameManager, Player player, PoolsManager poolsManager, HUDManager hudManager, Borders borders)
        {
            _logManager = logManager;
            _gameManager = gameManager;
            _player = player;
            _poolsManager = poolsManager;
            _hudManager = hudManager;
            _borders = borders;

            _gameManager.onResetAll += Reset;
        }
        
        /// <summary>
        /// Reset cache values
        /// </summary>
        private void Reset() 
        {
            _lastTimeSpawned = Time.time;
            _lastTimeIncreasedFrequency = Time.time;
        }
        
        /// <summary>
        /// Choose a pool manager based on items probabilities
        /// </summary>
        /// <returns></returns>
        private PoolManager PickPoolManager()
        {
            PoolManager pickedPoolManager = null;
            float maxProbability = 0f;

            foreach (SpawnItem item in spawnItems)
            {
                if(!item.enabled) continue;
                float probability = Random.Range(0f, 1f) * item.spawnProbability;
                if(probability > maxProbability)
                {
                    maxProbability = probability;
                    pickedPoolManager = item.poolManager;
                }
            }

            if(pickedPoolManager == null && _logManager.Log(LogLevel.ErrorsAndWarnings))
            {
                Debug.LogWarning("<SpawnManager> Failed to pick a prefab to spawn");
            }

            return pickedPoolManager;
        }

        private void AssignReferences(Transform instance)
        {
            var powerUp = instance.GetComponent<PowerUp>();
            if(powerUp)
            {
                powerUp.Init(_player, _gameManager.gamePreset, _logManager, _borders);
            }

            var rock = instance.GetComponent<Rock>();
            if(rock)
            {
                rock.Init(_gameManager, _poolsManager, _hudManager, _borders);
            }

            var enemy = instance.GetComponent<Enemy>();
            if(enemy)
            {
                enemy.Init(_gameManager, _poolsManager, _hudManager, _borders, _player);
            }
        }

        #region Unity Callbacks

        private void Update() 
        {
            if(!_gameManager)
                return;
                
            if(Time.time >= _lastTimeSpawned + spawnFrequency)
            {
                _lastTimeSpawned = Time.time;

                var instance = PickPoolManager().RequestInstance<Transform>();
                var position = _borders.RandomPointOnBorder(-3f);
                instance.position = position;

                AssignReferences(instance);

                var customizable = instance.GetComponent<ISpawnable>();
                if(customizable != null)
                {
                    customizable.OnSpawned();
                }
            }

            if(Time.time >= _lastTimeIncreasedFrequency + frequencyIncreaseTime)
            {
                _lastTimeIncreasedFrequency = Time.time;
                spawnFrequency = Mathf.Max(minFrequency, spawnFrequency - frequencyIncreaseDelta);
            }
        }

        #endregion
    }
}