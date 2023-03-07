using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Life
{
    /// <summary>
    /// This class allows block rotation with mouse.
    /// </summary>
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed;
        [SerializeField] private Camera mainCamera;
        
        // range and midpoint of cube oscillation
        [SerializeField] private float speedMultiplier = 2.0f;
        [SerializeField] private float speedOffset = 1.0f;

        /// <summary>
        /// This function sets the rotation speed and main camera.
        /// </summary>
        private void Start()
        {
            // set field variable of the base class
            rotateSpeed = 20f;
            mainCamera = Camera.main;
        }

        /// <summary>
        /// This function checks mouse button clicks.
        /// </summary>
        private void Update()
        {
            // check if left mouse button is down
            if (Input.GetMouseButton(0))
            {
                RotateBlock();
            }
            else
            {
                // smooth oscillation over time
                var oscillation = Mathf.Sin(Time.time * speedMultiplier) + speedOffset;
            
                // rotate horizontally
                transform.Rotate(0, oscillation * rotateSpeed * Time.deltaTime, 0, Space.World);
            }
        }
        
        /// <summary>
        /// This function checks and rotates selected target block.
        /// </summary>
        private void RotateBlock()
        {
            // grabs mouse click location
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // checks if raycast target matches current object
                if (hit.collider.gameObject == gameObject)
                {
                    // grabs direction of mouse drag
                    var mouseX = Input.GetAxis("Mouse X");
                    var mouseY = Input.GetAxis("Mouse Y");

                    // updates cube rotation with mouse drag direction
                    transform.Rotate(Vector3.up, -mouseX * rotateSpeed, Space.World);
                    transform.Rotate(Vector3.right, mouseY * rotateSpeed, Space.World);
                }
            }
        }
    }
}
