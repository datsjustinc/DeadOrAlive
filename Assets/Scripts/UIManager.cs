using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Life;

namespace Life
{
    public class UIManager : MonoBehaviour
    {
        // singleton of the class
        public static UIManager uiManager;
        
        // variables to control UI properties
        [SerializeField] private GameObject playerScore;
        [SerializeField] private GameObject enemyScore;
        [SerializeField] private TextMeshProUGUI playerText;
        [SerializeField] private TextMeshProUGUI enemyText;
        [SerializeField] private GameObject levelDisplay;
        [SerializeField] private TextMeshProUGUI levelText;
        
        [SerializeField] private TextMeshProUGUI gameWin;
        [SerializeField] private TextMeshProUGUI gameLose;
        
        // current game difficulty level
        public int levelCount;
        
        private Animator _playerAnim;
        private Animator _enemyAnim;
        public Animator levelAnim;
        
        private Button _playerButton;
        private Button _enemyButton;

        // variables hold child instances of blocks
        [SerializeField] private GameObject aliveGroup;
        [SerializeField] private GameObject deadGroup;
        
        [SerializeField] private string sceneWin;
        [SerializeField] private string sceneLose;
        
        /// <summary>
        /// This method is used to prevent duplicate UIManager objects.
        /// </summary>
        private void Awake()
        {
            if (uiManager == null)
            {
                uiManager = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// This method sets up UI on screen to be ready.
        /// </summary>
        private void Start()
        {
            // disable UI until start finishes execution
            playerScore.SetActive(false);
            enemyScore.SetActive(false);

            // disable win-lose condition UI
            gameWin.enabled = false;
            gameLose.enabled = false;

            // grab UI animator at the start of execution
            _playerAnim = playerScore.GetComponent<Animator>();
            _enemyAnim = enemyScore.GetComponent<Animator>();
            levelAnim = levelDisplay.GetComponent<Animator>();
            
            // grab UI button at the start of execution
            _playerButton = playerScore.GetComponent<Button>();
            _enemyButton = enemyScore.GetComponent<Button>();
            
            // grab UI text at the start of execution
            playerText.text = "Alive: " + aliveGroup.transform.childCount;
            enemyText.text = "Dead: " + deadGroup.transform.childCount;

            levelCount = 0;
        }

        /// <summary>
        /// This method updates the UI during gameplay.
        /// </summary>
        private void Update()
        {
            if (GameManager.gameManager.state == GameManager.GameState.Start)
            {
                // enable UI after start finishes execution
                playerScore.SetActive(true);
                enemyScore.SetActive(true);
                var alive = aliveGroup.transform.childCount;
                var dead = deadGroup.transform.childCount;
                
                // continue to keep track of dead and alive block tiles
                playerText.text = "Alive: " + Mathf.Round(alive * (100f / (alive + dead))) + "%";
                enemyText.text = "Dead: " + Mathf.Round(dead * (100f / (alive + dead))) + "%";
                levelText.text = "Level " + levelCount;

                if (Mathf.Round(alive * (100f / (alive + dead))) > 70f)
                {
                    gameWin.enabled = true;
                    SceneManager.LoadScene(sceneWin);
                }

                if (Mathf.Round(dead * (100f / (alive + dead))) > 90f)
                {
                    gameLose.enabled = true;
                    SceneManager.LoadScene(sceneLose);
                }
            }
        }
        
        /// <summary>
        /// This method open and closes player UI.
        /// </summary>
        public void OnPlayerClick()
        {
            var reverse = !_playerAnim.GetBool("Open");
            _playerAnim.SetBool("Open", reverse);
        }

        /// <summary>
        /// This method open and closes enemy UI.
        /// </summary>
        public void OnEnemyClick()
        {
            var reverse = !_enemyAnim.GetBool("Open");
            _enemyAnim.SetBool("Open", reverse);
        }

        /// <summary>
        /// This method opens and closes difficulty level UI.
        /// </summary>
        public void LevelChange()
        {
            var reverse = !levelAnim.GetBool("Open");
            levelAnim.SetBool("Open", reverse);
        }
    }
}
