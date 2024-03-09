using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;

    //score globale
    private int score;
    public Text scoreText;

    //score des pieces
    public Text scoreCoinText;
    private int scoreCoin;
    private const int pointCoinsDefault = 5;

    //boutons
    public GameObject playButton;
    public GameObject gameOver;



    private void Awake()
    {
        Application.targetFrameRate = 60;

        Pause();
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);
        

        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }

        Coins[] coins = FindObjectsOfType<Coins>();

        for (int i = 0; i < coins.Length; i++)
        {
            Destroy(coins[i].gameObject);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }
    public void GameOver()
    {
        gameOver.SetActive(true);
        playButton.SetActive(true);
        

        Pause();
    }

    public void IncreaseScore(int val)
    {
        score += val;
        scoreText.text = score.ToString();
    }

    public void IncreaseScoreCoin()
    {
        scoreCoin++;
        scoreCoinText.text = scoreCoin.ToString();
        IncreaseScore(pointCoinsDefault);
    }
}
