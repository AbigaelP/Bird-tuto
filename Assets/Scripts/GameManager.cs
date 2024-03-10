using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;

    //score globale
    private int score;
    public Text scoreText;

    //score des pieces
    public Text scoreCoinText;
    private const int pointCoinsDefault = 5;

    //score des graines
    public Text scoreSeedText;

    //boutons
    public GameObject playButton;
    public GameObject gameOver;
    public GameObject settingButton;



    private void Awake()
    {
        Application.targetFrameRate = 60;
        scoreCoinText.text = Coins.score.ToString();
        Pause();
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);
        settingButton.SetActive(false);
        

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

        Seeds[] seeds = FindObjectsOfType<Seeds>();

        for (int i = 0; i < seeds.Length; i++)
        {
            Destroy(seeds[i].gameObject);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        settingButton.SetActive(true);
        gameOver.SetActive(false);
    }
    public void GameOver()
    {
        Pause();
        gameOver.SetActive(true);
        playButton.SetActive(true);
        
    }

    public void IncreaseScore(int val)
    {
        score += val;
        scoreText.text = score.ToString();
    }

    public void IncreaseScoreCoin(int score)
    {
        scoreCoinText.text = score.ToString();
        IncreaseScore(pointCoinsDefault);
    }

    public void IncreaseScoreSeed(int number)
    {
        scoreSeedText.text = number.ToString();
    }

    public void OpenSetting()
    {
        SceneManager.LoadScene("Setting");
    }
}
