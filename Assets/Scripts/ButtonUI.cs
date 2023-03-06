using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Life;

namespace Life
{
    public class ButtonUI : MonoBehaviour
    {
        // variables to control UI properties
        [SerializeField] private GameObject playerScore;
        [SerializeField] private GameObject enemyScore;
        [SerializeField] private TextMeshProUGUI playerText;
        [SerializeField] private TextMeshProUGUI enemyText;
        private Animator _playerAnim;
        private Animator _enemyAnim;
        private Button _playerButton;
        private Button _enemyButton;

        // variables hold child instances of blocks
        [SerializeField] private GameObject aliveGroup;
        [SerializeField] private GameObject deadGroup;

        private void Start()
        {
            // disable UI until start finishes execution
            playerScore.SetActive(false);
            enemyScore.SetActive(false);
            
            // grab UI animator at the start of execution
            _playerAnim = playerScore.GetComponent<Animator>();
            _enemyAnim = enemyScore.GetComponent<Animator>();
            
            // grab UI button at the start of execution
            _playerButton = playerScore.GetComponent<Button>();
            _enemyButton = enemyScore.GetComponent<Button>();
            
            // grab UI text at the start of execution
            playerText.text = "Alive: " + aliveGroup.transform.childCount;
            enemyText.text = "Dead: " + deadGroup.transform.childCount;
        }

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
            }
        }
        
        public void OnPlayerClick()
        {
            var reverse = !_playerAnim.GetBool("Open");
            _playerAnim.SetBool("Open", reverse);
        }

        public void OnEnemyClick()
        {
            var reverse = !_enemyAnim.GetBool("Open");
            _enemyAnim.SetBool("Open", reverse);
        }
    }
}
