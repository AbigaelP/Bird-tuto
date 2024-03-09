using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static bool spawnOisillon = false;

    public static bool randomSpawning = false;

    public GameObject pipe;

    public GameObject coin;

    public float minHeight = -1f;

    public float maxHeight = 1f;

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
        Invoke(nameof(SpawnPipes), 2f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(SpawnPipes));
        CancelInvoke(nameof(SpawnCoins));
    }
    private void SpawnPipes()
    {
        //calculer la distance
        GameObject pipes = Instantiate(pipe, transform.position, Quaternion.identity);

        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        pipes.transform.Find("Top").localPosition += Vector3.up * Random.Range(4.5f, 7f);
        pipes.transform.Find("Bottom").localPosition += Vector3.up * Random.Range(-5.5f, -4.5f);



        float nextPipes = 1f;
        float nextCoin = 2;

        if(randomSpawning)
        {
            nextPipes = Random.Range(0.5f, 3f);
            nextCoin = Random.Range(1.2f, 3f);
        }

        Invoke(nameof(SpawnPipes), nextPipes);
        Invoke(nameof(SpawnCoins), nextPipes/nextCoin);

    }

    private void SpawnCoins()
    {
        GameObject coins = Instantiate(coin, transform.position, Quaternion.identity);

        coins.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        coins.transform.localPosition += Vector3.up * Random.Range(-2.5f, 3f);
    }
}
