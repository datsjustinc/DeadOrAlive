using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Life
{
    /// <summary>
    /// This class creates a classic cellular automaton simulation called the game of life.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // singleton of the class
        public static GameManager gameManager;
        
        // game state enum
        public enum GameState{Load, Start};
        public GameState state;
        
        // tile properties of tilemap
        [SerializeField] public int width = 10;
        [SerializeField] public int height = 10;
        [SerializeField] protected Tilemap tilemap;

        // block properties for tilemap
        [SerializeField] public GameObject alive;
        [SerializeField] public GameObject dead;
        [SerializeField] public Transform aliveParent;
        [SerializeField] public Transform deadParent;
        
        // grid representing tilemap and block map
        public bool[,] Cells;
        public GameObject[,] Blocks;

        // control variable for delays
        [SerializeField] protected bool check = true;
        
        // control variable for difficulty
        public float minDelay;
        public float maxDelay;
        public int underPopulation;
        public int overPopulation;
        public int revivalPopulation;

        /// <summary>
        /// This method is used to prevent duplicate GameManager objects.
        /// </summary>
        private void Awake()
        {
            if (gameManager == null)
            {
                gameManager = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // starting game state
            state = GameState.Load;
        }
        
        /// <summary>
        /// This method generates the starting tile placements of the grid.
        /// </summary>
        private IEnumerator Start()
        {
            // initializes a two-dimensional array ('grid') of bool values
            Cells = new bool[width, height];
            Blocks = new GameObject[width, height];

            // goes through rows in two-dimensional array
            for (var x = 1; x < width - 1; x++)
            {
                // goes through columns in two-dimensional array
                for (var y = 1; y < height - 1; y++)
                {
                    // randomizes bool values in current tile
                    Cells[x, y] = Random.value > 0.5f;

                    // generate blocks based on bool status of current tile and assign group
                    Blocks[x, y] = Instantiate(Cells[x, y] ? alive : dead, new Vector3(x, y, 0),
                        Quaternion.Euler(-90, 0, 0), Cells[x, y] ? aliveParent : deadParent);
                    
                    // amount of time to wait before next iteration of loop
                    yield return new WaitForSeconds(0.02f);
                }
            }

            // begin automating simulation
            state = GameState.Start;

            // set starting difficult delay range
            minDelay = 0.1f;
            maxDelay = 0.2f;
            underPopulation = 2;
            overPopulation = 3;
            revivalPopulation = 3;

            // increase game difficulty over time
            InvokeRepeating(nameof(Difficulty), 0f, 20f);
        }
        
        private void Update()
        {
            if(check && state == GameState.Start)
            {
                check = false;
                StartCoroutine(Automaton());
            }
        }

        /// <summary>
        /// This coroutine method manages the cellular automaton of the tile grid over time.
        /// </summary>
        private IEnumerator Automaton()
        {
            // goes through rows in two-dimensional array
            for (int x = 1; x < width - 1; x++)
            {
                // goes through rows in two-dimensional array
                for (int y = 1; y < height - 1; y++)
                {
                    // checks for number of 'living' tiles around current tile[x ,y]
                    int liveNeighbors = CountLiveNeighbors(x, y);

                    // if current tile is 'living'
                    if (Cells[x, y])
                    {
                        // if surrounding neighbors drop below 2, current tile 'dies' of underpopulation
                        // if surrounding neighbors rise above 3, current tile 'dies' of overpopulation
                        if (liveNeighbors < underPopulation || liveNeighbors > overPopulation)
                        {
                            // the current tile is 'dead'
                            Cells[x, y] = false;
                            
                            // destroy current tile and replace with 'dead' one
                            Destroy(Blocks[x, y]);
                            Blocks[x, y] = Instantiate(dead, new Vector3(x, y, 0), 
                                Quaternion.Euler(-90, 0, 0), deadParent);
                        }
                        else
                        {
                            // the current tile is 'alive'
                            Cells[x, y] = true;
                            
                            // destroy current tile and replace with 'alive' one
                            Destroy(Blocks[x, y]);
                            Blocks[x, y] = Instantiate(alive, new Vector3(x, y, 0), 
                                Quaternion.Euler(-90, 0, 0), aliveParent);
                        }
                    }
                    // if current tile is 'dead'
                    else
                    {
                        // if surrounding neighbors reach exactly 3, current tile 'revives' from reproduction
                        if (liveNeighbors == revivalPopulation)
                        {
                            // the current tile is 'alive'
                            Cells[x, y] = true;
                            
                            // destroy current tile and replace with 'alive' one
                            Destroy(Blocks[x, y]);
                            Blocks[x, y] = Instantiate(alive, new Vector3(x, y, 0), 
                                Quaternion.Euler(-90, 0, 0), aliveParent);
                            
                        }
                        else
                        {
                            // the current tile is 'dead'
                            Cells[x, y] = false;
                            
                            // destroy current tile and replace with 'dead' one
                            Destroy(Blocks[x, y]);
                            Blocks[x, y] = Instantiate(dead, new Vector3(x, y, 0), 
                                Quaternion.Euler(-90, 0, 0), deadParent);
                        }
                    }
                    
                    // randomize time to delay the tile checking loop
                    yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

                    // if last tile in grid is current being checked
                    if (x == width - 2 && y == height - 2)
                    {
                        // conditional statement in update function repeats check
                        check = true;
                    }
                }
            }
        }
        
        /// <summary>
        /// This method checks the number of neighboring tiles around a tile.
        /// </summary>
        /// <param name="x">row of the tile on grid to check</param>
        /// <param name="y">column of the tile on the grid to check </param>
        /// <returns>the amount of neighboring tiles</returns>
        private int CountLiveNeighbors(int x, int y)
        {
            // value stores count of surrounding neighbors
            int count = 0;

            // goes through rows prior to and following current cell[x, y]
            for (int i = -1; i <= 1; i++)
            {
                // goes through columns prior to and following current cell[x, y]
                for (int j = -1; j <= 1; j++)
                {
                    // skip current cell location
                    if (i == 0 && j == 0)
                    {
                        // next iteration of loop
                        continue;
                    }
                    
                    // update grid position based on rows and columns prior and following
                    var nx = x + i;
                    var ny = y + j;
                    
                    // position index out-of-bounds on grid
                    if (nx < 0 || nx > width || ny < 0 || ny > height)
                    {
                        // next iteration of loop
                        continue;
                    }
                    
                    // if neighboring cell is 'alive'
                    if (Cells[nx, ny])
                    {
                        //throw new
                        // increase 'live' neighbor tracker
                        count++;
                    }
                }
            }
            
            // return status of 'live' neighbors
            return count;
        }

        /// <summary>
        /// This method updates level difficulty data on UIManager.
        /// </summary>
        private void Difficulty()
        {
            // minor increase level difficulty change
            UIManager.uiManager.levelCount += 1;
            UIManager.uiManager.LevelChange();
            
            Invoke(nameof(CloseDifficulty), 2f);
        }

        /// <summary>
        /// This method closes level difficulty data after a delay.
        /// </summary>
        private void CloseDifficulty()
        {
            var reverse = !UIManager.uiManager.levelAnim.GetBool("Open");
            UIManager.uiManager.levelAnim.SetBool("Open", reverse);
            
            minDelay /= 1.5f;
            maxDelay /= 1.5f;

            // major increase level difficulty change
            if (UIManager.uiManager.levelCount != 0 && UIManager.uiManager.levelCount % 5 == 0)
            {
                // increase neighbors required to revive a 'dead' block
                revivalPopulation += 1;
            }
            
            // major increase level difficulty change
            if (UIManager.uiManager.levelCount != 0 && UIManager.uiManager.levelCount % 8 == 0)
            {
                // increase chance of blocks dying due to under population
                underPopulation += 1;
                
                // increase chance of blocks dying due to under population
                overPopulation -= 1;
            }
        }
        
    }
}
