using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Caches and gives access to frequently used render-related properties
    /// </summary>
    public class RenderCached : MonoBehaviour
    {
        #region Properties
        public MeshRenderer meshRenderer
        {
            get
            {
                if (_meshRenderer)
                {
                    return _meshRenderer;
                }
                else
                {
                    _meshRenderer = GetComponent<MeshRenderer>();
                    if (_meshRenderer)
                    {
                        return _meshRenderer;
                    }
                    else
                    {
                        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
                        return _meshRenderer;
                    }
                }
            }
        }

        public MeshFilter meshFilter
        {
            get
            {
                if (_meshFilter)
                {
                    return _meshFilter;
                }
                else
                {
                    _meshFilter = GetComponent<MeshFilter>();
                    if (_meshFilter)
                    {
                        return _meshFilter;
                    }
                    else
                    {
                        _meshFilter = gameObject.AddComponent<MeshFilter>();
                        return _meshFilter;
                    }
                }
            }
        }

        public PolygonCollider2D polyCollider
        {
            get
            {
                if (_polyCollider)
                {
                    return _polyCollider;
                }
                else
                {
                    _polyCollider = GetComponent<PolygonCollider2D>();
                    if (_polyCollider)
                    {
                        return _polyCollider;
                    }
                    else
                    {
                        _polyCollider = gameObject.AddComponent<PolygonCollider2D>();
                        return _polyCollider;
                    }
                }
            }
        }
        #endregion

        #region Cache
        protected MeshRenderer _meshRenderer;
        protected MeshFilter _meshFilter;
        protected PolygonCollider2D _polyCollider;
        #endregion
    }
}