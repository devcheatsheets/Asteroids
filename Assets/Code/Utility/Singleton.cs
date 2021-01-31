using UnityEngine;

namespace Asteroids.Utility
{

    /// <summary>
    /// Base class for all singletons (types that are supposed to have a single instance at all times)
    /// </summary>
    /// <typeparam name="T">Singleton type</typeparam>
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }
        

        private static T _instance;

        protected virtual void Awake() 
        {
            if(_instance != null)
            {
                Debug.LogError("<Singleton> Trying to instantiate a second instance of a singleton class");
            }
            else
            {
                _instance = (T) this;
            }
        }

        protected virtual void OnDestroy() 
        {
            if(_instance == this)
            {
                _instance = null;
            }
        }
    }
}