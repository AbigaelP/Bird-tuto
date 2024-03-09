using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Setting : MonoBehaviour
{
    public static int difficultyLevel = 1;

    public void level1()
    {
        difficultyLevel = 1;
        setDifficulty();
        SceneManager.LoadScene("bby");
    }
    public void level2()
    {
        difficultyLevel = 2;
        setDifficulty();
        SceneManager.LoadScene("bby");
    }
    public void level3()
    {
        difficultyLevel = 3;
        setDifficulty();
        SceneManager.LoadScene("bby");
    }
    public void level4()
    {
        difficultyLevel = 4;
        setDifficulty();
        SceneManager.LoadScene("bby");
    }
    public void level5()
    {
        difficultyLevel = 5;
        setDifficulty();
        SceneManager.LoadScene("bby");
    }
    public void level6()
    {
        difficultyLevel = 6;
        setDifficulty();
        SceneManager.LoadScene("bby");
    }

    private void setDifficulty()
    {
        Spawner.setDifficulty(Setting.difficultyLevel);
        Coins.setDifficulty(Setting.difficultyLevel);
    }
}
