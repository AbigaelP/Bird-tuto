using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Tu en es capable Abby <3
    // Je crois en mon Altesse <3
    // Je suis pr�sent pour aider au mieux :dab:
    public Player player; // objet du joueur

    //score global
    public Text scoreText; // affichage du score
    private int scoreValue; // valeur du score

    //score des pieces
    public Text scoreCoinText; // affichage du nombre de pi�ces
    private const int pointCoinsDefault = 5;  // nombre de points donn�s par les pi�ces


    public Text numberSeedsText; // affichage du nombre de graines
    public GameObject seeds; //affichage zone des graines

    //boutons
    public GameObject playButton; // butoun pour lance une partie
    public GameObject settingsButton; // bouton pour aller dans la s�lection du niveau
    public GameObject shopButton; // bouton pour aller dans l'�cran des skins
    public GameObject scoreButton; // bouton pour aller dans l'�cran des scores
    public GameObject quitButton; // bouton pour fermer le jeu

    //"�cran"
    public GameObject playground; // affichage du menu
    public GameObject settings; // affichage de l'�cran des niveau
    public GameObject shop; // affichage de l'�cran des skins 
    public GameObject gameOver; // affichage de l'�cran de game over
    public GameObject coins; // affichage de la zone des pi�ces
    public GameObject scoreBoard; // affichage de l'�cran des scores par niveau


    public int DifficultyLevel = 1; // level ou niveau de difficult�

    private void Awake()
    {
        Application.targetFrameRate = 60; // place le jeu en 60fps
        Coins.InitScore();
        Pause(); // mettre le jeu en "pause" par d�faut
        scoreCoinText.text = Coins.score.ToString(); // afficher directement le nombre de pi�ce
    }

    public void Play() // cette fonction permet de lancer le niveau
    {
        // le score et le nombre de graines sont remis � 0 avec leur affichage
        scoreValue = 0;
        scoreText.gameObject.SetActive(true);
        scoreText.text = scoreValue.ToString();
        numberSeedsText.text = "0";

        // tous les boutons sont d�sactiv�s
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        shopButton.SetActive(false);
        scoreButton.SetActive(false);
        quitButton.SetActive(false);

        // le temps et le joueur son r�activ�
        Time.timeScale = 1f;
        player.enabled = true;


        FindObjectOfType<Spawner>().enabled = true; // l'object qui g�r� l'apparition des tuyaux, des pi�ces et le reste est r�activ�
        Coins.SetDifficulty(DifficultyLevel); // le niveau de difficult� des pi�ces est mis � jour 
        if (DifficultyLevel == 5 || DifficultyLevel == 6 || DifficultyLevel == 9)
        { // l'afficahge des graines est r�activ�s pour les niveaux ou la graine appara�t
            seeds.SetActive(true);
        }
    }


    private void CleanUpLevel() // cette fonction permet de supprimer tous les objets, en dehors du joueur, qui sont apparus
    {
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

        Graines[] graines = FindObjectsOfType<Graines>();
        for (int i = 0; i < graines.Length; i++)
        {
            Destroy(graines[i].gameObject);
        }

        EnnemyBird[] enemies = FindObjectsOfType<EnnemyBird>();
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i].gameObject);
        }

        Cats[] cats = FindObjectsOfType<Cats>();
        for (int i = 0; i < cats.Length; i++)
        {
            Destroy(cats[i].gameObject);
        }
    }

    public void Pause() // d�sactive tout le jeu
    {
        Time.timeScale = 0f;
        player.enabled = false;
        FindObjectOfType<Spawner>().enabled = false;
        seeds.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }


    public void GameOver() // cette fonction permet d'afficher l'�cran de game over
    {
        Pause(); // le game over ne s'affiche qu'apr�s une partie, il faut d�sactiver le jeu
        CleanUpLevel(); // on nettoie ce qui reste du niveau

        // on d�sactive les �l�ments du niveau et on active l'affichage de gameover
        playground.SetActive(false);
        coins.SetActive(false);
        gameOver.SetActive(true);
        FindObjectOfType<GameOverManager>().SetGameOver(scoreValue, DifficultyLevel);
    }

    public void ComeBackToPlayground()
    { // cette fonction permet de d�sactiver l'affichage en cours pour faire r�appara�tre le menu du jeu avec tous les boutons
        playground.SetActive(true);
        settings.SetActive(false);
        shop.SetActive(false);
        gameOver.SetActive(false);
        scoreBoard.SetActive(false);
        playButton.SetActive(true);
        settingsButton.SetActive(true);
        shopButton.SetActive(true);
        scoreButton.SetActive(true);
        quitButton.SetActive(true);
        coins.SetActive(true);
    }

    public void OpenSettings() // permet d'afficher l'�cran du choix du niveau 
    {
        // on d�sactive l'affichage du menu et on active l'affichage du choix du niveau
        settings.SetActive(true);
        playground.SetActive(false);
        coins.SetActive(false);
    }

    public void OpenShop()
    { // permet d'afficher l'�cran des skins

        // on d�sactive l'affichage du menu et on active l'affichage de l'�cran de skins
        playground.SetActive(false);
        shop.SetActive(true);
    }

    public void OpenScore()
    { // permet d'afficher l'�cran des classement de scores par niveau

        // on d�sactive l'affichage du menu et on active l'affichage des classements de scores
        playground.SetActive(false);
        coins.SetActive(false);
        scoreBoard.SetActive(true);
        FindObjectOfType<ScoreboardDisplay>().ChooseLevel();
    }

    public void Quit()
    {
        Application.Quit();
    }


    public void IncreaseScore(int val) // permet d'augmenter le score du niveau actuel
    {
        scoreValue += val;
        scoreText.text = scoreValue.ToString();
    }

    public void UpdateCoins() // permet de mettre � jour l'affichage du nombre de pi�ces apr�s et d'augmenter le score en jeu 
    {
        scoreCoinText.text = Coins.score.ToString();
        IncreaseScore(pointCoinsDefault);
    }

    public void SetSeedsNumber(int seeds)
    { // permet de mettre � jour l'affichage
        numberSeedsText.text = seeds.ToString();
    }

    public void SetDifficutly(int level) // permet de mettre � jour le niveau de difficult� (choisir le niveau) et de revenir au menu
    {
        DifficultyLevel = level;
        FindObjectOfType<GameManager>().ComeBackToPlayground();
    }
}
