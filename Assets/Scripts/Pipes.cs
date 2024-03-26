using UnityEngine;

public class Pipes : MonoBehaviour
{
    private readonly float speed = 5f; // la vitesse de d�placement des pipes


    private float leftEdge; // la gauche de l'�cran

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 5f; // calcul la position la plus � gauche de l'�cran
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.left; // d�placer la pi�ce pour donner l'impression d'avancer

        if (transform.position.x < leftEdge) // on cherche � d�truire la pi�ce une fois qu'elle dispara�t de l'�cran
        {
            Destroy(gameObject);
        }
    }
}
