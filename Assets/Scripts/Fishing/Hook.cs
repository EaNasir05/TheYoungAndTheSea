using JetBrains.Annotations;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float movementLenght;

    private Rigidbody2D _rb;
    private Collider2D _hookCollider;

    private bool _isLanded = false;
    private bool _gravitySet = false;
    private bool _blockUpMovement = false;
    private bool _blockDownMovement = false;

    public void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hookCollider = GetComponent<Collider2D>();
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
            if (collision.gameObject.tag == "Sea")
            {
                _hookCollider = collision;
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _isLanded = true;
            }

            if(collision.gameObject.tag == "Fish") 
            {
                HookCaughtFish();
                _hookCollider.enabled = false;
                _blockDownMovement = true;
                _blockUpMovement = true;
            }
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
        if (Input.GetKey(KeyCode.UpArrow) && !_blockUpMovement)
        {
            this.gameObject.transform.position += new Vector3(0, movementLenght,0);
            _blockDownMovement = false;
        }

        if (Input.GetKey(KeyCode.DownArrow) && !_blockDownMovement)
        {
            this.gameObject.transform.position -= new Vector3(0, movementLenght, 0);
            _blockUpMovement = false;
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
        Debug.Log("AA");
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
