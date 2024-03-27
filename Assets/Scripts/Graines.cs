using UnityEngine;

public class Graines : MonoBehaviour
{
    private readonly float speed = 5f; // la vitesse de déplacement des graines

	private readonly float speedFalling = 2f; // la vitesse de déplacement de la graine vers la gauche quand elle tombe

    private float leftEdge; 


	public bool isFalling = false; // indique si la graine est en train de tomber ou non

	private const int defaultPoint = 10; // nombre de point que rapport les graines quand elles touchent un oisillon

	private readonly float gravity = 7.5f; // la gravité qui affecte la graine

	private void Start()
    {
		leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f; // calcul la position la plus à gauche de l'écran
	}

    void Update()
    {
		if(!isFalling) { // si la graine ne tomber pas, elle se déplace vers la gauche jusqu'à atteindre la gauche de l'écran
			transform.position += speed * Time.deltaTime * Vector3.left;
			if (transform.position.x < leftEdge) {
				Destroy(gameObject);
			}
		} else { // sinon elle se déplace moins vite vers la gauche et tombe jusqu'à être détruite
			transform.position += gravity * Time.deltaTime * Vector3.down;
			transform.position += speedFalling * Time.deltaTime * Vector3.left;
			if (transform.position.x < leftEdge) { 
				Destroy(gameObject);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(isFalling) { // on vérifie la collision que si la graine tombe
			if (other.CompareTag("Obstacle")) { // si la graine touche un obstacle, elle est détruite 
				Destroy(gameObject);
			} else if (other.CompareTag("BabyBird")) {  // si graine touche un oisillon, elle est détruite et raport des points au joueur
				FindObjectOfType<GameManager>().IncreaseScore(defaultPoint);
				other.GetComponent<BabyBird>().isFed = true;
				Destroy(gameObject);
			}
		}
	}
}
