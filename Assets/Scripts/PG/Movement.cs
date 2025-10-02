using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        if (!GameManager.instance.IsTalking())
        {
            transform.position = new Vector2(transform.position.x + speed * Input.GetAxisRaw("Horizontal"), transform.position.y);
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (Input.GetAxisRaw("Horizontal") == -1)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }
}
