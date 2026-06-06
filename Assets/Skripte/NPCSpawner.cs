using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabs;
    public int npcCount = 10;
    public float spawnRange = 20f;

    void Start()
    {
        for (int i = 0; i < npcCount; i++)
        {
            Vector3 randomPos = transform.position + new Vector3(
                Random.Range(-spawnRange, spawnRange),
                1f,
                Random.Range(-spawnRange, spawnRange)
            );

            int index = Random.Range(0, npcPrefabs.Length);
            Instantiate(npcPrefabs[index], randomPos, Quaternion.identity);
        }
    }
}
