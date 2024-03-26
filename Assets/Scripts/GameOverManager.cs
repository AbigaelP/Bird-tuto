using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public InputField inputName; // zone de saisie de texte du nom du joueur
    public GameObject newRecord; // zone contenant l'affichage à avoir si le joueur fait un nouveau cord
    public Text scoreText; // texte où est affiché le score du joueur
    public Text rankText; // text où est affiche le rang du nouveau score
    public GameObject usernameMissingText; // texte affichant un message d'erreur si le joueur ne saisie pas de nom pour le nouveau score
    private ScoreLevelManager scoreLevelManager; // classe gérant les scores 
    private int currentRank = 0; // rang actuel du score du joueur
    private int currentScore = 0; // score actuel du joueur
    private int currentDifficulty = 0; // niveau de difficulté correspondant au niveau du score

    void Awake() {
        scoreLevelManager = FindObjectOfType<ScoreLevelManager>(); // récupérer le gestionnaire de score par niveau
    }


    public void SetGameOver(int score, int difficulty) { // cette fonction permet d'afficher les donnes à afficher à l'écran
        // par défaut, on désactive la zone de nouveau record et le message d'erreur
        newRecord.SetActive(false);
        usernameMissingText.SetActive(false);
        
        currentScore = score;
        scoreText.text = currentScore.ToString();
        currentDifficulty = difficulty;

        currentRank = scoreLevelManager.GetRankScore(currentDifficulty, currentScore); // permet de récupérer la place du nouveau score dans le classement

        if(currentRank != -1) { // si le nouveau score entre dans le classement on gère l'affichage de la zone "nouveau record"
            newRecord.SetActive(true);
            string rank;
            switch(currentRank) {
                case 1: 
                    rank = "1ST";
                    break;
                case 2: 
                    rank = "2ND";
                    break;
                case 3: 
                    rank = "3TH";
                    break;
                default:
                    rank = currentRank + "TH";
                    break;
            }
            rankText.text = "Rank : "+rank;
        }
    }
    
    public void BackToPlayground() { // permet de revenir à l'écran d'accueil et d'enregistrer le nouveau record si il y en a un 
        if(newRecord.activeSelf) { 
            if(inputName.text.Length == 0) { // si aucun nom n'a été saisi pour le nouveau record, on affiche le message d'erreur à l'utiliser et on ne fait rien
                usernameMissingText.SetActive(true);
                return;
            } else { // sinon, on enregistre le nouveau record avec le pseudo à son emplacement
                Score score = new Score()
                {
                    Name = inputName.text,
                    Value = currentScore
                };
                scoreLevelManager.SaveScore(currentDifficulty, score, currentRank);
            }
        }
        Coins.SaveScore();
		FindObjectOfType<GameManager>().ComeBackToPlayground(); // on retourne à l'écran d'accueil
    }
}
