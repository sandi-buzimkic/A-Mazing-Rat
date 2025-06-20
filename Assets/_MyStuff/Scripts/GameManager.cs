using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public static GameManager Instance;
    bool gameOver=false;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        Time.timeScale = 0f;
    }
    public void GameOver()
    {
        gameOver=true;
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
    public void StartGame()
    {
        if (gameOver)
        {
            return;
        }
        Time.timeScale = 1f;
    }
}
