using UnityEngine;

namespace Asteroids.Misc
{
    /// <summary>
    /// Holds the logic for fracturing the rock into smaller rocks
    /// </summary>
    public class RockExplode : MonoBehaviour
    {
        public float explosionPower = 10f;
        public bool canExplode = true;
        public float spawnRadius = 3f;
        public int numPieces = 3;

        [HideInInspector] public float proportionLeft = 1f;

        private GameManager _gameManager;
        private PoolsManager _poolsManager;
        private HUDManager _hudManager;
        private Borders _borders;

        public void Init(GameManager gameManager, PoolsManager poolsManager, HUDManager hudManager, Borders borders)
        {
            _gameManager = gameManager;
            _poolsManager = poolsManager;
            _hudManager = hudManager;
            _borders = borders;
        }

        /// <summary>
        /// Pool an explosion and move it to the object
        /// </summary>
        private void SpawnExplosion()
        {
            var explosion = _poolsManager.GetPoolManager("RockExplosions").RequestInstance<Transform>();
            explosion.position = transform.position;
        }

        /// <summary>
        /// Spawn a new rock piece with a given proportion
        /// </summary>
        /// <param name="proportion">Size of the new piece</param>
        private void SpawnNewRockPiece(float proportion)
        {
            var newInstance = _poolsManager.GetPoolManager("Rocks").RequestInstance<Transform>();
            newInstance.position = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;

            var rock = newInstance.GetComponent<Rock>();
            if(rock)
            {
                rock.Init(_gameManager, _poolsManager, _hudManager, _borders);
            }

            var rockExplode = newInstance.GetComponent<RockExplode>();
            if (rockExplode)
            {
                rockExplode.Init(_gameManager, _poolsManager, _hudManager, _borders);
                rockExplode.canExplode = false;
                rockExplode.proportionLeft = proportion;
            }

            var customizable = newInstance.GetComponent<ISpawnable>();
            if(customizable != null)
            {
                customizable.OnExplode();
            }
            
            newInstance.GetComponent<Rigidbody2D>().AddForce((newInstance.transform.position - transform.position) * explosionPower);
        }
        
        /// <summary>
        /// Calculate proportions and spawn new rock pieces
        /// </summary>
        private void Fracture()
        {
            for(int i = 0; i < numPieces; i++)
            {
                float proportion;
                if(i == numPieces - 1)
                {
                    proportion = proportionLeft;
                }
                else
                {
                    proportion = Random.Range(0f, proportionLeft);
                    proportionLeft -= proportion;
                }

                SpawnNewRockPiece(proportion);
            }
        }

        /// <summary>
        /// Called via a Unity Event
        /// </summary>
        public void Explode()
        {
            SpawnExplosion();
            
            if (canExplode)
            {
                Fracture();
            }
            
            gameObject.SetActive(false);
        }
    }
}