using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuoySelection : MonoBehaviour
{
    private EventSystem eventSystem;
    [SerializeField] private GameObject circle;

    private void Start()
    {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == this)
        {
            circle.SetActive(true);
        }
        else
        {
            circle.SetActive(false);
        }
    }
}
