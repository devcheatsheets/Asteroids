using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public class PoolsManager : MonoBehaviour
    {
        [SerializeField] private List<PoolManager> _poolManagers = new();

        private LogManager _logManager;
        private Dictionary<string, PoolManager> _poolManagersCache = new Dictionary<string, PoolManager>();

        public void Init(LogManager logManager)
        {
            _logManager = logManager;

            foreach (var poolManager in _poolManagers)
            {
                poolManager.Init();
                if (!_poolManagersCache.ContainsKey(poolManager.poolName))
                {
                    _poolManagersCache.Add(poolManager.poolName, poolManager);
                }
            }
        }

        /// <summary>
        /// Returns a pool manager from the dictionary by its name
        /// </summary>
        /// <param name="name">Name of the pool manager as stated in the poolManagers list</param>
        /// <returns></returns>
        public PoolManager GetPoolManager(string name)
        {
            if (!_poolManagersCache.ContainsKey(name) && _logManager.Log(LogLevel.ErrorsAndWarnings))
            {
                Debug.LogWarning("<GameManager> A pool manager with key '" + name + "' doesn't exist in the GameManager. Please, add one before requesting.");
            }
            return _poolManagersCache[name];
        }

        /// <summary>
        /// Disable all instances in all pools
        /// </summary>
        public void DisableAllPools()
        {
            foreach(PoolManager poolManager in _poolManagers)
            {
                poolManager.DisableAllInstances();
            }
        }
    }
}