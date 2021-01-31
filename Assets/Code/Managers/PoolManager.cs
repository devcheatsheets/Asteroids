#pragma warning disable CS0649

using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Responsible for providing and recycling projectiles from the pool
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        [Tooltip("Used for caching purposes")]
        public string poolName;
        
        [SerializeField] private GameObject _prefab;

        private List<GameObject> _pool = new List<GameObject>();

        [SerializeField] private int _initialPoolSize = 10;

        #region Cache

        private static Dictionary<string, PoolManager> _poolManagersCache = new Dictionary<string, PoolManager>();

        #endregion

        /// <summary>
        /// Returns a pool manager from the dictionary by its name
        /// </summary>
        /// <param name="name">Name of the pool manager as stated in the poolManagers list</param>
        /// <returns></returns>
        public static PoolManager GetPoolManager(string name)
        {
            if(!_poolManagersCache.ContainsKey(name) && GameManager.Instance.Log(LogLevel.ErrorsAndWarnings))
            {
                Debug.LogWarning("<GameManager> A pool manager with key '" + name + "' doesn't exist in the GameManager. Please, add one before requesting.");
            }
            return _poolManagersCache[name];
        }
        
        /// <summary>
        /// Disable all instances in all pools
        /// </summary>
        public static void DisableAllPools()
        {
            foreach(PoolManager pm in GameObject.FindObjectsOfType<PoolManager>())
            {
                pm.DisableAllInstances();
            }
        }

        /// <summary>
        /// Add pool manager to the dictionary for 
        /// </summary>
        private void RegisterPoolManager()
        {
            if (!_poolManagersCache.ContainsKey(poolName))
            {
                _poolManagersCache.Add(poolName, this);
            }
        }

        /// <summary>
        /// Create new instance and add it to the pool
        /// </summary>
        /// <returns></returns>
        private GameObject InstantiateNew()
        {
            var newInstance = Instantiate(_prefab);
            _pool.Add(newInstance);
            newInstance.transform.parent = this.transform;
            newInstance.gameObject.SetActive(false);
            
            return newInstance;
        }

        /// <summary>
        /// Initialize the pool list with a given number of instances
        /// </summary>
        /// <param name="numInstances">Number of projectiles to instantiate at start</param>
        /// <returns></returns>
        private List<GameObject> InitPool(int numInstances)
        {
            for(int i = 0; i < numInstances; i++)
            {
                InstantiateNew();
            }
            return _pool;
        }

        /// <summary>
        /// Activates and enables an available projectile from the pool
        /// </summary>
        /// <returns></returns>
        public T RequestInstance<T>() where T : Component
        {
            foreach(GameObject obj in _pool)
            {
                if(!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj.GetComponent<T>();
                }
            }

            var newObj = InstantiateNew();
            newObj.SetActive(true);
            return newObj.GetComponent<T>();
        }

        /// <summary>
        /// Disable all instances in the pool
        /// </summary>
        private void DisableAllInstances()
        {
            foreach(GameObject obj in _pool)
            {
                obj.SetActive(false);
            }
        }

        #region Unity Callbacks

        private void Start() 
        {
            _pool = InitPool(_initialPoolSize);
            RegisterPoolManager();
        }

        #endregion
    }
}