using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite[] sprites;

    private int spriteIndex;

    private Vector3 direction;

    private float gravity = -25f;

    private float strength = 8f;

    private int scoreSeed = 0;

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
        scoreSeed = 0;
    }
    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
        }

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                direction = Vector3.up * strength;
            }
        }

        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    private void AnimateSprite()
    {
        spriteIndex++;
        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }
        
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D c2d)
    {
        if (c2d.gameObject.tag == "Obstacle")
        {
            FindObjectOfType<GameManager>().GameOver();
        }else if (c2d.gameObject.tag == "Scoring")
        {
            FindObjectOfType<GameManager>().IncreaseScore(1);
        }
        else if (c2d.gameObject.tag == "Seed")
        {
            scoreSeed ++;
            FindObjectOfType<GameManager>().IncreaseScoreSeed(scoreSeed);

            Destroy(c2d.gameObject);
            
        }
    }
}
