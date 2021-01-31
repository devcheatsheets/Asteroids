using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Caches and gives access to basic frequently used properties
    /// </summary>
    public class BasicCached : MonoBehaviour
    {
        public new GameObject gameObject
        {
            get
            {
                if(_gameObject)
                {
                    return _gameObject;
                }
                _gameObject = this.transform.gameObject;
                return _gameObject;
            }
        }

        public new Transform transform
        {
            get
            {
                if(_transform)
                {
                    return _transform;
                }
                _transform = GetComponent<Transform>();
                return _transform;
            }
        }

        #region Cache
        private GameObject _gameObject;
        private Transform _transform;
        #endregion
    }
}