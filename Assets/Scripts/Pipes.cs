using UnityEngine;

public class Pipes : MonoBehaviour
{
    private readonly float speed = 5f; // la vitesse de déplacement des pipes


    private float leftEdge; // la gauche de l'écran

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x -5f; // calcul la position la plus à gauche de l'écran
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.left; // déplacer la pièce pour donner l'impression d'avancer
        
        if (transform.position.x < leftEdge) // on cherche à détruire la pièce une fois qu'elle disparaît de l'écran
        {
            Destroy(gameObject);
        }
    }
}
