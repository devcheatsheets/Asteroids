using UnityEngine;

namespace Asteroids.Misc
{
    /// <summary>
    /// Handles rock-specific behaviour
    /// </summary>
    [RequireComponent(typeof(Movable))]
    [RequireComponent(typeof(Hittable))]
    [RequireComponent(typeof(Displayable))]
    [RequireComponent(typeof(RockExplode))]
    public class Rock : BasicCached, ISpawnable
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
        
        public RockExplode rockExplode
        {
            get
            {
                if(_rockExplode)
                {
                    return _rockExplode;
                }
                _rockExplode = GetComponent<RockExplode>();
                return _rockExplode;
            }
        }

        #endregion
        
        #region Public

        public int scoreToAdd = 10;
        public float onSpawnBorderImmunityTime = 3f;

        public float minRockSize = 0.3f;
        public float minRockCanExplodeSize = 1.5f;

        #endregion
        
        #region Cache

        private Hittable _hittable;
        private Movable _movable;
        private Displayable _displayable;
        private RockExplode _rockExplode;

        #endregion
        
        public void OnExplode()
        {
            var proportion = rockExplode.proportionLeft;
            displayable.meshType = MeshType.Circle;
            displayable.radius = 3 * proportion;
            displayable.numVertices = 7;
            displayable.messUpRadius = proportion;

            if(displayable.radius <= minRockSize)
            {
                displayable.gameObject.SetActive(false);
            }
            else
            {
                displayable.Init();
                if(displayable.radius >= minRockCanExplodeSize)
                {
                    rockExplode.canExplode = true;
                }
                else
                {
                    hittable.ResetOnLivesEqualsZero();
                    hittable.onLivesEqualsZero += ()=>{
                        GameManager.Instance.AddScore(scoreToAdd);
                        gameObject.SetActive(false);
                    };
                }
            }
        }

        public void OnSpawned()
        {
            displayable.meshType = MeshType.Circle;
            displayable.radius = 3;
            displayable.numVertices = 10;
            displayable.messUpRadius = 1;
            displayable.Init();
            
            rockExplode.canExplode = true;
            rockExplode.proportionLeft = 1f;
            
            movable.MakeImmuneToBorderControl(onSpawnBorderImmunityTime);
            movable.AddForceTowardsCenter(GameManager.Instance.gamePreset.spawnedCenterTargetOffset);
            
            hittable.lives = 1;
            hittable.ResetOnLivesEqualsZero();
            hittable.onLivesEqualsZero += ()=>{
                GameManager.Instance.AddScore(scoreToAdd);
            };
        }

    }
}