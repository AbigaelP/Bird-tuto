using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public PlayerSkins player; // dernier skin choisi pour le joueur
    public PipeSkins pipe; //dernier skin choisi pour les tuyaux

    public void SetPlayerSkin(PlayerSkins skin) { // cette fonction permet de mettre à jour le skin du joueur
        player.buttonText.text = "Equip";
        player = skin;
        
        FindObjectOfType<GameManager>().player.sprites = player.sprites; // on échange les sprites du joueur avec ceux du skin
        FindObjectOfType<GameManager>().player.GetComponent<SpriteRenderer>().sprite = player.sprites[0]; // on change le skin affiché du joueur
    }

    public void SetPipeSkin(PipeSkins skin) { // cette fonction permet de mettre à jour le skin des tuyaux
        pipe.buttonText.text = "Equip";
        pipe = skin;
    }
}
