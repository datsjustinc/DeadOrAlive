using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Life;

namespace Life
{
    public class BlockDead : BlockBase
    {
        /// <summary>
        /// This method inherits parent class start method.
        /// </summary>
        protected override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// This function controls function of block press.
        /// </summary>
        protected override void Update()
        {
            // check if starting grid blocks are set up
            if (GameManager.gameManager.state == GameManager.GameState.Start)
            {
                // check if block is in idle state and mouse clicked
                if (Input.GetMouseButtonDown(0) && state == BlockState.Idle)
                {
                    RaycastHit hit;
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
                    if (Physics.Raycast(ray, out hit))
                    {
                        // checks if raycast target matches clicked object
                        if (hit.collider.gameObject == gameObject)
                        {
                            // begin block hover animation
                            state = BlockState.Hover;
                        }
                    }
                }
                
                // block has reach just about its target position (in place due to floating-point round issues)
                if ((transform.position - hoverPos).magnitude < 0.1f)
                {
                    if (GameManager.gameManager.ability1)
                    {
                        GameManager.gameManager.ability1 = false;
                        Replace();
                    }
                }
            }
            
            // check the state of the block every frame and execution methods
            switch (state)
            {
                case BlockState.Hover:
                    Hover();
                    break;
                case BlockState.Descend:
                    Descend();
                    break;
            }
        }

        protected virtual void Replace()
        {
            var pos = transform.position;
                    
            // turn tile on grid to 'alive' status
            GameManager.gameManager.Cells[(int) pos.x, (int) pos.y] = true;
                    
            // instantiate new 'alive' block in position as current block
            GameManager.gameManager.Blocks[(int)pos.x, (int)pos.y] = Instantiate(GameManager.gameManager.alive,
                new Vector3(pos.x, pos.y, hoverPos.z), Quaternion.Euler(-90, 0, 0),
                GameManager.gameManager.aliveParent);

            // delete current block object
            Destroy(gameObject);
        }

        /// <summary>
        /// This method inherits parent class hover method.
        /// </summary>
        protected override void Hover()
        {
            base.Hover();
        }

        /// <summary>
        /// This method inherits parent class descend method.
        /// </summary>
        protected override void Descend()
        {
            base.Descend();
        }
    }
}
