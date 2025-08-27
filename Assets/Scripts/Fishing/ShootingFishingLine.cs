using Unity.VisualScripting;
using UnityEngine;

public class ShootingFishingLine : MonoBehaviour
{
    [SerializeField]
    [Range(1f,100f)]
    public float power = 1f;

    private Rigidbody2D _rb;

    private LineRenderer _lr;

    [SerializeField] private Transform _releasePos;
    [SerializeField] private Transform _endPosition;

    private Vector3 _endPos;
    private Vector3 _startPos;
    private Vector3 _nextPos;

    public float arcHeight;

    [SerializeField]
    [Range(10, 100)]
    private int linePoints = 25;

    [SerializeField]
    [Range(0.01f, 0.25f)]
    private float timeBetweenPoints = 0.1f;

    private bool _isShot = true;
    private bool clicked = false;



    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.constraints = RigidbodyConstraints2D.FreezePositionY;

        _lr = GetComponent<LineRenderer>();

        _startPos = _releasePos.position;

        _endPos = _endPosition.position;

    }

    void Update()
    {
        if (_isShot)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _endPos.x -= 0.05f;
                Debug.Log(_endPos + "aggiornata");
                Debug.Log(_startPos);
                //DrawProjection();
                

            }

        

            if (Input.GetKeyUp(KeyCode.Space))
            {
                //_isShot = false;
                clicked = true;
            //_rb.constraints = RigidbodyConstraints2D.None;
            }

            if (clicked)
            {
                ReleaseHook();
            }
        }

    }

    void Arrived()
    {
        Debug.Log("AAA");
        //Destroy(gameObject);
    }

    static Quaternion LookAt2D(Vector2 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }

    public void ReleaseHook()
    {
        float x0 = _startPos.x;
        float x1 = _endPos.x;
        float dist = x1 - x0;
        Debug.Log(transform.position.x);
        float nextX = Mathf.MoveTowards(transform.position.x, x1, power * Time.deltaTime);
        float baseY = Mathf.Lerp(_startPos.y, _endPos.y, (nextX - x0) / dist);
        float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
        _nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

        // Rotate to face the next position, and then move there
        transform.rotation = LookAt2D(_nextPos - transform.position);
        transform.position = _nextPos;

        // Do something when we reach the target
        if (_nextPos == _endPos) Arrived();
    }

    Vector3 DrawParabola(Vector3 start, Vector3 end, float height, float t, Vector3 outDirection) // NON FUNZIONA
    {
        float parabolicT = t * 2 - 1;
        //start and end are not level, gets more complicated
        Vector3 travelDirection = end - start;
        Vector3 levelDirection = end - new Vector3(start.x, end.y, start.z);
        Vector3 right = Vector3.Cross(travelDirection, levelDirection);
        Vector3 up = outDirection;
        Vector3 result = start + t * travelDirection;
        result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
        return result;
    }

    public void DrawProjection()
    {
        _lr.startWidth = 0.1f;
        _lr.endWidth = 0.05f;
        _lr.enabled = true;
        _lr.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;   //NON FUNZIONA
        Vector2 startPosition = _releasePos.position;
        Vector2 startVelocity = power * new Vector2(-1, 1) / _rb.mass;     //POSIBILE ERRORE
        int i = 0;
        _lr.SetPosition(i, startPosition);
        for(float time=0; time <linePoints; time += timeBetweenPoints)
        {
            i++;
            Vector2 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            _lr.SetPosition(i, point);
        }

    }
    
}
