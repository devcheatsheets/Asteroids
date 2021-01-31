using UnityEngine;

namespace Asteroids.Misc
{
    /// <summary>
    /// Handles enemy-specific behaviour
    /// </summary>
    [RequireComponent(typeof(Movable))]
    [RequireComponent(typeof(Hittable))]
    public class Enemy : BasicCached, ISpawnable
    {
        #region Essential Properties

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

        #endregion

        #region Public

        public int scoreToAdd = 10;
        public float onSpawnBorderImmunityTime = 3f;

        #endregion
        
        #region Cache

        private Hittable _hittable;
        private Movable _movable;

        #endregion

        public void OnExplode()
        {
            throw new System.NotImplementedException();
        }

        public void OnSpawned()
        {
            movable.MakeImmuneToBorderControl(onSpawnBorderImmunityTime);
            hittable.ResetOnLivesEqualsZero();
            hittable.onLivesEqualsZero += ()=>{
                GameManager.Instance.AddScore(scoreToAdd);
                var explosion = PoolManager.GetPoolManager("EnemyExplosions").RequestInstance<Transform>();
                explosion.position = transform.position;
                gameObject.SetActive(false);
            };
        }
    }
}