using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Life
{
    /// <summary>
    /// This class manages the camera shake effect.
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
    
        // camera position properties
        private Vector3 _originalPos;

        /// <summary>
        /// This method grabs starting position of camera.
        /// </summary>
        private void Start()
        {
            // grabs original position of camera
            _originalPos = transform.localPosition;
        }

        /// <summary>
        /// This method starts camera shake with set values.
        /// </summary>
        /// <param name="intensity">how sensitive shake is</param>
        /// <param name="duration">how long shake lasts</param>
        public void Shake(float intensity, float duration)
        {
            StartCoroutine(ShakeCoroutine(intensity, duration));
        }

        /// <summary>
        /// This method runs camera shake over time.
        /// </summary>
        /// <param name="intensity">how sensitive shake is</param>
        /// <param name="duration">how long shake lasts</param>
        /// <returns>number of frames to wait</returns>
        private IEnumerator ShakeCoroutine(float intensity, float duration)
        {
            // keeps track of shake time
            float timeElapsed = 0f;

            // while shake time is less than set amount
            while (timeElapsed < duration)
            {
                // get new shake location based on intensity factor
                Vector3 randomPoint = _originalPos + Random.insideUnitSphere * intensity;

                // update camera to new shake location over time
                transform.localPosition = Vector3.Lerp(transform.localPosition, randomPoint, Time.deltaTime * 10f);

                // increment shake time
                timeElapsed += Time.deltaTime;

                yield return null;
            }
            
            // return camera to starting position
            transform.localPosition = _originalPos;
        }
    }
}
