using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _fishesToSpawn;

    private Collider2D _spawnerCollider;
    public void Awake()
    {
        _spawnerCollider = GetComponent<Collider2D>();

    }
    public void Start()
    {
        FishSpawnerManager.instance.SpawnFish(_spawnerCollider, _fishesToSpawn);
    }
}
