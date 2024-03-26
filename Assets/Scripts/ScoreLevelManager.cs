using System.Collections.Generic;
using UnityEngine;

public class ScoreLevelManager : MonoBehaviour
{
    private readonly int rankMin = 5; // le rang le plus bas possible dans le classement

    // liste des scores à afficher pour chaque niveau
    private List<Score> level1 = new(); 
    private List<Score> level2 = new();
    private List<Score> level3 = new();
    private List<Score> level4 = new();
    private List<Score> level5 = new();
    private List<Score> level6 = new();
    private List<Score> level7 = new();
    private List<Score> level8 = new();
    private List<Score> level9 = new();

    void Awake() {
        // chaque liste de score est initialisé
        level1 = SetLevelScore(1);
        level2 = SetLevelScore(2);
        level3 = SetLevelScore(3);
        level4 = SetLevelScore(4);
        level5 = SetLevelScore(5);
        level6 = SetLevelScore(6);
        level7 = SetLevelScore(7);
        level8 = SetLevelScore(8);
        level9 = SetLevelScore(9);
    }

    private List<Score> SetLevelScore(int level) {
        List<Score> scores = new();
        for( int i = 0; i < rankMin; ++i) {
            string jsonString = PlayerPrefs.GetString("LEVEL_"+level+"_"+i); // on récupére la liste du niveau dans les préférences du joueur        
            if(jsonString != null && jsonString != string.Empty ) {
                Score score = JsonUtility.FromJson<Score>(jsonString);
                scores.Add(score);
            } else {
                break;
            }
        }
        for(int i = scores.Count; i < rankMin; ++i) { // si le rang minimum n'a pas été atteint, on rempli les rangs restants
            Score emptyScore = new()
            {
                Name = "Unknown",
                Value = 0
            };
            scores.Add(emptyScore);
        }
        return scores;
    }

    
    public int GetRankScore(int level, int score) { // cette fonction permet de récupérer le rang où serait ajouter le score pour la list du niveau
        List<Score> scores = GetLevelScore(level);
        for(int i = 0; i < scores.Count; ++i) { // tant que le score est inférieur au score récupéré, on continue de chercher
            if(scores[i].Value < score ) {
                return i+1; // si le score est supérieur 
            }
        }
        return -1; // on renvoie -1 si le score est trop bas pour être dans la liste des scores du niveau
    }

    public void SaveScore(int level, Score score, int position) { // cette fonction permet de sauvegarder un nouveau record et mettre à jour la liste de score du niveau choisi
        List<Score> scores = GetLevelScore(level);
        scores.Insert(position-1, score); // le score est inséré à la position qui correspond au rang
        scores.RemoveAt(scores.Count-1); // la dernière valeur est supprimée car hors rang
        Debug.Log(scores);

        // ici on transforme chaque score en format json pour les enregistrer dans les préférences du joueur par niveau
        for(int i = 0; i < scores.Count; ++i) {
            string json = JsonUtility.ToJson(scores[i]);
            PlayerPrefs.SetString("LEVEL_"+ level+"_"+i, json);
        }
        PlayerPrefs.Save();
    }

    public List<Score> GetLevelScore(int level) { // cette fonction permet de récupérer la liste des scores du niveau choisi
        return level switch
        {
            1 => level1,
            2 => level2,
            3 => level3,
            4 => level4,
            5 => level5,
            6 => level6,
            7 => level7,
            8 => level8,
            9 => level9,
            _ => level1,
        };
    }
}
