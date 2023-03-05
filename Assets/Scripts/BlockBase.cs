using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Life
{
    public class BlockBase : MonoBehaviour
    {
        [SerializeField] protected Camera mainCamera;
        [SerializeField] protected float rotateSpeed;

        [SerializeField] protected Vector3 originalPos;
        [SerializeField] protected Vector3 hoverPos;

        protected virtual void Start()
        {
            mainCamera = Camera.main;
            originalPos = transform.position;
            hoverPos = new Vector3(originalPos.x, originalPos.y, -80f);

        }

        protected virtual void Update()
        {
            if (Input.GetMouseButton(0) && GameManager.gameManager.state == GameManager.GameState.Start)
            {
                Hover();
            }
        }

        protected virtual void Hover()
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                // checks if raycast target matches current object
                if (hit.collider.gameObject == gameObject)
                {
                    transform.position = Vector3.Lerp(transform.position, hoverPos, 0.05f * Time.deltaTime);
                }
            }
        }

        protected virtual void Rotate()
        {
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
                    transform.Rotate(Vector3.up, -mouseX * rotateSpeed);
                    transform.Rotate(Vector3.right, mouseY * rotateSpeed);
                }
            }
        }
    }
}

