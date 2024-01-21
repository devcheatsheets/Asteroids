using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace Asteroids
{
    /// <summary>
    /// Handles collisions-related behaviour
    /// </summary>
    public class Hittable : BasicCached
    {
        /// <summary>
        /// Encapsulates a single type of collision and the related settings
        /// </summary>
        [System.Serializable] public class CollisionEvent
        {
            public LayerMask layerMask;
            public UnityEvent onCollision;
        } 
        
        public int currentLives 
        {
            get
            {
                return _currentLives;
            }
        }
        
        // Max lives
        public int lives = 1;

        public event System.Action onLivesEqualsZero;
        public event System.Action onLivesReduced;

        public List<CollisionEvent> collisionEvents = new List<CollisionEvent>();

        #region Private

        private int _currentLives;
        private HUDManager _hudManager;

        #endregion

        public void Init(HUDManager hudManager)
        {
            _hudManager = hudManager;
        }
        
        /// <summary>
        /// Remove all the listeners from the onLivesEqualsZero callback
        /// </summary>
        public void ResetOnLivesEqualsZero() 
        {
            onLivesEqualsZero = null;
        }

        /// <summary>
        /// Set current lives to max lives and update HUD
        /// </summary>
        public void ResetLives()
        {
            _currentLives = lives;
            _hudManager.DisplayLives(_currentLives);
        }
        
        /// <summary>
        /// Reduce current lives by a given amount
        /// </summary>
        /// <param name="amount">Value to subtract from current lives</param>
        private void ReduceLives(int amount)
        {
            _currentLives = Mathf.Max(0, _currentLives - amount);
            onLivesReduced?.Invoke();

            if(_currentLives <= 0)
            {
                onLivesEqualsZero?.Invoke();
            }
        }

        #region Unity Callbacks

        private void Awake() 
        {
            _currentLives = lives;
        }
        
        /// <summary>
        /// Sent when an incoming collider makes contact with this object's
        /// collider (2D physics only).
        /// </summary>
        /// <param name="other">The Collision2D data associated with this collision.</param>
        private void OnCollisionEnter2D(Collision2D other)
        {
            // Execute custom collision events
            foreach(var collisionEvent in collisionEvents)
            {
                var layermask = collisionEvent.layerMask;
                var layer = other.gameObject.layer;
                if(layermask == (layermask | (1 << layer)))
                {
                    collisionEvent.onCollision.Invoke();
                }
            }
            // Interaction with a projectile
            var projectile = other.gameObject.GetComponent<Projectile>();
            if(projectile)
            {
                // Do not interact if the projectile was cast by the same game object
                var shooter = GetComponent<Shooter>();
                if(shooter && projectile.shooter == shooter)
                {
                    return;
                }
                ReduceLives(projectile.damage);
                projectile.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}