using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Rendering;

public class Timer : MonoBehaviour
{
    [SerializeField] public float totalTime;
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] public float duration;
    [SerializeField] public float startRedTimer;

    private bool isPaused = false;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private float elapsedTime;
    private float percentage;
    bool IsExpanding = true;

    public void Start()
    {
        startPosition = timerText.gameObject.transform.localScale;
        endPosition = timerText.gameObject.transform.localScale + new Vector3(0.5f, 0.5f, 0);
        elapsedTime = 0;
        percentage = 0;

    }
    void Update()
    {
        if (!isPaused)
        {
            if (totalTime > 0)
            {
                totalTime -= Time.deltaTime;
            }
            else
            {
                totalTime = 0;
                FishingPointsManager.instance.TimeIsOver();
            }
            if (totalTime <= startRedTimer)
            {
                timerText.color = Color.red;
                if (IsExpanding)
                {
                    LerpTimer(startPosition, endPosition);
                }
                else
                {
                    LerpTimer(endPosition, startPosition);
                }

            }
        }
        DisplayTime(totalTime);
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    public void LerpTimer(Vector3 a, Vector3 b)
    {
        elapsedTime += Time.deltaTime;
        percentage = elapsedTime / duration;
        timerText.gameObject.transform.localScale = Vector3.Lerp(a, b, percentage);
        if (percentage >= 1f)
        {
            IsExpanding = !IsExpanding;
            elapsedTime = 0f;
            percentage = 0f;
        }

    }

}
