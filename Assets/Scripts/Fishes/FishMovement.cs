using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections.Generic;

public class FishMovement : MonoBehaviour
{
    private Transform _fishPos;
    private SpriteRenderer _fishSprite;

    public static event Action isFishCaught;

    [Header("FishStats")]
    [SerializeField] private float _speed;
    [Range(1f,10f)]
    [SerializeField] public float _strength;

    [Header("Positions Right Spawner")]
    [SerializeField] private float _fishPosXRightSpawner;

    [Header("Positions Left Spawner")]
    [SerializeField] private float _fishPosXLeftSpawner;

    [SerializeField] private float _fishPosMaxYSpawner;
    [SerializeField] private float _fishPosMinYSpawner;

    private Vector2 _fishDestination;
    private GameObject _hook;

    [SerializeField] public List<GameObject> _fishToEliminate = new List<GameObject>();

    private bool _fishCaught = false;
    private static bool _fishCantInteractWithHook = false;
    private bool _fishGoingUP = false;


    void Start()
    {
        _fishPos = GetComponent<Transform>();
        _fishSprite = GetComponent<SpriteRenderer>();

        if (_fishPos.position.x > Camera.main.transform.position.x)
        {
            _fishSprite.flipX = true;
            _fishDestination = new Vector2(_fishPosXLeftSpawner, UnityEngine.Random.Range(_fishPosMinYSpawner, _fishPosMaxYSpawner));
        }
        if(_fishPos.position.x < Camera.main.transform.position.x) 
        {
            _fishDestination = new Vector2(_fishPosXRightSpawner, UnityEngine.Random.Range(_fishPosMinYSpawner, _fishPosMaxYSpawner));
        }
    }
    void Update()
    {
        if (!_fishCaught) 
        {
            transform.position = Vector2.MoveTowards(_fishPos.position, _fishDestination, _speed * Time.deltaTime);  //FORSE QUI INVECE DI LERP MEGLIO SMOOTHDAMP

            if (this.transform.position == new Vector3(_fishDestination.x, _fishDestination.y, 0))
            {
                Destroy(this.gameObject);
            }
        }

        if (_fishGoingUP)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, _hook.transform.position, 0.5f);
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook" && !_fishCantInteractWithHook)
        {
            _fishCaught = true;
            _fishGoingUP = true;
            _fishCantInteractWithHook = true;
            isFishCaught?.Invoke();
            _hook = other.gameObject;
            _fishToEliminate.Add(this.gameObject);
        }
    }

    public void Restart()
    {
        _fishCaught = false;
        _fishCantInteractWithHook = false;
        if(_fishGoingUP)
        {
            _fishGoingUP = false;
            Destroy(_fishToEliminate[0]);
            _fishToEliminate.Clear();
        }
    }

    public void OnEnable()
    {
        Hook.onFishOutOfWater += Restart;
    }

    public void OnDisable()
    {
        Hook.onFishOutOfWater -= Restart;
    }
}
