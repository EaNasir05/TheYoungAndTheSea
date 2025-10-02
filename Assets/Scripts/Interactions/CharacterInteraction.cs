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
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nextToPlayer = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
