using UnityEngine;

public class Cats : MonoBehaviour
{
    private readonly float speed = 5f; //vitesse de déplacement des chats

    private float leftEdge; // la gauche de l'écran 

    private readonly int points = 10; // le nombre de point que rapport un chat touché par une graine
    private bool touched = false; // indique si oui ou non le chat a été touché pour ne pas le toucher plus d'une fois

    public Sprite touchedSprite; // le sprite à afficher quand le chat se fait toucher par une graine

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x -1f; // calcul la position la plus à gauche de l'écran
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.left; // déplacer le chat pour donner l'impression d'avancer
        
        if (transform.position.x < leftEdge) // on cherche à détruire l'objet chat une fois qu'il disparaît de l'écran
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Seed") && !touched && other.GetComponent<Graines>().isFalling) { // on vérifie si le chat a été touché par une graine lâcher par le joueur
            touched = true; // le chat ne pourra plus être touché par la suite
            GetComponent<SpriteRenderer>().sprite = touchedSprite; // on change le sprite du chat pour montrer qu'il a été touché
            FindObjectOfType<GameManager>().IncreaseScore(points); // le score du joueur doit être incrémenté du nombre de points choisi
            Destroy(other.gameObject); // la graine doit être détruite une fois qu'elle a touché le chat
        }
    }
}
