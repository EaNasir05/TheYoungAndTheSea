using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool nextToPlayer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextToPlayer = false;
    }

    private void Update()
    {
        if (!GameManager.instance.IsTalking() && Input.GetKeyDown(KeyCode.E) && nextToPlayer)
        {
            DialoguesManager.instance.StartDialogue(gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nextToPlayer = true;
            spriteRenderer.color = Color.yellow;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nextToPlayer = false;
            spriteRenderer.color = Color.white;
        }
    }
}
