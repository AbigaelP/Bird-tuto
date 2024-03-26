using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //Tu en es capable Abby <3
    // Je crois en mon Altesse <3
    // Je suis présent pour aider au mieux :dab:
    public Player player; // objet du joueur

    //score global
    public Text scoreText; // affichage du score
    private int scoreValue; // valeur du score

    //score des pieces
    public Text scoreCoinText; // affichage du nombre de pièces
    private const int pointCoinsDefault = 5;  // nombre de points donnés par les pièces


    public Text numberSeedsText; // affichage du nombre de graines
    public GameObject seeds; //affichage zone des graines

    //boutons
    public GameObject playButton; // butoun pour lance une partie
    public GameObject settingsButton; // bouton pour aller dans la sélection du niveau
    public GameObject shopButton; // bouton pour aller dans l'écran des skins
    public GameObject scoreButton; // bouton pour aller dans l'écran des scores
    public GameObject quitButton; // bouton pour fermer le jeu

    //"écran"
    public GameObject playground; // affichage du menu
    public GameObject settings; // affichage de l'écran des niveau
    public GameObject shop; // affichage de l'écran des skins 
    public GameObject gameOver; // affichage de l'écran de game over
    public GameObject coins; // affichage de la zone des pièces
    public GameObject scoreBoard; // affichage de l'écran des scores par niveau


    public int DifficultyLevel = 1; // level ou niveau de difficulté

    private void Awake()
    {
        Application.targetFrameRate = 60; // place le jeu en 60fps
        Coins.InitScore();
        Pause(); // mettre le jeu en "pause" par défaut
        scoreCoinText.text = Coins.score.ToString(); // afficher directement le nombre de pièce
    }

    public void Play() // cette fonction permet de lancer le niveau
    {
        // le score et le nombre de graines sont remis à 0 avec leur affichage
        scoreValue = 0;
        scoreText.gameObject.SetActive(true);
        scoreText.text = scoreValue.ToString();
        numberSeedsText.text = "0";

        // tous les boutons sont désactivés
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        shopButton.SetActive(false);
        scoreButton.SetActive(false);
        quitButton.SetActive(false);

        // le temps et le joueur son réactivé
        Time.timeScale = 1f;
        player.enabled = true;


        FindObjectOfType<Spawner>().enabled = true; // l'object qui gèré l'apparition des tuyaux, des pièces et le reste est réactivé
        Coins.SetDifficulty(DifficultyLevel); // le niveau de difficulté des pièces est mis à jour 
        if (DifficultyLevel == 5 || DifficultyLevel == 6 || DifficultyLevel == 9)
        { // l'afficahge des graines est réactivés pour les niveaux ou la graine apparaît
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

    public void Pause() // désactive tout le jeu
    {
        Time.timeScale = 0f;
        player.enabled = false;
        FindObjectOfType<Spawner>().enabled = false;
        seeds.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }


    public void GameOver() // cette fonction permet d'afficher l'écran de game over
    {
        Pause(); // le game over ne s'affiche qu'après une partie, il faut désactiver le jeu
        CleanUpLevel(); // on nettoie ce qui reste du niveau

        // on désactive les éléments du niveau et on active l'affichage de gameover
        playground.SetActive(false);
        coins.SetActive(false);
        gameOver.SetActive(true);
        FindObjectOfType<GameOverManager>().SetGameOver(scoreValue, DifficultyLevel);
    }

    public void ComeBackToPlayground()
    { // cette fonction permet de désactiver l'affichage en cours pour faire réapparaître le menu du jeu avec tous les boutons
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

    public void OpenSettings() // permet d'afficher l'écran du choix du niveau 
    {
        // on désactive l'affichage du menu et on active l'affichage du choix du niveau
        settings.SetActive(true);
        playground.SetActive(false);
        coins.SetActive(false);
    }

    public void OpenShop()
    { // permet d'afficher l'écran des skins

        // on désactive l'affichage du menu et on active l'affichage de l'écran de skins
        playground.SetActive(false);
        shop.SetActive(true);
    }

    public void OpenScore()
    { // permet d'afficher l'écran des classement de scores par niveau

        // on désactive l'affichage du menu et on active l'affichage des classements de scores
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

    public void UpdateCoins() // permet de mettre à jour l'affichage du nombre de pièces après et d'augmenter le score en jeu 
    {
        scoreCoinText.text = Coins.score.ToString();
        IncreaseScore(pointCoinsDefault);
    }

    public void SetSeedsNumber(int seeds)
    { // permet de mettre à jour l'affichage
        numberSeedsText.text = seeds.ToString();
    }

    public void SetDifficutly(int level) // permet de mettre à jour le niveau de difficulté (choisir le niveau) et de revenir au menu
    {
        DifficultyLevel = level;
        FindObjectOfType<GameManager>().ComeBackToPlayground();
    }
}
