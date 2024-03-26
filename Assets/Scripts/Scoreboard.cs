using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardDisplay : MonoBehaviour
{
    public Transform templateScore; // un template qui correspond à l'affichage de chaque ligne de score
    public Transform containerScore; // l'emplacement des scores
    public Dropdown selectLevel; // une liste déroulante des niveaux

    // la liste de tous les niveaux du jeu
    private readonly List<string> levels = new (){"Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8", "Level 9" };

    private List<Transform> listLines = new(); // la liste des lignes de score à afficher à l'écran

    void Start()
    {
        selectLevel.AddOptions(levels); // permet d'ajouter la liste des niveaux dans la liste déroulante
        templateScore.gameObject.SetActive(false); // le template par défaut doit être caché
        ChooseLevel(); // mettre à jour l'affichage avec le niveau par défaut
    }

    public void ChooseLevel() { // cette fonction permet de choisir le niveau dont on souhaite voir la liste des scores
        CleanPreviousLine(); // on supprimer, l'affichage précédent
        List<Score> scores = FindObjectOfType<ScoreLevelManager>().GetLevelScore(selectLevel.value+1); // on récupère la liste du niveau de la liste de récoulante
        for(int i = 0; i < scores.Count; i++) {

            
            Transform scoreLine = Instantiate(templateScore, containerScore); // on crée une copie du template de la ligne des scores
            RectTransform lineRectTransform = scoreLine.GetComponent<RectTransform>();
            lineRectTransform.anchoredPosition = new Vector2(0, -lineRectTransform.rect.height*i); // permet de place les lignes dans l'ordre en les décalant vers le bas par rapport à la valeur de i
            scoreLine.gameObject.SetActive(true); // la ligne doit être affichée
            listLines.Add(scoreLine); // on ajoute la ligne dans une liste afin de pouvoir la supprimer plu tard
            
            int rank = i+1;
            string rankText; //on choisit l'affichage du rang en fonction de la position du score
            switch(rank) {
                case 1: 
                    rankText = "1ST";
                    break;
                case 2: 
                    rankText = "2ND";
                    break;
                case 3: 
                    rankText = "3TH";
                    break;
                default:
                    rankText = rank + "TH";
                    break;
            }
            //dans la copie du template, on récupère chaque zone de texte qui correspond au range, au non et à la valeur du score
            scoreLine.Find("RankValue").GetComponent<Text>().text = rankText; 
            scoreLine.Find("NameValue").GetComponent<Text>().text = scores[i].Name;
            scoreLine.Find("ScoreValue").GetComponent<Text>().text = scores[i].Value.ToString();
            
        }
    }

    private void CleanPreviousLine() { // cette fonction permet de remettre à 0 l'affichage des lignes de scores
        foreach(Transform line in listLines) {
            Destroy(line.gameObject);
        }
        listLines.Clear();
    }
}
