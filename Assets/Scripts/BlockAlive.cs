using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Life;

namespace Life
{
    public class BlockAlive : BlockBase
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
                    state = BlockState.Descend;

                    int x = (int)transform.position.x;
                    int y = (int)transform.position.y;

                    if (GameManager.gameManager.ability2)
                    {
                        GameManager.gameManager.ability2 = false;
                        Surround(x, y);
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
        
        protected virtual void Surround(int x, int y)
        {
            // goes through rows prior to and following current block[x, y]
            for (int i = -1; i <= 1; i++)
            {
                // goes through columns prior to and following current block[x, y]
                for (int j = -1; j <= 1; j++)
                {
                    // skip current block location
                    if (i == 0 && j == 0)
                    {
                        // next iteration of loop
                        continue;
                    }
                    
                    var nx = x + i;
                    var ny = y + j;
                    
                    // position index out-of-bounds on grid
                    if (nx < 0 || nx > GameManager.gameManager.width || ny < 0 || ny > GameManager.gameManager.height)
                    {
                        // next iteration of loop
                        continue;
                    }
                    
                    // if neighboring tile is 'dead'
                    if (!GameManager.gameManager.Cells[nx, ny])
                    {
                        // turn tile on grid to 'alive' status
                        GameManager.gameManager.Cells[nx, ny] = true;
                        Destroy(GameManager.gameManager.Blocks[nx, ny]);
                        
                        // instantiate new 'alive' block in position as current neighboring block position
                        GameManager.gameManager.Blocks[nx, ny] = Instantiate(GameManager.gameManager.alive,
                            new Vector3(nx, ny, originalPos.z), Quaternion.Euler(-90, 0, 0),
                            GameManager.gameManager.aliveParent);
                    }
                }
            }
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
