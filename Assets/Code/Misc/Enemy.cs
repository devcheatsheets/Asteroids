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
                if (_hittable)
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
                if (_movable)
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
                if (_rotateable)
                {
                    return _rotateable;
                }
                _rotateable = GetComponent<Rotateable>();
                return _rotateable;
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
        private Rotateable _rotateable;

        #endregion

        private GameManager _gameManager;
        private PoolsManager _poolsManager;

        public void Init(GameManager gameManager, PoolsManager poolsManager, HUDManager hudManager, Borders borders, Player player)
        {
            _gameManager = gameManager;
            _poolsManager = poolsManager;
            hittable.Init(hudManager);
            movable.Init(borders);
            rotateable.Init(player);
        }

        public void OnExplode()
        {

        }

        public void OnSpawned()
        {
            movable.MakeImmuneToBorderControl(onSpawnBorderImmunityTime);
            hittable.ResetOnLivesEqualsZero();
            hittable.onLivesEqualsZero += () =>
            {
                _gameManager.AddScore(scoreToAdd);
                var explosion = _poolsManager.GetPoolManager("EnemyExplosions").RequestInstance<Transform>();
                explosion.position = transform.position;
                gameObject.SetActive(false);
            };
        }
    }
}