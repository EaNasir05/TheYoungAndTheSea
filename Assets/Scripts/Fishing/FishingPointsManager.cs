using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FishingPointsManager : MonoBehaviour
{
    public static FishingPointsManager instance;

    [SerializeField] private FishList fishList;
    [SerializeField] private GameObject pointsField;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject chargeBar;
    [SerializeField] private GameObject summary;
    [SerializeField] private Image blackWall;
    private Dictionary<string, int> fishesCaught;
    public bool stop;
    private bool timeIsOver;
    private float fadeTime;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        timeIsOver = false;
        instance = this;
        fishesCaught = new Dictionary<string, int>();
        stop = true;
        fadeTime = 2;
        for (int i = 0; i < 3; i++)
        {
            summary.transform.GetChild(2).GetChild(i).gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        StartCoroutine(EnterScene());
    }

    private void Update()
    {
        if (timeIsOver && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            StartCoroutine(ChangeScene("MainIsland"));
        }
    }

    public void AddFish(string fish)
    {
        if (fishesCaught.ContainsKey(fish))
        {
            fishesCaught[fish]++;
        }
        else
        {
            fishesCaught.Add(fish, 1);
        }
        pointsField.transform.GetChild(1).GetComponent<Image>().sprite = fishList.GetFish(fish).GetSprite();
        StartCoroutine(ShowFishCaught());
    }

    private IEnumerator ShowFishCaught()
    {
        pointsField.SetActive(true);
        yield return new WaitForSeconds(2);
        pointsField.SetActive(false);
    }

    public void TimeIsOver()
    {
        stop = true;
        int i = 0;
        foreach (KeyValuePair<string, int> kvp in fishesCaught)
        {
            Inventory.AddFish(kvp.Key, kvp.Value);
            fishList.GetFish(kvp.Key).Unlock();
            summary.transform.GetChild(2).GetChild(i).gameObject.SetActive(true);
            summary.transform.GetChild(2).GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = kvp.Value.ToString();
            summary.transform.GetChild(2).GetChild(i).GetChild(1).GetComponent<Image>().sprite = fishList.GetFish(kvp.Key).GetSprite();
            i++;
        }
        timeIsOver = true;
        chargeBar.SetActive(false);
        timer.SetActive(false);
        summary.SetActive(true);
    }

    private IEnumerator EnterScene()
    {
        Color c = blackWall.color;
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, t / fadeTime);
            blackWall.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        blackWall.color = new Color(c.r, c.g, c.b, 0);
        stop = false;
    }

    private IEnumerator ChangeScene(string scene)
    {
        Color c = blackWall.color;
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, t / fadeTime);
            blackWall.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        blackWall.color = new Color(c.r, c.g, c.b, 1);
        GameManager.instance.SetMorning(false);
        SceneManager.LoadScene(scene);
    }
}
