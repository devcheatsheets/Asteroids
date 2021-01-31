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

        #region Private
        private float _curDistanceToPlayer = float.MaxValue;
        #endregion

        #region Cache
        private Movable _movable;
        #endregion

        public void OnSpawned()
        {
            if (effects.Count == 0)
            {
                if (GameManager.Instance.Log(LogLevel.ErrorsAndWarnings))
                {
                    Debug.LogWarning("No effects to choose from", gameObject);
                }
                return;
            }
            _chosenEffect = effects.Count == 1 ? effects[0] : effects[Random.Range(0, effects.Count)];
            
            movable.MakeImmuneToBorderControl(onSpawnBorderImmunityTime);
            movable.AddForceTowardsCenter(GameManager.Instance.gamePreset.spawnedCenterTargetOffset);
        }

        public void OnExplode()
        {
            throw new System.NotImplementedException();
        }

        #region Unity Callbacks

        private void Update() 
        {
            _curDistanceToPlayer = Vector2.Distance(Player.Instance.transform.position, transform.position);
            if(_curDistanceToPlayer <= pickRange)
            {
                Player.Instance.ApplyPowerUp(_chosenEffect);
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}