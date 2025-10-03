using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float movementLenght;
    [SerializeField] Transform marginLeft;
    [SerializeField] Transform marginBottom;

    private Rigidbody2D _rb;

    private bool _isLanded = false;
    private bool _gravitySet = false;
    private bool _blockUpMovement = false;
    private bool _blockDownMovement = false;
    private bool inTheSea = false;
    private bool returning = false;

    private float _fishStrenght;

    private Vector3 _contactPointReturn;
    private Vector3 _startPosition;

    public static event Action onFishOutOfWater;

    public Vector3 GetContactPointReturn() {  return _contactPointReturn; }
    public bool IsInTheSea() { return inTheSea; }

    public void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPosition = new Vector2 (this.gameObject.transform.position.x, this.gameObject.transform.position.y);
    }
    public void Update()
    {
        if (_isLanded)
        {
            SetGravityZero();
            HookMovement(); 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isLanded)
        {
            if (collision.gameObject.tag == "Sea" && !returning)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _isLanded = true;
                inTheSea = true;
                _contactPointReturn = collision.ClosestPoint(this.gameObject.transform.position);
            }
        }
        
        if(collision.gameObject.tag == "Fish") 
        {
            _fishStrenght = collision.GetComponent<FishMovement>()._strength;
            inTheSea = false;
            returning = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Collider2D collider = collision.GetComponent<Collider2D>();

        if (collider.name == "Sea")
        {
            Vector3 contactPoint = collision.ClosestPoint(this.gameObject.transform.position);
            Vector3 center = collider.bounds.center;

            _blockDownMovement = contactPoint.y < center.y;
            _blockUpMovement = contactPoint.y > center.y;
        }
    }

    public void HookMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !_blockUpMovement && !FishingPointsManager.instance.stop && transform.position.y < _contactPointReturn.y)
        {
            this.gameObject.transform.position += new Vector3(0, movementLenght,0);
        }
        if (Input.GetKey(KeyCode.RightArrow) && !_blockUpMovement && !FishingPointsManager.instance.stop && transform.position.x < _startPosition.x)
        {
            _contactPointReturn += new Vector3((movementLenght / 2), 0, 0);
            gameObject.transform.position += new Vector3((movementLenght / 2), 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow) && !_blockDownMovement && !FishingPointsManager.instance.stop && transform.position.y > marginBottom.position.y && transform.position.x > marginLeft.position.x)
        {
            this.gameObject.transform.position -= new Vector3(0, movementLenght, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && !_blockUpMovement && !FishingPointsManager.instance.stop)
        {
            _contactPointReturn += new Vector3(-(movementLenght / 2), 0, 0);
            gameObject.transform.position += new Vector3(-(movementLenght / 2), 0, 0);
        }
    }

    public void SetGravityZero()
    {
        if (!_gravitySet)
        {
            _rb.gravityScale = 0;
            _rb.constraints = RigidbodyConstraints2D.None;
            _gravitySet = true;
        }
    }

    public void HookCaughtFish()
    {
        _blockUpMovement = true;
        _blockDownMovement = true;

        StartCoroutine("MoveFishUpward");
    }

    IEnumerator MoveFishUpward()
    {
        while (transform.position != _startPosition)
        {
            if (FishingPointsManager.instance.stop)
            {
                break;
            }
            transform.position = Vector3.MoveTowards(
                transform.position,
                _startPosition,
                movementLenght * (float)2
            );
            yield return new WaitForSeconds(_fishStrenght / 100f);
        }
        Restart();
    }

    public void Restart()
    {
        if (this.gameObject.transform.position.y >= _contactPointReturn.y)
        {
            onFishOutOfWater?.Invoke();
            transform.position = new Vector2 (_startPosition.x, _startPosition.y);
            _isLanded = false;
            _gravitySet = false;
            _blockUpMovement = false;
            _blockDownMovement = false;
            returning = false;
        }
    }

    public void OnEnable()
    {
        FishMovement.isFishCaught += HookCaughtFish;
    }

    public void OnDisable()
    {
        FishMovement.isFishCaught -= HookCaughtFish;
    }

}
