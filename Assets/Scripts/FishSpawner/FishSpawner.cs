using UnityEngine;
using System.Collections;

public class FishSpawner : MonoBehaviour
{
    [Header("RangeTimer")]
    public float minTimeToWait;
    public float maxTimeToWait;

    [SerializeField] private GameObject[] _fishesToSpawn;

    private Collider2D _spawnerCollider;
    public void Awake()
    {
        _spawnerCollider = GetComponent<Collider2D>();
    }

    public void Start()
    {
        StartCoroutine("SpawnFishes");
    }

    IEnumerator SpawnFishes()
    {
        while (true)
        {
            FishSpawnerManager.instance.SpawnFish(_spawnerCollider, _fishesToSpawn);
            yield return new WaitForSeconds(Random.Range(minTimeToWait, maxTimeToWait));
        }
    }


}
