using UnityEngine;
using Life;

namespace Life
{
    public class BlockAlive : BlockBase
    {
        private GameObject _hitId = null;
        
        // block action particle affects
        [SerializeField] public ParticleSystem explosion;

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
                            // record object hit id
                            _hitId = hit.collider.gameObject;
                            
                            // begin block hover animation
                            state = BlockState.Hover;
                            AudioManager.audioManager.BlockHover();
                        }
                    }
                }
            }
            
            // block has reach just about its target position (in place due to floating-point round issues)
            if ((transform.position - hoverPos).magnitude < 0.1f)
            {
                state = BlockState.Descend;
                AudioManager.audioManager.BlockDescend();

                // activate camera shake
                var cameraShake = mainCamera.GetComponent<CameraShake>();
                cameraShake.Shake(0.2f, 0.1f);
                
                if (_hitId == gameObject)
                {
                    var pos = transform.position;
                    Surround((int)pos.x, (int)pos.y);
                    _hitId = null;
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
        
        /// <summary>
        /// This method replcaes surrounding 'dead' blocks with 'alive' blocks.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
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
                    if (nx < 1 || nx > GameManager.gameManager.width - 1 || ny < 1 || ny > GameManager.gameManager.height - 1)
                    {
                        // next iteration of loop
                        continue;
                    }
                    
                    // if neighboring tile is 'dead'
                    if (!GameManager.gameManager.Cells[nx, ny])
                    {
                        // set new instance of 'alive' block to descend state
                        GameManager.gameManager.Blocks[nx, ny].GetComponent<BlockDead>().state = BlockState.Hover;
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
            transform.position = Vector3.Lerp(transform.position, originalPos, 7f * Time.deltaTime);

            if ((transform.position - originalPos).magnitude < 0.001f)
            {
                state = BlockState.Idle;
            }
        }
    }
}
