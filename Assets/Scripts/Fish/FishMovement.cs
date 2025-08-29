using UnityEngine;

public class FishMovement : MonoBehaviour
{
    private Transform _fishPos;
    private Collider2D _spawnerCollider;
    private FishSpawnerManager _spawnerColliderDirection;
    private SpriteRenderer _fishSprite;

    private float _elapsedTime;
    private float _duration = 5f;




    private Vector2 _fishDestination;


    void Start()
    {
        _fishPos = GetComponent<Transform>();
        _fishSprite = GetComponent<SpriteRenderer>();

        if (_fishPos.position.x > Camera.main.transform.position.x)
        {
            _fishSprite.flipX = true;
            _spawnerCollider = GetComponent<FishSpawnerManager>().rightSpawner;
        }

        if(_fishPos.position.x < Camera.main.transform.position.x) 
        {
            _spawnerCollider = GetComponent<FishSpawnerManager>().leftSpawner;
        }

        _fishDestination = FishSpawnerManager.instance.GetRandomPointInCollider(_spawnerCollider, 1f);

    }


    void Update()
    {
        _elapsedTime += Time.deltaTime;
        float percentage = _elapsedTime / _duration;

        transform.position = Vector2.Lerp(_fishPos.position, _fishDestination, percentage);
    }
}
