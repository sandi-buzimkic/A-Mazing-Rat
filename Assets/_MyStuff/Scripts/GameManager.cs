using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameState { Start, Playing, GameOver, Pause}

public class GameFlowController: MonoBehaviour
{
    public static GameFlowController Instance;

    public MapGen MapController;
    public UIManager UIManager;
    public ScoreManager ScoreManager;


    public int score=0;
    public GameState GameState { get; private set; } = GameState.Start;

    [SerializeField] private Image slowTimeUI;
    private Coroutine slowTimeCoroutine;
    public Coroutine flashCoroutine;
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
        Application.targetFrameRate = 60;
        SetState(GameState.Start);
    }
    public void SetState(GameState newState)
    {
        GameState = newState;

        switch (newState)
        {
            case GameState.Start:

                UIManager?.ShowStartUI();
                UIManager.StartFlash();

                MapController.scrollSpeed = 0f;
                break;

            case GameState.Playing:

                Time.timeScale = 1f;
                UIManager?.ShowGameplayUI();
                if(MapController.scrollSpeed == 0f)
                {
                    MapController.scrollSpeed = 5f;
                }
                StopFlashing();

                break;

            case GameState.Pause:

                UIManager?.ShowPauseUI();

                Time.timeScale = 0f;

                break;

            case GameState.GameOver:
                UIManager?.ShowGameOverUI();

                MapController.scrollSpeed = 0f;
                break;
        }
    }

    public void StartGame()
    {
        if (GameState == GameState.Start)
        {
            SetState(GameState.Playing);
        }
    }

    public void GameOver()
    {
        if (GameState == GameState.Playing)
        {
            SetState(GameState.GameOver);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void StopFlashing()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }
    }
    public void Pause()
    {
        SetState(GameState.Pause);
    }
    public void ContinueGame()
    {
        StartCoroutine(CountdownToResume(3f));
    }
    public void Shop()
    {
        SceneManager.LoadScene("Shop");
    }

    public IEnumerator CountdownToResume(float duration = 3f)
    {
        float remaining = duration;
        UIManager?.ShowGameplayUI();
        while (remaining > 0)
        {
            UIManager.ShowCountdownText(Mathf.CeilToInt(remaining).ToString());

            float t = 0f;
            Vector3 startScale = Vector3.one * 2f;
            Vector3 endScale = Vector3.one;

            while (t < 1f)
            {
                t += Time.unscaledDeltaTime * 2f; 
                UIManager.countdownText.transform.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }

            remaining--;
            yield return new WaitForSecondsRealtime(0.2f);
        }

        UIManager.HideCountdownText();
        SetState(GameState.Playing);
    }

    public void StartTimeSlow()
    {
        if (slowTimeCoroutine != null)
            StopCoroutine(slowTimeCoroutine);

        slowTimeCoroutine = StartCoroutine(SlowTime());
    }

    public IEnumerator SlowTime()
    {
        if (slowTimeUI != null)
        {
            slowTimeUI.fillAmount = 1f;
            slowTimeUI.gameObject.SetActive(true);
        }

        float duration = 5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (GameState != GameState.Pause)
            {
                Time.timeScale = 0.6f;
                elapsed += Time.unscaledDeltaTime;

                if (slowTimeUI != null)
                {
                    slowTimeUI.fillAmount = 1f - (elapsed / duration);
                }
            }
            yield return null;
        }

        Time.timeScale = 1f;

        if (slowTimeUI != null)
        {
            slowTimeUI.gameObject.SetActive(false);
        }

        slowTimeCoroutine = null;
    }


}
