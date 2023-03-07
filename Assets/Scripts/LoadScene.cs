using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the scene load transition.
/// </summary>
public class LoadScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    /// <summary>
    /// This method opens a new scene.
    /// </summary>
    public void OpenScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    
    /// <summary>
    /// This method quits the application.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
