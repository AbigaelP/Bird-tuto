using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // l'objet SpriteRenderer permettant de mettre à jour le sprite pour gérer l'animation

    public Sprite[] sprites; // liste des sprites pour gérer l'animation

    private int spriteIndex; // l'indi

    private Vector3 direction; // permet de gérer la direction de l'oiseau au moment du clique 

    public float gravity = -25f; // permet de gérer la vitesse de la chute

    public float strength = 8f; // permet de gérer à quel point l'oiseau remonte au moment du clique

    private int seedsNumber; // le nombre de graines tenues par le joueur

    private readonly float timeBetweenShots = 1f; // permettre gérer le temps entre les lâcher de graines

    private float lastShotTime; // permet d'avoir le dernier temps où la dernière graine a été lâchée

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
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f); // permet d'invoquer à un interval de temps l'animation des sprites
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
            { // si le délai entre le dernier lâcher de graine et le moment du clique est assez grand, le joueur peut lâcher la graine
                FindObjectOfType<Spawner>().SpawnFallingSeed(transform); // faire apparaître une nouvelle graine à la position du joueur au moment où la graine est lâché
                seedsNumber--;
                FindObjectOfType<GameManager>().SetSeedsNumber(seedsNumber); // mettre à jour l'affichage du nombre de graines
                lastShotTime = Time.time; // mettre à jour la dernière fois où le joueur à lâcher une graine
            }
        }
        // faire tomber le joueur
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    public bool HoldSeeds()
    { // permet de vérifier si le joueur tiens au moins une graine
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
        else if (other.CompareTag("Scoring")) // si le joueur passe entre deux tuyaux opposés, il gagne un point
        {
            FindObjectOfType<GameManager>().IncreaseScore(1);
        }
        else if (other.CompareTag("Seed"))
        {
            if (!other.gameObject.GetComponent<Graines>().isFalling)
            { // si le joueur touche une graine qui ne tombe pas, il la garde
                seedsNumber++;
                FindObjectOfType<GameManager>().SetSeedsNumber(seedsNumber); // on met à jour l'affichage du nombre de graines
                Destroy(other.gameObject); // la graine sera détruite
            }
        }
    }
}
