using UnityEngine;

public class FishMovement : MonoBehaviour
{
    private Transform _fishPos;
    private SpriteRenderer _fishSprite;

    private float _elapsedTime;
    [SerializeField] private float _speed;

    [Header("Positions Right Spawner")]
    [SerializeField] private float _fishPosXRightSpawner;

    [Header("Positions Left Spawner")]
    [SerializeField] private float _fishPosXLeftSpawner;

    [SerializeField] private float _fishPosMaxYSpawner;
    [SerializeField] private float _fishPosMinYSpawner;

    private Vector2 _velocity = Vector2.zero;

    private Vector2 _fishDestination;


    void Start()
    {
        _fishPos = GetComponent<Transform>();
        _fishSprite = GetComponent<SpriteRenderer>();

        if (_fishPos.position.x > Camera.main.transform.position.x)
        {
            _fishSprite.flipX = true;
            _fishDestination = new Vector2(_fishPosXLeftSpawner, Random.Range(_fishPosMinYSpawner, _fishPosMaxYSpawner));
        }
        if(_fishPos.position.x < Camera.main.transform.position.x) 
        {
            _fishDestination = new Vector2(_fishPosXRightSpawner, Random.Range(_fishPosMinYSpawner, _fishPosMaxYSpawner));
        }
    }


    void Update()
    {
        transform.position = Vector2.MoveTowards(_fishPos.position, _fishDestination, _speed * Time.deltaTime);  //FORSE QUI INVECE DI LERP MEGLIO SMOOTHDAMP

        if (this.transform.position == new Vector3(_fishDestination.x, _fishDestination.y,0))
        {
            Destroy(this.gameObject);
        }
    }
}
