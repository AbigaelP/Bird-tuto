using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

public class Seeds : MonoBehaviour
{
   
    private static float speed = 5f;

    private float leftEdge;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
    }

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }

}
