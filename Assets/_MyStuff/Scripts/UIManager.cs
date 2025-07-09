using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameStartUI;
    public GameObject gameOverUI;
    public GameObject runtimeUI;
    public GameObject startTextUI;
    public GameObject pauseUI;
    public GameObject scenery;
    public TMP_Text countdownText;
    public void ShowStartUI()
    {
        scenery.SetActive(true);
        gameStartUI.SetActive(true);
        runtimeUI.SetActive(false);
        gameOverUI.SetActive(false);
        startTextUI.SetActive(true);
        pauseUI.SetActive(false);
    }

    public void ShowGameplayUI()
    {
        scenery.SetActive(false);
        gameStartUI.SetActive(false);
        runtimeUI.SetActive(true);
        gameOverUI.SetActive(false);
        startTextUI.SetActive(false);
        pauseUI.SetActive(false);
    }

    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
        runtimeUI.SetActive(false);
    }
    public void ShowPauseUI()
    {
        pauseUI.SetActive(true);
    }
    public IEnumerator FlashStartText()
    {
        for (int i = 0; i < 10; i++)
        {
            startTextUI.SetActive(true);
            yield return new WaitForSeconds(1f);
            startTextUI.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public Coroutine StartFlash()
    {
        Coroutine flash = StartCoroutine(FlashStartText());
        return flash;
    }

    public void ShowCountdownText(string text)
    {
        countdownText.text = text;
        countdownText.transform.localScale = Vector3.one * 2f;
        countdownText.gameObject.SetActive(true);
    }

    public void HideCountdownText()
    {
        countdownText.gameObject.SetActive(false);
    }

}

