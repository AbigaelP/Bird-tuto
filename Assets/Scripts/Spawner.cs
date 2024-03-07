
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    public float minHeight = -1f;

    public float maxHeight = 1f;

    private void OnEnable()
    {
        Invoke(nameof(Spawn), 2);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }
    private void Spawn()
    {
        GameObject pipes = Instantiate(prefab, transform.position, Quaternion.identity);

        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);
        pipes.transform.Find("Top").localPosition += Vector3.up * Random.Range(4.5f, 7);
        pipes.transform.Find("Bottom").localPosition += Vector3.up * Random.Range(-5.5f, -4.5f);

        Invoke(nameof(Spawn), Random.Range(1f,3f));
    }
}
