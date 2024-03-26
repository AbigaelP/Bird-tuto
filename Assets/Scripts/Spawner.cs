using UnityEngine;

public class Spawner : MonoBehaviour
{
    private bool spawnBabyBird; // indique si les oiseaux peuvent appara�tre
    private bool spawnEnemyBird; // indique si les oiseaux ennemis peuvent appara�tre
    private bool spawnCat; // indique si les chats peuvent appara�tr
    private bool coinSpawnEveryPipe; // indique si il y au une pi�ce apr�s chaque pipe ou non
    private float minimumPipeSpawn; // la distance minimal entre les pipes du haut et du bas
    private float maximumPipeSpawn; // la distance maximal entre les pipes du haut et du bas

    public GameObject pipe; // template des pipes

    public GameObject coin; // template des pi�ces

    public GameObject seed; // template des graines

    public GameObject enemyBird; // template des oiseaux ennemis

    public GameObject cat; // template des chats

    private readonly int minHeight = -1; //la hauteur minimal d'apparition des objets
    private readonly int maxHeight = 2; // la hauteur maximal d'apparition des objets

    private int counter; // compteur pour faire appara�tre des objets tous les n tuyaux
    private int nextSeedA; //indique apr�s combien de tuyaux appara�tre la prochaine graine
    private int countSpawnCoin; // utiliser pour faire appara�tre les pi�ces une fois sur deux

    public void SetDifficulty() // permet d'indiquer la difficult� et de choisir quels objets apparaissent et � quelle fr�quence
    {
        spawnBabyBird = false;
        spawnEnemyBird = false;
        spawnCat = false;
        minimumPipeSpawn = 1f;
        maximumPipeSpawn = 3f;
        coinSpawnEveryPipe = true;
        switch (FindObjectOfType<GameManager>().DifficultyLevel)
        {
            case 2:
                maximumPipeSpawn = 2f;
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
                break;
        }
    }

    private void OnEnable()
    {
        SetDifficulty(); // active la difficult� du jeu
        counter = 0;
        countSpawnCoin = 0;
        nextSeedA = Random.Range(1, 2);
        Invoke(nameof(SpawnPipes), 2); // fait appara�tre les premiers tuyaux au bout de 2 secondes
        if (spawnEnemyBird)
        {
            Invoke(nameof(SpawnEnemyBird), 5f); // fait appara�tre le premier oiseau ennemi au bout de 5 secondes
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

    private void SpawnPipes() // g�re l'affichage des tuyaux et de tout ce qui appara�t apr�s un tuyau 
    {


        GameObject pipes = Instantiate(pipe, transform.position, Quaternion.identity);
        float moveTop = Random.Range(5f, 7f); // r�cup�rer de combier sera d�placement le tuyau du haut
        float moveBottom = Random.Range(3f, 5f); // r�cup�rer de combier sera d�placement le tuyau du bas
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);

        //d�placement de chaque tuyau et mise � jour du skin
        pipes.transform.Find("Top").localPosition += Vector3.up * moveTop;
        SetPipeColor(pipes.transform.Find("Top"));

        pipes.transform.Find("Bottom").localPosition += Vector3.down * moveBottom;
        SetPipeColor(pipes.transform.Find("Bottom"));

        //suppression ou non des tuyaux des c�t�s de chaque tuyaux
        DeleteSidePipe(pipes.transform.Find("Top").Find("TopLeftSide").gameObject);
        DeleteSidePipe(pipes.transform.Find("Top").Find("TopRightSide").gameObject);
        DeleteSidePipe(pipes.transform.Find("Bottom").Find("BottomLeftSide").gameObject);
        DeleteSidePipe(pipes.transform.Find("Bottom").Find("BottomRightSide").gameObject);


        // r�cup�rer l'oisillon et son nid
        Transform babyBirdTransform = pipes.transform.Find("BabyBird");
        Transform nestTransform = pipes.transform.Find("Nest");

        // r�cup�rer d'attente pour le prochain tuyau et la prochaine pi�ce
        float nextPipes = Random.Range(minimumPipeSpawn, maximumPipeSpawn);
        float nextCoin = Random.Range(1.5f, 4f);

        if (spawnBabyBird)
        {
            counter++;
            if (counter >= nextSeedA)
            { // l'apparition de la graine est g�r�e ici pour l'oiseau 
                counter = 0;
                nextSeedA = Random.Range(4, 8); // on g�n�re le nombre de tuyaux avant la graine d'apr�s
                float nextSeed = Random.Range(1.5f, 4f);
                Invoke(nameof(SpawnSeeds), nextPipes / nextSeed); // la graine appara�t entr le tuyau actuel et le pochain tuyau
            }

            if (FindObjectOfType<Player>().HoldSeeds())
            {
                if (Random.Range(1, 4) > 1)
                {
                    Destroy(babyBirdTransform.gameObject);
                    Destroy(nestTransform.gameObject);

                }
                else
                { // le nid et l'oisillon sont d�plac�s pour rester au dessus du tuyau
                    babyBirdTransform.localPosition += Vector3.down * moveBottom;
                    nestTransform.localPosition += Vector3.down * moveBottom;
                }
            }
            else
            { // l'oisillon et le nid sont d�truit si le joueur ne poss�de pas de graine
                Destroy(babyBirdTransform.gameObject);
                Destroy(nestTransform.gameObject);
            }
        }
        else
        { // l'oisillon et le nid sont d�truit si le niveau de poss�de pas d'oisillon
            Destroy(babyBirdTransform.gameObject);
            Destroy(nestTransform.gameObject);
        }

        if (coinSpawnEveryPipe)
        { // si la pi�ce appara�t � tout moment, on la fait appara�tre � tout moment entre deux tuyaux
            Invoke(nameof(SpawnCoins), nextPipes / nextCoin);

        }
        else
        { // sinon, la pi�ce appara�t tous les deux tuyaux
            counter++;
            if (countSpawnCoin == 2)
            {
                counter = 0;
                Invoke(nameof(SpawnCoins), nextPipes / nextCoin);
            }
        }


        if (spawnCat)
        {
            counter++;
            if (counter >= nextSeedA)
            { // l'apparition de la graine est g�r�e ici pour le chat 
                counter = 0;
                nextSeedA = Random.Range(4, 8); // on g�n�re le nombre de tuyaux avant la graine d'apr�s
                float nextSeed = Random.Range(1.5f, 4f);
                Invoke(nameof(SpawnSeeds), nextPipes / nextSeed);
            }
            if (Random.Range(1, 4) > 1)
            {
                Invoke(nameof(SpawnCat), nextPipes / 2); // le chat appara�t entre deux pipes � tout moment sans prendre en compte si oui ou non le joueur a une graine
            }
        }

        Invoke(nameof(SpawnPipes), nextPipes); // on pr�voit l'apparition des prochains tuyaux
    }


    private void DeleteSidePipe(GameObject pipe)
    { // cette fonction permet de supprimer les tuyaux sur le c�t� � partir d'une probabilit�
        if (Random.Range(1, 5) > 1)
        {
            Destroy(pipe);
        }
        else
        { // si le tuyaux n'est pas supprim�, on met � jour son skin
            SetPipeColor(pipe.transform);
        }
    }

    private void SetPipeColor(Transform pipe)
    { // cette fonction met � jour les skins de chaque tuyau
        pipe.GetComponent<SpriteRenderer>().sprite = FindObjectOfType<SkinManager>().pipe.sprite;
    }

    private void SpawnCoins() // cette fonction permet de faire appara�tre les pi�ces
    {
        GameObject coins = Instantiate(coin, transform.position, Quaternion.identity);
        // on ajoute un d�placement min/max + un d�placement al�atoire
        coins.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        coins.transform.localPosition += Vector3.up * Random.Range(-3, 3);
    }

    private void SpawnSeeds() // cette fonction permet de faire appara�tre les graines
    {
        GameObject seeds = Instantiate(seed, transform.position, Quaternion.identity);
        // on ajoute un d�placement min/max + un d�placement al�atoire
        seeds.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        seeds.transform.localPosition += Vector3.up * Random.Range(-3, 3);
    }

    private void SpawnEnemyBird()
    { // cette fonction fait appara�tre les oiseaux ennemis et fait r�appara�tre le prochain au bout de tois secondes
        GameObject enemy = Instantiate(enemyBird, transform.position, Quaternion.identity);
        enemy.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        Invoke(nameof(SpawnEnemyBird), 3);
    }

    public void SpawnFallingSeed(Transform position)
    { // cette fonction fait appara�tre les graines qui tombent depuis la position du joueur
        GameObject fallingSeed = Instantiate(seed, position.position, Quaternion.identity);
        fallingSeed.GetComponent<Graines>().isFalling = true;
    }

    public void SpawnCat()
    { // cette fonction fait appara�tre les chats et les positionne sur le sol
        GameObject cats = Instantiate(cat, transform.position, Quaternion.identity);
        cats.transform.position += Vector3.down * 3.3f;
    }
}
