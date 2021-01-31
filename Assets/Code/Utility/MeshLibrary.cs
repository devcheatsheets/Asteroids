using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Utility
{
    /// <summary>
    /// Handles mesh generation
    /// </summary>
    public static class MeshLibrary
    {
        /// <summary>
        /// Generates a quad-shaped mesh
        /// </summary>
        /// <param name="width">Width of the quad</param>
        /// <param name="height">Height of the quad</param>
        /// <returns></returns>
        public static Mesh Quad(float width, float height)
        {
            var w = width * 0.5f;
            var h = height * 0.5f;

            Vector3[] vertices = new Vector3[4]
            {
                new Vector3(-w, -h, 0),
                new Vector3(w, -h, 0),
                new Vector3(w, h, 0),
                new Vector3(-w, h, 0)
            };

            int[] triangles = new int[6]
            {
                // lower left triangle
                0, 3, 1,
                // upper right triangle
                2, 1, 3
            };

            Vector3[] normals = new Vector3[4]
            {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward
            };

            Vector2[] uv = new Vector2[4]
            {
                  new Vector2(0, 0),
                  new Vector2(1, 0),
                  new Vector2(0, 1),
                  new Vector2(1, 1)
            };

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;

            return mesh;
        }

        /// <summary>
        /// Generates an equilateral triangle
        /// </summary>
        /// <param name="radius">Distance to each point from the mesh center</param>
        /// <returns></returns>
        public static Mesh Triangle(float radius)
        {
            Vector3[] vertices = new Vector3[3]
            {
                new Vector3(0, radius, 0),
                new Vector3(2f/3f*radius, -2f/3f*radius, 0),
                new Vector3(-2f/3f*radius, -2f/3f*radius, 0)
            };

            int[] triangles = new int[3]
            {
                0, 1, 2
            };

            Vector3[] normals = new Vector3[3]
            {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward
            };

            Vector2[] uv = new Vector2[3]
            {
                  new Vector2(0, 0),
                  new Vector2(1, 0),
                  new Vector2(0, 1)
            };

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;
            mesh.triangles = triangles;

            return mesh;
        }

        /// <summary>
        /// Generates a circle
        /// </summary>
        /// <param name="radius">Distance from the center to each verticle</param>
        /// <param name="numV">Number of vertices in the circle</param>
        /// <returns></returns>
        public static Mesh Circle(float radius, int numV)
        {
            List<Vector3> verticesList = new List<Vector3> { };
            float x;
            float y;
            for (int i = 0; i < numV; i++)
            {
                x = radius * Mathf.Sin((2 * Mathf.PI * i) / numV);
                y = radius * Mathf.Cos((2 * Mathf.PI * i) / numV);
                verticesList.Add(new Vector3(x, y, 0f));
            }
            Vector3[] vertices = verticesList.ToArray();

            
            List<int> trianglesList = new List<int> { };
            for (int i = 0; i < (numV - 2); i++)
            {
                trianglesList.Add(0);
                trianglesList.Add(i + 1);
                trianglesList.Add(i + 2);
            }
            int[] triangles = trianglesList.ToArray();

            List<Vector3> normalsList = new List<Vector3> { };
            for (int i = 0; i < vertices.Length; i++)
            {
                normalsList.Add(-Vector3.forward);
            }
            Vector3[] normals = normalsList.ToArray();

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles;

            return mesh;
        }

        /// <summary>
        /// Offset each of the vertices by a random amount 
        /// </summary>
        /// <param name="mesh">Mesh to apply the effect to</param>
        /// <param name="effectRadius">Radius by which to offset the vertices</param>
        /// <returns></returns>
        public static Mesh MessedUp(Mesh mesh, float effectRadius)
        {
            List<Vector3> newVertices = new List<Vector3>();

            for(int i = 0; i < mesh.vertices.Length; i++)
            {
                var deltaPosition = Random.onUnitSphere * effectRadius;
                var newV = mesh.vertices[i] + deltaPosition;
                newV.z = 0;
                newVertices.Add(newV);

            }
            mesh.vertices = newVertices.ToArray();
            return mesh;
        }
    }
}