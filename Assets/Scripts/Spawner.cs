using UnityEngine;

public class Spawner : MonoBehaviour
{
	private bool spawnBabyBird; // indique si les oiseaux peuvent apparaître
	private bool spawnEnemyBird; // indique si les oiseaux ennemis peuvent apparaître
	private bool spawnCat; // indique si les chats peuvent apparaîtr
	private bool coinSpawnEveryPipe; // indique si il y au une pièce après chaque pipe ou non
	private float minimumPipeSpawn; // la distance minimal entre les pipes du haut et du bas
	private float maximumPipeSpawn; // la distance maximal entre les pipes du haut et du bas

    public GameObject pipe; // template des pipes

	public GameObject coin; // template des pièces

	public GameObject seed; // template des graines

	public GameObject enemyBird; // template des oiseaux ennemis

	public GameObject cat; // template des chats

    private readonly int minHeight = -1; //la hauteur minimal d'apparition des objets
    private readonly int maxHeight = 2; // la hauteur maximal d'apparition des objets

	private int counter; // compteur pour faire apparaître des objets tous les n tuyaux
	private int nextSeedA; //indique après combien de tuyaux apparaître la prochaine graine
	private int countSpawnCoin; // utiliser pour faire apparaître les pièces une fois sur deux

	public void SetDifficulty() // permet d'indiquer la difficulté et de choisir quels objets apparaissent et à quelle fréquence
	{
		spawnBabyBird = false;
		spawnEnemyBird = false;
		spawnCat = false;
		minimumPipeSpawn = 1f;
		maximumPipeSpawn = 2f;
		coinSpawnEveryPipe = true;
		switch (FindObjectOfType<GameManager>().DifficultyLevel)
		{
			case 2:
                maximumPipeSpawn = 1f;
				coinSpawnEveryPipe = false;
				break;
			case 5:
			case 6:
				spawnBabyBird = true;
				break;
			case 7:
				maximumPipeSpawn = 2f;
				break;
			case 8:
				spawnEnemyBird = true;
				break;
			case 9:
				spawnEnemyBird = true;
				spawnCat = true;
				maximumPipeSpawn = 3f;

                break;
		}
	}

    private void OnEnable()
	{
		SetDifficulty(); // active la difficulté du jeu
		counter = 0;
		countSpawnCoin = 0;
		nextSeedA = Random.Range(1, 2);
		Invoke(nameof(SpawnPipes), 2); // fait apparaître les premiers tuyaux au bout de 2 secondes
		if(spawnEnemyBird) {
			Invoke(nameof(SpawnEnemyBird), 5f); // fait apparaître le premier oiseau ennemi au bout de 5 secondes
		}
	}

    private void OnDisable() // annuler toutes les invocations qui attendent
    {
        CancelInvoke(nameof(SpawnPipes));
		CancelInvoke(nameof(SpawnCoins));
		CancelInvoke(nameof(SpawnSeeds));
		CancelInvoke(nameof(SpawnEnemyBird));
		CancelInvoke(nameof(SpawnCat));
	}

    private void SpawnPipes() // gère l'affichage des tuyaux et de tout ce qui apparaît après un tuyau 
    {

		
		GameObject pipes = Instantiate(pipe, transform.position, Quaternion.identity);
		float moveTop =  Random.Range(5f, 7f); // récupérer de combier sera déplacement le tuyau du haut
		float moveBottom =  Random.Range(3f, 5f); // récupérer de combier sera déplacement le tuyau du bas
		pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
		
		//déplacement de chaque tuyau et mise à jour du skin
		pipes.transform.Find("Top").localPosition += Vector3.up * moveTop;
		SetPipeColor(pipes.transform.Find("Top"));
		
		pipes.transform.Find("Bottom").localPosition += Vector3.down * moveBottom;
		SetPipeColor(pipes.transform.Find("Bottom"));

		//suppression ou non des tuyaux des côtés de chaque tuyaux
		DeleteSidePipe(pipes.transform.Find("Top").Find("TopLeftSide").gameObject);
		DeleteSidePipe(pipes.transform.Find("Top").Find("TopRightSide").gameObject);
		DeleteSidePipe(pipes.transform.Find("Bottom").Find("BottomLeftSide").gameObject);
		DeleteSidePipe(pipes.transform.Find("Bottom").Find("BottomRightSide").gameObject);


		// récupérer l'oisillon et son nid
		Transform babyBirdTransform = pipes.transform.Find("BabyBird");
		Transform nestTransform = pipes.transform.Find("Nest");
		
		// récupérer d'attente pour le prochain tuyau et la prochaine pièce
		float nextPipes = Random.Range(minimumPipeSpawn, maximumPipeSpawn);
		float nextCoin = Random.Range(1.5f, 4f);

		if(spawnBabyBird)
		{
			counter++;
			if(counter >= nextSeedA) { // l'apparition de la graine est gérée ici pour l'oiseau 
				counter = 0;
				nextSeedA = Random.Range(4, 8); // on génère le nombre de tuyaux avant la graine d'après
				float nextSeed = Random.Range(1.5f, 4f);
				Invoke(nameof(SpawnSeeds), nextPipes/nextSeed); // la graine apparaît entr le tuyau actuel et le pochain tuyau
			}
			
			if(FindObjectOfType<Player>().HoldSeeds()) { 
				if(Random.Range(1,4) > 1) {
					Destroy(babyBirdTransform.gameObject);
					Destroy(nestTransform.gameObject);

				} else { // le nid et l'oisillon sont déplacés pour rester au dessus du tuyau
					babyBirdTransform.localPosition += Vector3.down*moveBottom;
					nestTransform.localPosition += Vector3.down*moveBottom;
				}
			} else { // l'oisillon et le nid sont détruit si le joueur ne possède pas de graine
				Destroy(babyBirdTransform.gameObject);
				Destroy(nestTransform.gameObject);
			}
		} else { // l'oisillon et le nid sont détruit si le niveau de possède pas d'oisillon
			Destroy(babyBirdTransform.gameObject);
			Destroy(nestTransform.gameObject);
		}

		if(coinSpawnEveryPipe) { // si la pièce apparaît à tout moment, on la fait apparaître à tout moment entre deux tuyaux
			Invoke(nameof(SpawnCoins), nextPipes/nextCoin);

		} else { // sinon, la pièce apparaît tous les deux tuyaux
			counter ++;
			if(countSpawnCoin == 2) {
				counter = 0; 
				Invoke(nameof(SpawnCoins), nextPipes/nextCoin);
			}
		}

		
		if(spawnCat) {
			counter++;
			if(counter >= nextSeedA) { // l'apparition de la graine est gérée ici pour le chat 
				counter = 0;
				nextSeedA = Random.Range(4, 8); // on génère le nombre de tuyaux avant la graine d'après
				float nextSeed = Random.Range(1.5f, 4f);
				Invoke(nameof(SpawnSeeds), nextPipes/nextSeed);
			}
			if(Random.Range(1,4) > 1) {
				Invoke(nameof(SpawnCat),nextPipes/2); // le chat apparaît entre deux pipes à tout moment sans prendre en compte si oui ou non le joueur a une graine
			}
		}

		Invoke(nameof(SpawnPipes), nextPipes); // on prévoit l'apparition des prochains tuyaux
	}


	private void DeleteSidePipe(GameObject pipe) { // cette fonction permet de supprimer les tuyaux sur le côté à partir d'une probabilité
		if(Random.Range(1,5) > 1) {
			Destroy(pipe);
		} else { // si le tuyaux n'est pas supprimé, on met à jour son skin
			SetPipeColor(pipe.transform);
		}
	}

	private void SetPipeColor(Transform pipe) { // cette fonction met à jour les skins de chaque tuyau
		pipe.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<SkinManager>().pipe.sprite;
	}

	private void SpawnCoins() // cette fonction permet de faire apparaître les pièces
	{
		GameObject coins = Instantiate(coin, transform.position, Quaternion.identity);
		// on ajoute un déplacement min/max + un déplacement aléatoire
		coins.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
		coins.transform.localPosition += Vector3.up * Random.Range(-3, 3);
	}
	
	private void SpawnSeeds() // cette fonction permet de faire apparaître les graines
	{
		GameObject seeds = Instantiate(seed, transform.position, Quaternion.identity);
		// on ajoute un déplacement min/max + un déplacement aléatoire
		seeds.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
		seeds.transform.localPosition += Vector3.up * Random.Range(-3, 3);
	}

	private void SpawnEnemyBird() { // cette fonction fait apparaître les oiseaux ennemis et fait réapparaître le prochain au bout de tois secondes
		GameObject enemy = Instantiate(enemyBird, transform.position, Quaternion.identity);
		enemy.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
		Invoke(nameof(SpawnEnemyBird), 3);
	}

	public void SpawnFallingSeed(Transform position) { // cette fonction fait apparaître les graines qui tombent depuis la position du joueur
		GameObject fallingSeed = Instantiate(seed, position.position, Quaternion.identity);
		fallingSeed.GetComponent<Graines>().isFalling = true;
	}

	public void SpawnCat() { // cette fonction fait apparaître les chats et les positionne sur le sol
		GameObject cats = Instantiate(cat, transform.position, Quaternion.identity);
		cats.transform.position += Vector3.down * 3.3f;
	}
}
