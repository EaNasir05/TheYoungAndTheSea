using System.Collections;
using UnityEngine;

public class ChangeSkyColor : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private float fadeDuration;
    [SerializeField] private Color targetColor;
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        StartCoroutine(ChangeBackgroundAfterDelay());
    }

    private IEnumerator ChangeBackgroundAfterDelay()
    {
        yield return new WaitForSeconds(waitTime);
        Color startColor = mainCamera.backgroundColor;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            mainCamera.backgroundColor = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        mainCamera.backgroundColor = targetColor;
    }
}
