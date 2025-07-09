using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI coinsText;
    private void Start()
    {
        highScoreText.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        coinsText.text = PlayerPrefs.GetInt("coins", 0).ToString();
    }
    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreUI();
    }
    private void UpdateScoreUI()
    {
        if (scoreText == null) return;

        scoreText.text = score.ToString();
        if(score > PlayerPrefs.GetInt("highscore", 0))
        {
            PlayerPrefs.SetInt("highscore", score);
            highScoreText.text = score.ToString();
        }
    }
    public void UpdateCoinsUI()
    {
        coinsText.text = PlayerPrefs.GetInt("coins", 0).ToString();
    }
}
