using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static bool spawnOisillon = false;

    public static bool randomSpawning = false;

    public GameObject pipe;

    public GameObject coin;

    public GameObject seed;

    public float minHeight = -1f;

    public float maxHeight = 1f;

    private int counter = 0;

    private int nextSeedA = 0;

    public static void setDifficulty( int difficulty)
    {
        //default case
        spawnOisillon = false;
        randomSpawning = true;

        switch (difficulty)
        {
            case 2:
                randomSpawning = false;
                break;
            case 5:
            case 6:
                spawnOisillon = true;
                break;
        }
    }

    private void OnEnable()
    {
        counter = 0;
        nextSeedA = Random.Range(4, 8);
        Invoke(nameof(SpawnPipes), 2f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(SpawnPipes));
        CancelInvoke(nameof(SpawnCoins));
        CancelInvoke(nameof(SpawnSeeds));
    }
    private void SpawnPipes()
    {
        //calculer la distance
        GameObject pipes = Instantiate(pipe, transform.position, Quaternion.identity);


        float nextPipes = 1f;
        float nextCoin = 2f;


        if(randomSpawning)
        {
            nextPipes = Random.Range(0.5f, 2f);
            nextCoin = Random.Range(1.2f, 3f);

            pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
            pipes.transform.Find("Top").localPosition += Vector3.up * Random.Range(4.5f, 7f);
            pipes.transform.Find("Bottom").localPosition += Vector3.up * Random.Range(-5.5f, -4.5f);
        }
        else
        {
            pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
            pipes.transform.Find("Top").localPosition += Vector3.up * 4.5f;
            pipes.transform.Find("Bottom").localPosition += Vector3.up * -4.5f;
        }


        if (spawnOisillon)
        {   
            counter++;
            if (counter >= nextSeedA)
            {
                Invoke(nameof(SpawnSeeds), 1);
                counter = 0;
                nextSeedA = Random.Range(4, 8);
            }
            
        }

        Invoke(nameof(SpawnPipes), nextPipes);
        Invoke(nameof(SpawnCoins), nextPipes/nextCoin);

    }

    private void SpawnCoins()
    {
        GameObject coins = Instantiate(coin, transform.position, Quaternion.identity);

         coins.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);

        if (randomSpawning)
        {
         coins.transform.localPosition += Vector3.up * Random.Range(-2.5f, 3f);
        }

    }

    private void SpawnSeeds()
    {
        GameObject seeds = Instantiate(seed, transform.position, Quaternion.identity);

        seeds.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        seeds.transform.localPosition += Vector3.up * Random.Range(-2.5f, 3f);
    }
}
