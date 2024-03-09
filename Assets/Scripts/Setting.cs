using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Setting : MonoBehaviour
{
    public static int difficultyLevel = 1;

    private void level1()
    {
        Setting.difficultyLevel = 1;
        SceneManager.LoadScene("bby");
    }
    private void level2()
    {
        Setting.difficultyLevel = 2;
        SceneManager.LoadScene("bby");
    }
    private void level3()
    {
        Setting.difficultyLevel = 3;
        SceneManager.LoadScene("bby");
    }
    private void level4()
    {
        Setting.difficultyLevel = 4;
        SceneManager.LoadScene("bby");
    }
    private void level5()
    {
        Setting.difficultyLevel = 5;
        SceneManager.LoadScene("bby");
    }
}
