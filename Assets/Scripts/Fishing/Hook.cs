using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hook : MonoBehaviour
{
    [SerializeField] float movementLenght;
    [SerializeField] Transform marginLeft;
    [SerializeField] Transform marginBottom;
    [SerializeField] private AudioClip fishBiteAudio;
    [SerializeField] private AudioClip fishCaughtAudio;
    [SerializeField] private AudioClip hookLandedAudio;

    private Rigidbody2D _rb;

    private bool _isLanded = false;
    private bool _gravitySet = false;
    private bool _blockUpMovement = false;
    private bool _blockDownMovement = false;
    private bool inTheSea = false;
    private bool returning = false;
    private bool ready;

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
            if (collision.gameObject.CompareTag("Sea") && !returning)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                SoundEffectsManager.instance.PlaySFXClip(hookLandedAudio, 1);
                _isLanded = true;
                inTheSea = true;
                _contactPointReturn = collision.ClosestPoint(this.gameObject.transform.position);
                if (SceneManager.GetActiveScene().name == "FishingTutorial")
                {
                    FishingTutorialManager.instance.DroppedHook();
                }
            }
        }
        
        if (collision.gameObject.CompareTag("Fish") && !returning)
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
            Vector3 contactPoint = collision.ClosestPoint(gameObject.transform.position);
            Vector3 center = collider.bounds.center;

            _blockDownMovement = contactPoint.y < center.y;
            _blockUpMovement = contactPoint.y > center.y;
        }
    }

    public void HookMovement()
    {
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !_blockUpMovement && !FishingPointsManager.instance.stop && transform.position.y < 1.25)
        {
            gameObject.transform.position += new Vector3(0, movementLenght,0);
        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !_blockUpMovement && !FishingPointsManager.instance.stop && transform.position.x < _startPosition.x)
        {
            _contactPointReturn += new Vector3((movementLenght / 3), 0, 0);
            gameObject.transform.position += new Vector3((movementLenght / 3), 0, 0);
        }
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !_blockDownMovement && !FishingPointsManager.instance.stop && transform.position.y > marginBottom.position.y)
        {
            gameObject.transform.position -= new Vector3(0, movementLenght, 0);
        }
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !_blockUpMovement && !FishingPointsManager.instance.stop && transform.position.x > marginLeft.position.x)
        {
            _contactPointReturn += new Vector3(-(movementLenght / 3), 0, 0);
            gameObject.transform.position += new Vector3(-(movementLenght / 3), 0, 0);
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
        ready = false;
        SoundEffectsManager.instance.PlaySFXClip(fishBiteAudio, (float)0.85);
        StartCoroutine(CheckHookCondition());
        StartCoroutine("MoveFishUpward");
    }

    private IEnumerator CheckHookCondition()
    {
        yield return new WaitForSeconds(_fishStrenght + (float)0.5);
        if (!ready)
        {
            transform.position = new Vector2(_startPosition.x, _startPosition.y);
            _isLanded = false;
            _gravitySet = false;
            _blockUpMovement = false;
            _blockDownMovement = false;
            returning = false;
        }
    }

    IEnumerator MoveFishUpward()
    {
        float duration = _fishStrenght;
        float elapsed = 0;
        Vector3 start = transform.position;
        Vector3 target = _startPosition;
        while (elapsed < duration)
        {
            if (FishingPointsManager.instance.stop)
                yield break;
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
        Restart();
    }

    public void Restart()
    {
        if (this.gameObject.transform.position.y >= _contactPointReturn.y)
        {
            ready = true;
            SoundEffectsManager.instance.PlaySFXClip(fishCaughtAudio, (float)0.7);
            onFishOutOfWater?.Invoke();
            transform.position = new Vector2(_startPosition.x, _startPosition.y);
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
