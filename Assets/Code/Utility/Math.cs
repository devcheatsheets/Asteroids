using UnityEngine;

namespace Asteroids.Utility
{
    /// <summary>
    /// Handles complex and frequent calculations
    /// </summary>
    public static class Math
    {
        /// <summary>
        /// Calculates an angle between two vectors
        /// </summary>
        public static float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Adjust the calculated angle to fit the game space
        /// </summary>
        /// <param name="angle">Calculated angle</param>
        /// <param name="axis">Chosen 'forward' axis</param>
        /// <returns></returns>
        public static float NormalizedAngle(float angle, Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    {
                        return angle + 180f;
                    }
                case Axis.Y:
                    {
                        return angle + 90f;
                    }
            }
            return angle;
        }
    }
}