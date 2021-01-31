using System.Collections;
using Asteroids.Scriptable;
using Asteroids.Utility;
using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// handles all the player-specific functionality
    /// </summary>
    [RequireComponent(typeof(Hittable))]
    [RequireComponent(typeof(Displayable))]
    [RequireComponent(typeof(Movable))]
    [RequireComponent(typeof(Rotateable))]
    [RequireComponent(typeof(Shooter))]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class Player : Singleton<Player>
    {
        #region EssentialComponents
        public Hittable hittable
        {
            get
            {
                if(_hittable)
                {
                    return _hittable;
                }
                _hittable = GetComponent<Hittable>();
                return _hittable;
            }
        }
        
        public Displayable displayable
        {
            get
            {
                if(_displayable)
                {
                    return _displayable;
                }
                _displayable = GetComponent<Displayable>();
                return _displayable;
            }
        }
        
        public Movable movable
        {
            get
            {
                if(_movable)
                {
                    return _movable;
                }
                _movable = GetComponent<Movable>();
                return _movable;
            }
        }
        
        public Rotateable rotateable
        {
            get
            {
                if(_rotateable)
                {
                    return _rotateable;
                }
                _rotateable = GetComponent<Rotateable>();
                return _rotateable;
            }
        }
        
        public Shooter shooter
        {
            get
            {
                if(_shooter)
                {
                    return _shooter;
                }
                _shooter = GetComponent<Shooter>();
                return _shooter;
            }
        }
        
        public new PolygonCollider2D collider
        {
            get
            {
                if(_collider)
                {
                    return _collider;
                }
                _collider = GetComponent<PolygonCollider2D>();
                return _collider;
            }
        }
        
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
        #endregion

        /// <summary>
        /// Indicates whether the player is fully reset and can collidewith other objects
        /// </summary>
        /// <value></value>
        public bool playerAttackable 
        {
            get
            {
                return _playerAttackable;
            }
        }
        
        #region Private

        private Coroutine _powerUpCoroutine;
        private bool _playerAttackable = true;

        #endregion
        
        #region Cache
        private Hittable _hittable;
        private Displayable _displayable;
        private Movable _movable;
        private Rotateable _rotateable;
        private Shooter _shooter;
        private PolygonCollider2D _collider;
        private GameObject _gameObject;
        private Transform _transform;
        #endregion
        
        /// <summary>
        /// Executed when hittable gets lives reduced. Added to the component's callback 
        /// </summary>
        private void OnLivesReduced()
        {
            var explosion = PoolManager.GetPoolManager("PlayerExplosions").RequestInstance<Transform>();
            explosion.position = transform.position;
            HUDManager.Instance.DisplayLives(hittable.currentLives);
            ResetPlayer();
        }

        /// <summary>
        /// Executed when hittable lives are equal to zero. Added to the component's callback 
        /// </summary>
        private void OnLivesEqualsZero()
        {
            GameManager.Instance.SetGameState(GameState.GameOver);;
        }

        #region PowerUps

        /// <summary>
        /// Temporarily alter the player's settings with a power up. Display the power up name
        /// </summary>
        /// <param name="powerUp">An effect to apply</param>
        public void ApplyPowerUp(PowerUpEffect powerUp)
        {
            _powerUpCoroutine = StartCoroutine(ApplyPowerUpEnum(powerUp));
            HUDManager.Instance.DisplayMessage(powerUp.name);
        }

        /// <summary>
        /// Handle changes to settings over time
        /// </summary>
        /// <param name="powerUp">An effect to apply</param>
        /// <returns></returns>
        private IEnumerator ApplyPowerUpEnum(PowerUpEffect powerUp)
        {

            movable.speed += powerUp.playerMotionSpeedBoost;
            rotateable.rotationSpeed += powerUp.playerRotationSpeedBoost;

            var startScatterAmount = shooter.scatterAmount;
            shooter.scatterAmount = Mathf.Clamp(shooter.scatterAmount - powerUp.playerShootAccuracyBoost, 0f, 360f);
            var scatterAmountDelta = startScatterAmount - shooter.scatterAmount;

            shooter.scatterAmount = Mathf.Max(shooter.scatterAmount - powerUp.playerShootAccuracyBoost, 0);
            shooter.projectileLifeTime += powerUp.projectilesLifeTimeBoost;
            shooter.delay -= powerUp.shootFrequencyBoost;
            
            var startTimeScale = Time.timeScale;
            Time.timeScale = Mathf.Clamp(Time.timeScale - powerUp.timeScaleBoost, 0.3f, 2f);
            var timeScaleDelta = startTimeScale - Time.timeScale;
            
            yield return new WaitForSeconds(powerUp.effectTime);

            movable.speed = Mathf.Max(movable.speed - powerUp.playerMotionSpeedBoost, GameManager.Instance.gamePreset.motionSpeed);
            rotateable.rotationSpeed  = Mathf.Max(rotateable.rotationSpeed - powerUp.playerRotationSpeedBoost, GameManager.Instance.gamePreset.rotationSpeed);
            shooter.scatterAmount += scatterAmountDelta;
            shooter.projectileLifeTime -= powerUp.projectilesLifeTimeBoost;
            shooter.delay += powerUp.shootFrequencyBoost;
            Time.timeScale += timeScaleDelta;
        }

        #endregion
        
        /// <summary>
        /// Reset player's settings and position and temporarily disable the collider
        /// </summary>
        private void ResetPlayer()
        {
            ResetPlayerStats();
            _playerAttackable = false;
            transform.position = Vector3.zero;
            StartCoroutine(ResetPlayerEnum(GameManager.Instance.gamePreset.playerResetTime));
        }

        /// <summary>
        /// Handles animating the reset and turning the collider on/off over time
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="frequency"></param>
        /// <returns></returns>
        private IEnumerator ResetPlayerEnum(float seconds, float frequency = 0.1f)
        {
            float startTime = Time.time;
            
            collider.enabled = false;
            while(Time.time < startTime + seconds)
            {
                displayable.meshRenderer.enabled = !displayable.meshRenderer.enabled;
                yield return new WaitForSeconds(frequency);
            }
            displayable.meshRenderer.enabled = true;
            collider.enabled = true;
            _playerAttackable = true;
        }

        /// <summary>
        /// Reset the player settings according to the game preset selected in the GameManager
        /// </summary>
        public void ResetPlayerStats()
        {
            _playerAttackable = true;

            if(_powerUpCoroutine != null)
            {
                StopCoroutine(_powerUpCoroutine);
            }

            var gamePreset = GameManager.Instance.gamePreset;
            
            hittable.lives = gamePreset.playerLives;
            movable.speed = gamePreset.motionSpeed;
            rotateable.rotationSpeed = gamePreset.rotationSpeed;
            shooter.scatterAmount = gamePreset.shootScatterAmount;
            shooter.projectileLifeTime = gamePreset.projectilesLifeTime;
            shooter.delay = gamePreset.shootFrequency;
            Time.timeScale = gamePreset.timeScale;
        }

        #region Unity Callbacks

        void OnEnable()
        {
            hittable.onLivesReduced += OnLivesReduced;
            hittable.onLivesEqualsZero += OnLivesEqualsZero;
        }

        void OnDisable()
        {
            hittable.onLivesReduced -= OnLivesReduced;
            hittable.onLivesEqualsZero -= OnLivesEqualsZero;
        }

        private void Start()
        {
            AsteroidsEvents.onResetAll += ResetPlayerStats;
            AsteroidsEvents.onResetAll += hittable.ResetLives;
        }

        #endregion
    }
}