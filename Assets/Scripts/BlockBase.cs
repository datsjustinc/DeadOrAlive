using UnityEngine;

namespace Life
{
    /// <summary>
    /// This class defines the features of the block class.
    /// </summary>
    public class BlockBase : MonoBehaviour
    {
        [SerializeField] protected Camera mainCamera;

        // position properties of hover method
        [SerializeField] protected Vector3 originalPos;
        [SerializeField] protected Vector3 hoverPos;
        
        // block state enum status
        public enum BlockState{Idle, Hover, Descend};
        public BlockState state;

        protected virtual void Start()
        {
            mainCamera = Camera.main;
            originalPos = transform.position;
            originalPos.z = 0f;
            hoverPos = new Vector3(originalPos.x, originalPos.y, -2f);
            state = BlockState.Idle;
        }

        /// <summary>
        /// This method manages the state of the block's actions.
        /// </summary>
        protected virtual void Update()
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
            }
            
            // block has reach just about its target position (in place due to floating-point round issues)
            if ((transform.position - hoverPos).magnitude < 0.1f)
            {
                state = BlockState.Descend;
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
        /// This method raises the block to its target hover position over time.
        /// </summary>
        protected virtual void Hover()
        {
            transform.position = Vector3.Lerp(transform.position, hoverPos, 7f * Time.deltaTime);
        }

        /// <summary>
        /// This method lowers the block back to its original starting position over time.
        /// </summary>
        protected virtual void Descend()
        {
            transform.position = Vector3.Lerp(transform.position, originalPos, 7f * Time.deltaTime);

            if ((transform.position - originalPos).magnitude < 0.001f)
            {
                state = BlockState.Idle;
            }
        }
    }
}

