using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // l'objet SpriteRenderer permettant de mettre � jour le sprite pour g�rer l'animation

    public Sprite[] sprites; // liste des sprites pour g�rer l'animation

    private int spriteIndex; // l'indi

    private Vector3 direction; // permet de g�rer la direction de l'oiseau au moment du clique 

    public float gravity = -25f; // permet de g�rer la vitesse de la chute

    public float strength = 8f; // permet de g�rer � quel point l'oiseau remonte au moment du clique

    private int seedsNumber; // le nombre de graines tenues par le joueur

    private readonly float timeBetweenShots = 1f; // permettre g�rer le temps entre les l�cher de graines

    private float lastShotTime; // permet d'avoir le dernier temps o� la derni�re graine a �t� l�ch�e

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
        seedsNumber = 0;
    }
    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f); // permet d'invoquer � un interval de temps l'animation des sprites
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) // permet de faire remonter le joueur au moment du clic gauche/ de l'appuie sur la barre espace
        {
            direction = Vector3.up * strength;
        }

        if (Input.GetMouseButtonDown(1) && seedsNumber != 0)
        { // lors du clique droit
            if (Time.time - lastShotTime >= timeBetweenShots)
            { // si le d�lai entre le dernier l�cher de graine et le moment du clique est assez grand, le joueur peut l�cher la graine
                FindObjectOfType<Spawner>().SpawnFallingSeed(transform); // faire appara�tre une nouvelle graine � la position du joueur au moment o� la graine est l�ch�
                seedsNumber--;
                FindObjectOfType<GameManager>().SetSeedsNumber(seedsNumber); // mettre � jour l'affichage du nombre de graines
                lastShotTime = Time.time; // mettre � jour la derni�re fois o� le joueur � l�cher une graine
            }
        }
        // faire tomber le joueur
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    public bool HoldSeeds()
    { // permet de v�rifier si le joueur tiens au moins une graine
        return seedsNumber > 0;
    }

    private void AnimateSprite() // permet de changer l'affichage du sprite afin de permettre l'animation de l'oiseau
    {
        spriteIndex++;
        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("EnemyBird")) // si le joueur touche un obstacle ou un ennemi, c'est game over
        {
            FindObjectOfType<GameManager>().GameOver();
        }
        else if (other.CompareTag("Scoring")) // si le joueur passe entre deux tuyaux oppos�s, il gagne un point
        {
            FindObjectOfType<GameManager>().IncreaseScore(1);
        }
        else if (other.CompareTag("Seed"))
        {
            if (!other.gameObject.GetComponent<Graines>().isFalling)
            { // si le joueur touche une graine qui ne tombe pas, il la garde
                seedsNumber++;
                FindObjectOfType<GameManager>().SetSeedsNumber(seedsNumber); // on met � jour l'affichage du nombre de graines
                Destroy(other.gameObject); // la graine sera d�truite
            }
        }
    }
}
