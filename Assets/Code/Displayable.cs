using System.Collections.Generic;
using UnityEngine;

using Asteroids.Utility;

namespace Asteroids
{
    /// <summary>
    /// Responsible for mesh displaying-related behaviour
    /// </summary>
    public class Displayable : RenderCached
    {

        #region Public Variables
        
        public MeshType meshType = MeshType.Quad;
        public float width = 1f;
        public float height = 1f;
        public float radius = 1f;
        public int numVertices = 10;
        public bool messUp = false;
        public float messUpRadius = 0.2f;
        public bool generateCollider = false;
        [Tooltip("If empty, the standard material will be applied")]
        public Material material;
        [Space]
        public bool initOnStart = true;

        #endregion

        /// <summary>
        /// Initialize the mesh based on the component's settings
        /// </summary>
        public void InitMesh()
        {
            meshRenderer.sharedMaterial = material ? material : new Material(Shader.Find("Standard"));

            Mesh newMesh = new Mesh();
            switch (meshType)
            {
                case MeshType.Triangle:
                    {
                        newMesh = MeshLibrary.Triangle(radius);
                        break;
                    }
                case MeshType.Quad:
                    {
                        newMesh = MeshLibrary.Quad(width, height);
                        break;
                    }
                case MeshType.Circle:
                    {
                        newMesh = MeshLibrary.Circle(radius, numVertices);
                        break;
                    }
            }
            if (messUp)
            {
                newMesh = MeshLibrary.MessedUp(newMesh, messUpRadius);
            }
            meshFilter.mesh = newMesh;
        }

        /// <summary>
        /// Initialize a Polygon Collider based on the mesh
        /// </summary>
        private void InitCollider()
        {
            polyCollider.pathCount = 1;

            List<Vector2> pathList = new List<Vector2> { };
            var meshVertices = meshFilter.mesh.vertices;
            for (int i = 0; i < meshVertices.Length; i++)
            {
                pathList.Add(new Vector2(meshVertices[i].x, meshVertices[i].y));
            }
            Vector2[] path = pathList.ToArray();

            polyCollider.SetPath(0, path);
        }
        
        /// <summary>
        /// Initialize the mesh and the collider based on the componnet's settings
        /// </summary>
        public void Init()
        {
            InitMesh();
            if(generateCollider)
                InitCollider();
        }

        #region Unity Callbacks

        // Start is called before the first frame update
        void Start()
        {
            if(initOnStart)
                Init();            
        }

        #endregion
    }
}