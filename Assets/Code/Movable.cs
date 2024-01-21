using System.Collections;
using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Handles motion-related behaviour
    /// </summary>
    public class Movable : BasicCached
    {
        private new Rigidbody2D rigidbody
        {
            get
            {
                if(!_rigidbody)
                {
                    _rigidbody = GetComponent<Rigidbody2D>();
                }
                return _rigidbody;
            }
        }
        
        private Vector3 _moveVector
        {
            get
            {
                return lookAxis == Axis.X ? Vector3.right : Vector3.up;
            }
        }

        [Tooltip("The axis that is used as 'forward'")]
        public Axis lookAxis = Axis.Y;
        
        public float speed = 10f;

        public MotionMode motionMode;
        public BordersBehaviour bordersBehaviour;
        public float bordersOffset = 0f;

        public ParticleSystem thrustParticles;

        #region Private

        private bool _immuneToBorderControl = false;
        private Vector2 _moveDirection;
        private Borders _borders;

        #endregion
        

        #region Cache
        
        private Rigidbody2D _rigidbody;

        #endregion

        public void Init(Borders borders)
        {
            _borders = borders;
        }

        /// <summary>
        /// Use rigidbody add force to push the object towards viewport center
        /// </summary>
        /// <param name="offsetRadius"></param>
        public void AddForceTowardsCenter(float offsetRadius)
        {
            var targetPosition = Vector2.zero + Random.insideUnitCircle.normalized * offsetRadius;
            _moveDirection = targetPosition - (Vector2)transform.position;
            rigidbody.AddForce(_moveDirection * speed);
        }

        /// <summary>
        /// Disable at-borders behaviour for a given number of seconds
        /// </summary>
        /// <param name="seconds">Amount of time to disable the behaviour for</param>
        public void MakeImmuneToBorderControl(float seconds)
        {
            if(_immuneToBorderControl) return;
            StartCoroutine(MakeImmuneToBorderControl_Enum(seconds));
        }
        
        /// <summary>
        /// An enumerator that handles enabling and disabling of at-borders behaviour
        /// </summary>
        /// <param name="seconds">Amount of time to disable the behaviour for</param>
        /// <returns></returns>
        private IEnumerator MakeImmuneToBorderControl_Enum(float seconds)
        {
            _immuneToBorderControl = true;
            yield return new WaitForSeconds(seconds);
            _immuneToBorderControl = false;
        }

        /// <summary>
        /// Handle at-borders behaviour based on the component's settings
        /// </summary>
        private void BordersControl()
        {
            if(_immuneToBorderControl) return;
            if(_borders.WithinBorders(transform.position, bordersOffset)) return;
            switch(bordersBehaviour)
            {
                case BordersBehaviour.Disable:
                {
                    gameObject.SetActive(false);
                    break;
                }
                case BordersBehaviour.MoveToOppositePoint:
                {
                    transform.position = _borders.OppositePosition(transform.position, bordersOffset);
                    break;
                }
            }
        }
        
        /// <summary>
        /// Move the object forward
        /// </summary>
        private void MoveForward()
        {
            transform.Translate(_moveVector * (speed * Time.deltaTime), Space.Self);
        }

        /// <summary>
        /// Update particles to indicate motion
        /// </summary>
        /// <param name="maxParticles"></param>
        private void UpdateThrust(int maxParticles)
        {
            if (!thrustParticles) return;
            var main = thrustParticles.main;
            main.maxParticles = maxParticles;
        }

        /// <summary>
        /// Update motion and thrust particles based on User input
        /// </summary>
        private void UpdateInput()
        {
            if(Input.GetKey(KeyCode.W))
            {
                MoveForward();
                UpdateThrust(30);
            }
            else
            {
                UpdateThrust(0);
            }
        }

        #region Unity Callbacks

        // Update is called once per frame
        void Update()
        {
            if(!_borders)
                return;
                
            switch (motionMode)
            {
                case MotionMode.Automatic:
                {
                    MoveForward();
                    break;
                }
                case MotionMode.InputBased:
                {
                    UpdateInput();
                    break;
                }
            }
            BordersControl();
        }

        #endregion
    }
}