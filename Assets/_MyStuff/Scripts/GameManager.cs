using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameStartUI;
    public GameObject gameOverUI;
    public GameObject runtimeUI;
    public GameObject scoreUI;
    public GameObject startUI;

    public MapGen MapController;

    public int score=0;
    public bool gameOver =false;
    public bool starting = true;
    public Coroutine currentCoroutine;
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
        gameStartUI.SetActive(true);
        runtimeUI.SetActive(false);
        currentCoroutine = StartCoroutine(Flash());
        MapController.scrollSpeed = 0;
    }
    public void GameOver()
    {
        gameOver=true;
        gameOverUI.SetActive(true);
        MapController.scrollSpeed = 0;
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
        if (starting)
        {
            starting=false;
            gameStartUI.SetActive(false);
            runtimeUI.SetActive(true);
            MapController.scrollSpeed = 5f;

            if (currentCoroutine == null)
            {
                return;
            }
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
            startUI.SetActive(false);
        }
        
    }
    public void AddPoints(int points)
    {
        score += points;
        scoreUI.GetComponent<TextMeshProUGUI>().text = "" + score;
    }
    public void ResetPoints()
    {
        score += 0;
        scoreUI.GetComponent<TextMeshProUGUI>().text = "" + score;
    }
    public IEnumerator Flash()
    {
        for (int i = 0; i < 10; i++)
        {
            startUI.SetActive(true);
            yield return new WaitForSeconds(1f);
            startUI.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
