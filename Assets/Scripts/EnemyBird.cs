using UnityEngine;

public class EnnemyBird : MonoBehaviour
{
    private readonly float speed = 7f; // vitesse de déplacement des oiseaux ennemis

    private float leftEdge; // la gauche de l'écran

    private SpriteRenderer spriteRenderer; // permettra de mettre à jour le sprite afficher  sur l'écran

    public Sprite[] sprites; // liste des sprites à afficher pour gérer l'animation des oiseaux ennemis

    private int spriteIndex; // le numéro du sprite afficher sur l'écran

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // récupérer l'object spriteRenderer du GameObject
    }

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x -1f; // calcul la position la plus à gauche de l'écran
    }
    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.left; // déplacer la pièce pour donner l'impression d'avancer
        
        if (transform.position.x < leftEdge) // on cherche à détruire la pièce une fois qu'elle disparaît de l'écran
        {
            Destroy(gameObject);
        }

        AnimateSprite(); // mettre à jour le sprite à afficher
    }

    private void AnimateSprite() // cette fonction permet d'alterner les sprites pour gérer l'animation
    {
        spriteIndex++;
        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }
        spriteRenderer.sprite = sprites[spriteIndex];
    }
}

