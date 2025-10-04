using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FishingTutorialManager : MonoBehaviour
{
    public static FishingTutorialManager instance;
    [SerializeField] private GameObject progress;
    [SerializeField] private GameObject pointsField;
    [SerializeField] private GameObject chargeBar;
    [SerializeField] private Image blackWall;
    [SerializeField] private GameObject lineInput;
    [SerializeField] private GameObject hookInput;
    [SerializeField] private GameObject[] spawners;
    private Dictionary<string, int> fishesCaught;
    private bool timeIsOver;
    private float fadeTime;
    private int tutorialPhase;
    private bool ready;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        timeIsOver = false;
        instance = this;
        fishesCaught = new Dictionary<string, int>
        {
            { "Bugino", 0 },
            { "Favotto", 0 }
        };
        FishingPointsManager.instance = new FishingPointsManager();
        FishingPointsManager.instance.stop = true;
        Debug.Log(FishingPointsManager.instance.stop);
        fadeTime = 2;
    }

    private void Start()
    {
        StartCoroutine(EnterScene());
    }

    private void Update()
    {
        if (tutorialPhase == 1 && Input.GetKey(KeyCode.Space))
        {
            lineInput.SetActive(false);
        }
        if (tutorialPhase == 2 && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
        {
            hookInput.SetActive(false);
        }
    }

    public void AddFish(Sprite fish, string fishName)
    {
        if (tutorialPhase == 2)
        {
            ready = true;
        }
        else
        {
            if (fishesCaught[fishName] < 3)
            {
                fishesCaught[fishName]++;
                int index;
                if (fishName == "Bugino")
                {
                    index = 0;
                }
                else
                {
                    index = 1;
                }
                progress.transform.GetChild(index).GetComponent<TMP_Text>().text = fishesCaught[fishName] + "/3";
            }
            pointsField.transform.GetChild(1).GetComponent<Image>().sprite = fish;
            StartCoroutine(ShowFishCaught());
        }
    }

    public void DroppedHook()
    {
        if (tutorialPhase == 1)
        {
            ready = true;
        }
    }

    private IEnumerator ShowFishCaught()
    {
        pointsField.SetActive(true);
        if (tutorialPhase == 3 && fishesCaught["Bugino"] + fishesCaught["Favotto"] == 6)
        {
            ready = true;
        }
        yield return new WaitForSeconds(2);
        pointsField.SetActive(false);
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
        DialoguesManager.instance.StartDialogue("Grandpa");
    }

    public IEnumerator StopDialogue(int phase)
    {
        yield return new WaitForSeconds(1);
        tutorialPhase = phase;
        if (phase == 4)
        {
            StartCoroutine(ChangeScene("OcchioCalmo"));
        }
        else
        {
            StartCoroutine(WaitForCondition());
        }
    }

    private IEnumerator WaitForCondition()
    {
        ready = false;
        FishingPointsManager.instance.stop = false;
        if (tutorialPhase == 1)
        {
            lineInput.SetActive(true);
        }
        if (tutorialPhase == 2)
        {
            hookInput.SetActive(true);
            spawners[0].SetActive(true);
            spawners[1].SetActive(true);
        }
        if (tutorialPhase == 3)
        {
            progress.SetActive(true);
        }
        yield return new WaitUntil(() => ready);
        progress.SetActive(false);
        FishingPointsManager.instance.stop = true;
        DialoguesManager.instance.StartDialogue("Grandpa");
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
        SceneManager.LoadScene(scene);
    }
}
