using UnityEngine;

public class FishSpawnerManager : MonoBehaviour
{
    [SerializeField] public Collider2D rightSpawner;
    [SerializeField] public Collider2D leftSpawner;

    public static FishSpawnerManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        Debug.Log(rightSpawner.name );
        Debug.Log(leftSpawner.name);

    }

    public void SpawnFish(Collider2D spawnableAreaCollider, GameObject[] fishes)
    {
        foreach (GameObject fish in fishes)
        {
            Vector2 spawnPosition = GetRandomSpawnPosition(spawnableAreaCollider);
            GameObject spawnedFish = Instantiate(fish, spawnPosition, Quaternion.identity);
        }
    }

    private Vector2 GetRandomSpawnPosition(Collider2D spawnableAreaCollider)
    {
        Vector2 spawnPosition = Vector2.zero;
        bool isSpawnPosValid = false;

        int attemptCount = 0;
        int maxAttempts = 200;

        int layerToNotSpawnOn = LayerMask.NameToLayer("OutOfBoundary");

        while (!isSpawnPosValid && attemptCount < maxAttempts)
        {
            spawnPosition =GetRandomPointInCollider(spawnableAreaCollider);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 2f);

            bool isInvalidCollision = false;

            foreach (Collider2D collider in colliders)
            {
                if(collider.gameObject.layer == layerToNotSpawnOn) 
                {
                    isInvalidCollision = true;
                    break;
                }
            }

            if (!isInvalidCollision) 
            {
                isSpawnPosValid = true; 
            }

            attemptCount++;
        }

        if (!isSpawnPosValid) 
        {
            Debug.LogWarning("Error");
        }

        return spawnPosition;
    }

    public Vector2 GetRandomPointInCollider(Collider2D collider, float offset = 1f)
    {
         Bounds collBounds = collider.bounds;

        Vector2 minBounds = new Vector2(collBounds.center.x, collBounds.min.y + offset);
        Vector2 maxBounds = new Vector2(collBounds.center.x, collBounds.max.y - offset);

        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomY = Random.Range(minBounds.y, maxBounds.y);

        return new Vector2(randomX, randomY);
    }

}
