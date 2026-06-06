using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    public GameObject[] gunPrefabs;   // različne puške, ki jih lahko spawnamo
    public int gunCount = 5;          // koliko pušk spawnati
    public float spawnRange = 20f;    // razpon spawnanja okoli spawnerja
    public float spawnHeight = 1f;    // višina nad tlemi

void Start()
{
    if (gunPrefabs.Length == 0)
    {
        Debug.LogError("GunSpawner: Ni nobenega prefab-a v gunPrefabs array!");
        return;
    }

for (int i = 0; i < gunCount; i++)
{
    Vector3 randomPos = new Vector3(
        Random.Range(-spawnRange, spawnRange),
        spawnHeight,
        Random.Range(-spawnRange, spawnRange)
    );

    int index = Random.Range(0, gunPrefabs.Length);
    GameObject spawnedGun = Instantiate(gunPrefabs[index], randomPos, Quaternion.identity);
spawnedGun.SetActive(true); // da se vidi

}

}

}
