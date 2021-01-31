#pragma warning disable CS0649

using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Handles behaviour that is related to shooting projectiles
    /// </summary>
    public class Shooter : BasicCached
    {
        /// <summary>
        /// Position for the projectile to be spawned at. Returns object's transform if no custom one provided
        /// </summary>
        public Transform shootPosition
        {
            get
            {
                if(_shootPosition)
                {
                    return _shootPosition;
                }
                return transform;
            }
        }

        [SerializeField] private Transform _shootPosition;
        public int damage = 1;
        public float delay = 0.3f;

        [Space]
        public float projectileLifeTime = 1f;
        public float projectileSpeed = 0.1f;

        public float scatterAmount = 5f;
        public Color projectileColor = Color.white;

        [Space]
        public ShootMode shootMode;

        #region Private
        private float _lastShootTime = 0f;
        #endregion

        /// <summary>
        /// Request a projectile from the pool, set it up and launch
        /// </summary>
        public void LaunchProjectile()
        {
            try
            {
                var projectile = PoolManager.GetPoolManager("Projectiles").RequestInstance<Projectile>();
                projectile.InitShooter(this);
            }
            catch (System.Exception)
            {
                if (GameManager.Instance.Log(LogLevel.OnlyErrors) && !PoolManager.GetPoolManager("Projectiles"))
                {
                    Debug.LogError("<GameManager> Projectiles Pool Manager is not initialized");
                }
            }
        }

        /// <summary>
        /// Handles shooting with delay over time
        /// </summary>
        public void Shoot()
        {
            if(Time.time >= _lastShootTime + delay) 
            {
                _lastShootTime = Time.time;
                LaunchProjectile();
            }
        }

        #region Unity Callbacks

        private void Update() 
        {
            switch(shootMode)
            {
                case ShootMode.InputBased:
                {
                    if(Input.GetMouseButton(0))
                    {
                        Shoot();
                    }
                    break;
                }
                case ShootMode.Automatic:
                {
                    Shoot();
                    break;
                }
            }
        }
        
        #endregion
    }
}