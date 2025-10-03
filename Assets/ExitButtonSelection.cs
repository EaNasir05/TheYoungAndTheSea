using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExitButtonSelection : MonoBehaviour
{
    private EventSystem eventSystem;
    private TMP_Text textField;
    private Color startingColor;

    private void Start()
    {
        textField = transform.GetChild(0).GetComponent<TMP_Text>();
        startingColor = textField.color;
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == this)
        {
            textField.color = Color.black;
        }
        else
        {
            textField.color = startingColor;
        }
    }
}
