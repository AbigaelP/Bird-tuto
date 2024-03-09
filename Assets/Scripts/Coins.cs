using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public static int score = 0;
    private static float speed = 5f;

    private float leftEdge;

    public static void setDifficulty (int difficulty)
    {
        speed = 5f;
        switch (difficulty)
        {
            case 3:
            case 6:
                speed = 10f;
                break;
            case 4:
                speed = 1f;
                break;
        }
    }

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    void Awake()
    {
        //Make Collider2D as trigger 
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D c2d)
    {
        //Destroy the coin if Object tagged Player comes in contact with it
        if (c2d.CompareTag("Player"))
        {
            score++;
            //Add coin to counter
            FindObjectOfType<GameManager>().IncreaseScoreCoin(score);
 
            //Destroy coin
            Destroy(gameObject);
        }
    }
}
