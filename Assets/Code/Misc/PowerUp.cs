using System.Collections.Generic;
using UnityEngine;
using Asteroids.Scriptable;

namespace Asteroids.Misc
{
    /// <summary>
    /// Handles powerUp-specific behaviour
    /// </summary>
    [RequireComponent(typeof(Movable))]
    public class PowerUp : BasicCached, ISpawnable
    {
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
        
        public List<PowerUpEffect> effects;

        private PowerUpEffect _chosenEffect;

        public float pickRange = 1f;
        
        [Space]
        public float onSpawnBorderImmunityTime = 3f;

        private Player _player;
        private GamePreset _gamePreset;
        private LogManager _logManager;

        #region Private
        private float _curDistanceToPlayer = float.MaxValue;
        #endregion

        #region Cache
        private Movable _movable;
        #endregion

        public void Init(Player player, GamePreset gamePreset, LogManager logManager, Borders borders)
        {
            _player = player;
            _gamePreset = gamePreset;
            _logManager = logManager;
            movable.Init(borders);
        }

        public void OnSpawned()
        {
            if (effects.Count == 0)
            {
                if (_logManager.Log(LogLevel.ErrorsAndWarnings))
                {
                    Debug.LogWarning("No effects to choose from", gameObject);
                }
                return;
            }
            _chosenEffect = effects.Count == 1 ? effects[0] : effects[Random.Range(0, effects.Count)];
            
            movable.MakeImmuneToBorderControl(onSpawnBorderImmunityTime);
            movable.AddForceTowardsCenter(_gamePreset.spawnedCenterTargetOffset);
        }

        public void OnExplode()
        {
            
        }

        #region Unity Callbacks

        private void Update() 
        {
            _curDistanceToPlayer = Vector2.Distance(_player.transform.position, transform.position);
            if(_curDistanceToPlayer <= pickRange)
            {
                _player.ApplyPowerUp(_chosenEffect);
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}