using UnityEngine;
using Asteroids.Utility;

namespace Asteroids
{
    /// <summary>
    /// Responsible for calculating the game borders based on the camera's viewport
    /// </summary>
    public class Borders : Singleton<Borders>
    {
        [Tooltip("Actual border can be further than the camera's viewport")]
        public float margin = 1f;

        #region Private

        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;

        #endregion
        
        /// <summary>
        /// Calculate the borders based on the camera's current viewport
        /// </summary>
        private void CalculateBorders()
        {
            var camera = Camera.main;
            if(!camera)
            {
                if(GameManager.Instance.Log(LogLevel.ErrorsAndWarnings))
                {
                    Debug.LogWarning("<Borders> Add camera to the scene");
                }
            }
            var leftBottom = camera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
            var rightTop = camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

            _minX = leftBottom.x - margin;
            _minY = leftBottom.y - margin;
            _maxX = rightTop.x + margin;
            _maxY = rightTop.y + margin;
        }

        /// <summary>
        /// Returns true if a given position is within the borders
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <param name="borderOffset">Additional margin</param>
        /// <returns></returns>
        public bool WithinBorders(Vector3 position, float borderOffset = 0f)
        {
            var x = position.x;
            var y = position.y;

            var xMatches = x > _minX-borderOffset && x < _maxX+borderOffset;
            var yMatches = y > _minY-borderOffset && y < _maxY+borderOffset;

            return xMatches && yMatches;
        }

        /// <summary>
        /// Return the opposite position on the border based on the currently provided position
        /// </summary>
        /// <param name="position">Position to invert</param>
        /// <param name="borderOffset">Additional margin</param>
        /// <returns></returns>
        public Vector3 OppositePosition(Vector3 position, float borderOffset = 0f)
        {
            var x = position.x;
            var y = position.y;

            if(x <= _minX-borderOffset)
            {
                return new Vector3(_maxX+borderOffset, position.y, 0);
            }
            else if(x >= _maxX+borderOffset)
            {
                return new Vector3(_minX-borderOffset, position.y, 0);
            }
            else if(y <= _minY-borderOffset)
            {
                return new Vector3(position.x, _maxY+borderOffset, 0);
            }
            else
            {
                return new Vector3(position.x, _minY-borderOffset, 0);
            }
        }

        /// <summary>
        /// Return a random point on the border
        /// </summary>
        /// <param name="offset">Additional margin</param>
        /// <returns></returns>
        public Vector3 RandomPointOnBorder(float offset)
        {
            int side = Random.Range(0, 4);
            float x = 0f, y = 0f;
            if(side == 0)
            {
                x = _minX + offset;
                y = Random.Range(_minY, _maxY);
            }
            else if(side == 1)
            {
                x = Random.Range(_minX, _maxX);
                y = _maxY - offset;
            }
            else if(side == 2)
            {
                x = _maxX - offset;
                y = Random.Range(_minY, _maxY);
            }
            else
            {
                x = Random.Range(_minX, _maxX);
                y = _minY + offset;
            }
            return new Vector3(x, y, 0f);
        }

        #region Unity Callbacks

        // Start is called before the first frame update
        void Start()
        {
            CalculateBorders();
        }

        #endregion
    }
}