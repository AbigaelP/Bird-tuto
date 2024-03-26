using UnityEngine;
using UnityEngine.UI;

public class PipeSkins : MonoBehaviour
{
    public string id;
    public int price; // le prix du skin 
    public bool unlocked; // indique si le skin est dévérouillé ou non
    public Sprite sprite;  // Le sprite correspondant au skin
    public Text buttonText; // Texte à afficher sur le bouton en fonction de si le skin a été dévérouillé ou non

    private void Awake() {
        if(!unlocked) {
            buttonText.text = price + " coins";
        } 
        if(PlayerPrefs.HasKey("PIPE_SKIN_"+id) && PlayerPrefs.GetInt("PIPE_SKIN_"+id) == 1) {
            unlocked = true;
            buttonText.text = "Equip";
        }
    }

    public void PurchaseOrEquip() {
        if(unlocked) { // permet d'équiper le skin si il a été dévérouillé
                FindObjectOfType<SkinManager>().SetPipeSkin(this); // on met à jour le skin par défaut des tuyaux
                buttonText.text = "Selected"; // le texte du bouton doit être modifié en conséquence
        } else {
            if(Coins.score >= price) { // si le skin n'a pas été dévérouillé mais que le joueur a assez de pièces
                Coins.score -= price; // on réduit le nombre de pièces du jouer par rapport au prix du skin
                unlocked = true; // le skin est dévérouillé
                buttonText.text = "Equip"; // le texte du bouton doit être modifié en conséquence
                FindObjectOfType<GameManager>().UpdateCoins(); // l'affichage du nombre de pièces est mis à jour
                PlayerPrefs.SetInt("PIPE_SKIN_"+id, 1);
                Coins.SaveScore();
            }
        }
    } 
}
