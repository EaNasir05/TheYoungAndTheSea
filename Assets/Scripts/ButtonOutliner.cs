using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonOutliner : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    private Outline outliner;

    private void Start()
    {
        outliner = GetComponent<Outline>();
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == gameObject)
        {
            outliner.enabled = true;
        }
        else if (outliner.enabled)
        {
            outliner.enabled = false;
        }
    }
}
