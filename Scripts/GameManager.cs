/*using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }
}*/
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int moves = 20;
    public bool gameOver = false;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI movesText;
    public GameObject gameOverText;

    void Start()
    {
        UpdateUI();
        gameOverText.SetActive(false);
    }

    public void AddScore(int amount)
    {
        if(gameOver) return;

        score += amount;
        UpdateUI();
    }

    public void UseMove()
    {
        if(gameOver) return;

        moves--;
        UpdateUI();

        if(moves <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        movesText.text = "Moves: " + moves;
    }

    void GameOver()
    {
        gameOver = true;
        gameOverText.SetActive(true);
    }
}