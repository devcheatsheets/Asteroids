using System.Collections;
using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Handles projectile-related behaviour. Can only be cast by a Shooter and inherits properties from it
    /// </summary>
    [RequireComponent(typeof(Displayable))]
    public class Projectile : BasicCached
    {
        public int damage
        {
            get
            {
                return _damage;
            }
        }

        public Shooter shooter
        {
            get
            {
                return _shooter;
            }
        }

        private Displayable displayable
        {
            get
            {
                if (!_displayable)
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
        
        private int _damage;
        private Shooter _shooter;
        private float _lifeTime = 1f;
        private float _speed = 0.1f;

        private Displayable _displayable;
        private Movable _movable;
        
        /// <summary>
        /// Change colour based on the shooter's settings
        /// </summary>
        private void ChangeColor()
        {
            displayable.Init();
            displayable.meshRenderer.material.color = _shooter.projectileColor;
        }

        /// <summary>
        /// Initialize the projectile's settings based on a shooter who cast the object
        /// </summary>
        /// <param name="shooter">Shooter that casted the projectile</param>
        public void InitShooter(Shooter shooter, Borders borders)
        {
            this._shooter = shooter;
            transform.position = shooter.shootPosition.position;
            transform.rotation = shooter.shootPosition.rotation;

            _damage = shooter.damage;

            var scatterAmount = shooter.scatterAmount;
            transform.Rotate(0, 0, Random.Range(-scatterAmount, scatterAmount), Space.Self);
            _speed = shooter.projectileSpeed;
            _lifeTime = shooter.projectileLifeTime;
            
            ChangeColor();

            movable.Init(borders);

            gameObject.SetActive(true);
        }

        /// <summary>
        /// Returns projectile back to the pool after a given number of seconds
        /// </summary>
        /// <returns></returns>
        private IEnumerator LifeEnum()
        {
            yield return new WaitForSeconds(_lifeTime);
            gameObject.SetActive(false);
        }

        #region Unity Callbacks

        private void OnEnable() 
        {
            StartCoroutine(LifeEnum());
        }
        
        private void Update() 
        {
            transform.Translate(Vector3.up * (_speed * Time.deltaTime), Space.Self);
        }

        #endregion
    }
}