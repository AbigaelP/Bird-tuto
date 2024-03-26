using UnityEngine;

public class BabyBird : MonoBehaviour
{
    private float bottomEdge; // le bas de l'écran

    private readonly float speed = 2f; //la vitesse de déplacement de l'oisillon quand il descend dans son nid

    public bool isFed = false; //indique si l'oisillon a été nourri avec une graine

    private void Awake()
    {
        bottomEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).y - 1f; // calcul la position du bas de l'écran
    }

    private void Update() {
        if(isFed) { // vérifier que l'oisillon a été nourri avec une graine
            transform.position += speed * Time.deltaTime * Vector3.down; // faire descendre l'oisillon dans son nid
        }
        if (transform.position.y < bottomEdge) // on cherche à faire descendre l'oisillon pour qu'il quitte l'écran et le supprimer
        {
            Destroy(gameObject);
        }
    }
}
