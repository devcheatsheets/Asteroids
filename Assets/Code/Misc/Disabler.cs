using System.Collections;
using UnityEngine;

namespace Asteroids.Misc
{
    /// <summary>
    /// Disables a gameObject after a given amount of time
    /// </summary>
    public class Disabler : MonoBehaviour
    {

        public float disableInSeconds = 1f;

        private IEnumerator DisableEnum()
        {
            yield return new WaitForSeconds(disableInSeconds);
            gameObject.SetActive(false);
        }

        private void OnEnable() 
        {
            StartCoroutine(DisableEnum());
        }

    }
}
