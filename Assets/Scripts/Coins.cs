using UnityEngine;

public class Coins : MonoBehaviour
{
	public static int score; // le nombre de pièces en début de parti
	
    private static float speed = 5f; // la vitesse de déplacement de la pièce

	private float leftEdge; // la gauche de l'écran

	public AudioClip coinAudio; // audio à jouer quand le joueur touche une pièce

	public static void SetDifficulty(int difficulty) // cette fonction permet de changer la vitesse de la pièce en fonction du niveau de difficulté ou bien de la laisser par défaut
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

	public static void SaveScore() { //permet de sauvegarder le nombre de pièces après mise à jour
		PlayerPrefs.SetInt("COINS", score);
		PlayerPrefs.Save();
	}

	public static void InitScore() { // permet d'initialiser par défaut le nombre de pièces ou bien de récupérer le nombre de pièce sauvegardé
		score = 1000;
		if(PlayerPrefs.HasKey("COINS")) {
			score = PlayerPrefs.GetInt("COINS");
		}
	}

	private void Start()
	{
		leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f; // calcul la position la plus à gauche de l'écran
	}

    void Update()
    {
		transform.position += speed * Time.deltaTime * Vector3.left; // déplacer la pièce pour donner l'impression d'avancer

		if (transform.position.x < leftEdge) // on cherche à détruire la pièce une fois qu'elle disparaît de l'écran
		{
			Destroy(gameObject);
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // vérifier que le joueur à toucher la pièce
		{
			score++; // le nombre de pièces est incrémenter
			FindObjectOfType<GameManager>().UpdateCoins(); // le nombre de pièce est mis à jour sur l'interface
			AudioSource.PlayClipAtPoint(coinAudio, transform.position); // l'audio de la pièce est joué
			Destroy(gameObject); // la pièce doit être détruite
        }
    }
}
