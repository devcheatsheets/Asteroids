using UnityEngine;

namespace Asteroids
{
    public class LogManager : MonoBehaviour
    {
        [Tooltip("Types of Debug.Logs that shoulds be displayed")]
        [SerializeField] private LogLevel _logLevel;
        
        /// <summary>
        /// Returns true or false depending on the log level defined by the User and the log level of the current message
        /// </summary>
        /// <param name="level">Seriousness (log level) of a message</param>
        /// <returns></returns>
        public bool Log(LogLevel level)
        {
            switch(_logLevel)
            {
                case LogLevel.OnlyErrors:
                    return level == LogLevel.OnlyErrors;
                case LogLevel.ErrorsAndWarnings:
                    return level == LogLevel.OnlyErrors || level == LogLevel.ErrorsAndWarnings;
                case LogLevel.All:
                    return level == LogLevel.OnlyErrors || level == LogLevel.ErrorsAndWarnings || level == LogLevel.All;
            }

            return false;
        }
    }
}