using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoatInteraction : MonoBehaviour
{
    [SerializeField] private FishList fishList;
    [SerializeField] private FishingAreas fishingAreas;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject seaMap;
    [SerializeField] private GameObject[] buoys;
    [SerializeField] private GameObject xButton;
    private bool nextToPlayer;
    private string selectedFishingArea;

    private void Awake()
    {
        nextToPlayer = false;
    }

    private void Update()
    {
        if (!GameManager.instance.IsTalking() && Input.GetKeyDown(KeyCode.E) && nextToPlayer)
        {
            GameManager.instance.SetTalking(true);
            for (int i = 0; i < 4; i++)
            {
                buoys[i].GetComponent<Button>().interactable = fishingAreas.list[i].IsUnlocked();
                buoys[i].transform.GetChild(0).gameObject.SetActive(fishingAreas.list[i].IsUnlocked());
            }
            seaMap.SetActive(true);
            eventSystem.SetSelectedGameObject(buoys[0]);
        }
    }

    public void CloseMap()
    {
        seaMap.SetActive(false);
        GameManager.instance.SetTalking(false);
    }

    public void SelectFishingArea(string area)
    {
        selectedFishingArea = area;
        for (int i = 0; i < 4; i++)
        {
            buoys[i].GetComponent<BuoySelection>().HideCircle();
            buoys[i].SetActive(false);
        }
        xButton.SetActive(false);
        string[] fishes = fishingAreas.GetArea(area).GetFishes();
        switch (area)
        {
            case "OcchioCalmo":
                seaMap.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "OCCHIO CALMO";
                break;
            case "BancoArgenteo":
                seaMap.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "BANCO ARGENTEO";
                break;
            case "FondaleCorallino":
                seaMap.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "FONDALE CORALLINO";
                break;
            case "DorsaleSommersa":
                seaMap.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "DORSALE SOMMERSA";
                break;
            default:
                seaMap.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "AREA SCONOSCIUTA";
                break;
        }
        for (int i = 0; i < 3; i++)
        {
            seaMap.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Image>().sprite = fishList.GetFish(fishes[i]).GetSprite();
            if (fishList.GetFish(fishes[i]).IsUnlocked())
            {
                seaMap.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Image>().color = Color.white;
            }
            else
            {
                seaMap.transform.GetChild(0).GetChild(2).GetChild(i).GetComponent<Image>().color = Color.black;
            }
        }
        seaMap.transform.GetChild(0).gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(seaMap.transform.GetChild(0).GetChild(4).gameObject);
    }

    public void CloseAreaInfo()
    {
        for (int i = 0; i < 4; i++)
        {
            buoys[i].SetActive(true);
        }
        xButton.SetActive(true);
        seaMap.transform.GetChild(0).gameObject.SetActive(false);
        eventSystem.SetSelectedGameObject(buoys[0]);
    }

    public void GoFishing()
    {
        if (GameManager.instance.GetDay() == 1)
        {
            StartCoroutine(GameManager.instance.ChangeScene("FishingTutorial"));
        }
        else
        {
            StartCoroutine(GameManager.instance.ChangeScene(selectedFishingArea));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.instance.IsMorning())
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
