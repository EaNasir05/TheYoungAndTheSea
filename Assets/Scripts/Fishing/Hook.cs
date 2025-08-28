using JetBrains.Annotations;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] float movementLenght;

    private Rigidbody2D _rb;
    private Collider2D _collider;

    private bool _isLanded = false;
    private bool _gravitySet = false;
    private bool _blockUpMovement = false;
    private bool _blockDownMovement = false;

    public void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }

    public void Update()
    {
        if (_isLanded)
        {
            SetGravityZero();
            HookMovement();

            if (!_collider.bounds.Contains(this.transform.position))
            {
                Debug.Log(this.transform.position.y);
                Debug.Log(_collider.bounds.size.y);

                if((this.transform.position.y) <= _collider.bounds.size.y)
                {
                    _blockUpMovement = true;
                }
                if((this.transform.position.y) >= _collider.bounds.size.y)
                {
                    _blockDownMovement = true;
                }
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isLanded)
        {
            if (collision.gameObject.tag == "Sea")
            {
                _collider = collision;
                Debug.Log(_collider.gameObject.name);
                Debug.Log("KKK");
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _isLanded = true;
            }

        }
    }


    public void HookMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !_blockUpMovement)
        {
            this.gameObject.transform.position += new Vector3(0, movementLenght,0);
        }

        if (Input.GetKey(KeyCode.DownArrow) && !_blockDownMovement)
        {
            this.gameObject.transform.position -= new Vector3(0, movementLenght, 0);
        }
    }

    public void CheckOutOfBoundaries()
    {
        this.gameObject.transform.position = this.gameObject.transform.position;
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

}
