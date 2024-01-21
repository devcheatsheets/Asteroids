using System;
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
    public class Player : MonoBehaviour
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
        /// Indicates whether the player is fully reset and can collide with other objects
        /// </summary>
        /// <value></value>
        public bool playerAttackable 
        {
            get
            {
                return _playerAttackable;
            }
        }

        private PoolsManager _poolsManager;
        private HUDManager _hudManager;
        private Borders _borders;
        private LogManager _logManager;
        
        #region Private

        private Coroutine _powerUpCoroutine;
        private bool _playerAttackable = true;

        private GamePreset _gamePreset;

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

        public Action onLivesEqualsZero;

        public void Init(PoolsManager poolsManager, HUDManager hudManager, Borders borders, LogManager logManager, GameManager gameManager)
        {
            _poolsManager = poolsManager;
            _hudManager = hudManager;
            _borders = borders;
            _logManager = logManager;

            gameManager.onResetAll += ResetPlayerStats;
            gameManager.onResetAll += hittable.ResetLives;

            hittable.Init(_hudManager);
            movable.Init(_borders);
            shooter.Init(_poolsManager, _borders, _logManager);

            _gamePreset = gameManager.gamePreset;
        }
        
        /// <summary>
        /// Executed when hittable gets lives reduced. Added to the component's callback 
        /// </summary>
        private void OnLivesReduced()
        {
            var explosion = _poolsManager.GetPoolManager("PlayerExplosions").RequestInstance<Transform>();
            explosion.position = transform.position;
            _hudManager.DisplayLives(hittable.currentLives);
            ResetPlayer();
        }

        /// <summary>
        /// Executed when hittable lives are equal to zero. Added to the component's callback 
        /// </summary>
        private void Hittable_OnLivesEqualsZero()
        {
            onLivesEqualsZero?.Invoke();
        }

        #region PowerUps

        /// <summary>
        /// Temporarily alter the player's settings with a power up. Display the power up name
        /// </summary>
        /// <param name="powerUp">An effect to apply</param>
        public void ApplyPowerUp(PowerUpEffect powerUp)
        {
            _powerUpCoroutine = StartCoroutine(ApplyPowerUpEnum(powerUp));
            _hudManager.DisplayMessage(powerUp.name);
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

            movable.speed = Mathf.Max(movable.speed - powerUp.playerMotionSpeedBoost, _gamePreset.motionSpeed);
            rotateable.rotationSpeed  = Mathf.Max(rotateable.rotationSpeed - powerUp.playerRotationSpeedBoost, _gamePreset.rotationSpeed);
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
            StartCoroutine(ResetPlayerEnum(_gamePreset.playerResetTime));
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
            
            hittable.lives = _gamePreset.playerLives;
            movable.speed = _gamePreset.motionSpeed;
            rotateable.rotationSpeed = _gamePreset.rotationSpeed;
            shooter.scatterAmount = _gamePreset.shootScatterAmount;
            shooter.projectileLifeTime = _gamePreset.projectilesLifeTime;
            shooter.delay = _gamePreset.shootFrequency;
            Time.timeScale = _gamePreset.timeScale;
        }

        #region Unity Callbacks

        void OnEnable()
        {
            hittable.onLivesReduced += OnLivesReduced;
            hittable.onLivesEqualsZero += Hittable_OnLivesEqualsZero;
        }

        void OnDisable()
        {
            hittable.onLivesReduced -= OnLivesReduced;
            hittable.onLivesEqualsZero -= Hittable_OnLivesEqualsZero;
        }

        #endregion
    }
}