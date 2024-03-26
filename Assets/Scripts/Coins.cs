using UnityEngine;

public class Coins : MonoBehaviour
{
    public static int score; // le nombre de pi�ces en d�but de parti

    private static float speed = 5f; // la vitesse de d�placement de la pi�ce

    private float leftEdge; // la gauche de l'�cran

    public AudioClip coinAudio; // audio � jouer quand le joueur touche une pi�ce

    public static void SetDifficulty(int difficulty) // cette fonction permet de changer la vitesse de la pi�ce en fonction du niveau de difficult� ou bien de la laisser par d�faut
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

    public static void SaveScore()
    { //permet de sauvegarder le nombre de pi�ces apr�s mise � jour
        PlayerPrefs.SetInt("COINS", score);
        PlayerPrefs.Save();
    }

    public static void InitScore()
    { // permet d'initialiser par d�faut le nombre de pi�ces ou bien de r�cup�rer le nombre de pi�ce sauvegard�
        score = 1000;
        if (PlayerPrefs.HasKey("COINS"))
        {
            score = PlayerPrefs.GetInt("COINS");
        }
    }

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f; // calcul la position la plus � gauche de l'�cran
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.left; // d�placer la pi�ce pour donner l'impression d'avancer

        if (transform.position.x < leftEdge) // on cherche � d�truire la pi�ce une fois qu'elle dispara�t de l'�cran
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // v�rifier que le joueur � toucher la pi�ce
        {
            score++; // le nombre de pi�ces est incr�menter
            FindObjectOfType<GameManager>().UpdateCoins(); // le nombre de pi�ce est mis � jour sur l'interface
            AudioSource.PlayClipAtPoint(coinAudio, transform.position); // l'audio de la pi�ce est jou�
            Destroy(gameObject); // la pi�ce doit �tre d�truite
        }
    }
}
