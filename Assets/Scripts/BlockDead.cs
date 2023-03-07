using UnityEngine;
using Life;

namespace Life
{
    public class BlockDead : BlockBase
    {
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
        /// This method inherits parent class update method.
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
                            AudioManager.audioManager.BlockHover();
                        }
                    }
                }
            }
            
            // block has reach just about its target position (in place due to floating-point round issues)
            if ((transform.position - hoverPos).magnitude < 0.1f)
            {
                Replace();
                state = BlockState.Descend;
                AudioManager.audioManager.BlockDescend();
                
                // activate camera shake
                var cameraShake = mainCamera.GetComponent<CameraShake>();
                cameraShake.Shake(0.2f, 0.1f);
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
        /// This method replaces current block with new block.
        /// </summary>
        protected virtual void Replace()
        {
            AudioManager.audioManager.BlockReplace();
            
            var pos = transform.position;
                    
            // turn tile on grid to 'alive' status
            GameManager.gameManager.Cells[(int) pos.x, (int) pos.y] = true;
            
            // instantiate new 'alive' block in position as current block
            GameManager.gameManager.Blocks[(int)pos.x, (int)pos.y] = Instantiate(GameManager.gameManager.alive,
                new Vector3(pos.x, pos.y, hoverPos.z), Quaternion.Euler(-90, 0, 0),
                GameManager.gameManager.aliveParent);
            
            // play particle affect block replacement
            GameManager.gameManager.Blocks[(int)pos.x, (int)pos.y].GetComponent<BlockAlive>().explosion.Play();

            // set new instance of 'alive' block to descend state
            GameManager.gameManager.Blocks[(int)pos.x, (int)pos.y].GetComponent<BlockAlive>().state =
                BlockState.Descend;

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
            transform.position = Vector3.Lerp(transform.position, originalPos, 7f * Time.deltaTime);

            if ((transform.position - originalPos).magnitude < 0.001f)
            {
                state = BlockState.Idle;
            }
        }
    }
}
