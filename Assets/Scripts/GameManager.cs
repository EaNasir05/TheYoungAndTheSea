using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TMP_Text moneyCount;
    [SerializeField] private GameObject notification;
    [SerializeField] private GameObject moneyGain;
    [SerializeField] private Image blackWall;
    [SerializeField] private GameObject nightScreen;
    [SerializeField] private FishingAreas fishingAreas;
    [SerializeField] private Color morningSkyColor;
    [SerializeField] private Color nightSkyColor;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject fausto;
    [SerializeField] private GameObject gina;
    private int day;
    private int money;
    private bool morning;
    private bool talking;
    private bool upgraded;
    private int restaurateurFriendship;
    private int artistFriendship;
    private int restaurateurExp;
    private int artistExp;
    private int previousLevel;
    private int lastMoneyGain;
    private bool pricesListUnlocked;
    private bool colorsSchemeUnlocked;
    private float fadeTime;

    public int GetDay() { return day; }
    public int GetMoney() { return money; }
    public bool IsTalking() { return talking; }
    public bool IsMorning() { return morning; }
    public int GetRestaurateurFriendship() { return restaurateurFriendship; }
    public int GetArtistFriendship() { return artistFriendship; }
    public bool IsPricesListUnlocked() { return pricesListUnlocked; }
    public bool IsColorsSchemeUnlocked() { return colorsSchemeUnlocked; }

    public void SetTalking(bool value) { talking = value; }
    public void SetMorning(bool value) { morning = value; }
    public void SetLastMoneyGain(int value) { lastMoneyGain = value; }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            day = 0;
            money = 100;
            morning = false;
            upgraded = false;
            lastMoneyGain = 0;
            colorsSchemeUnlocked = false;
            pricesListUnlocked = false;
            Cursor.visible = false;
            fadeTime = 2;
            Inventory.Awake();
        }
        else if (instance != this)
        {
            CopyData(instance, this);
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        talking = true;
    }

    private void CopyData(GameManager oldGM, GameManager newGM)
    {
        newGM.day = oldGM.day;
        newGM.money = oldGM.money;
        newGM.morning = oldGM.morning;
        newGM.upgraded = oldGM.upgraded;
        newGM.restaurateurFriendship = oldGM.restaurateurFriendship;
        newGM.artistFriendship = oldGM.artistFriendship;
        newGM.restaurateurExp = oldGM.restaurateurExp;
        newGM.artistExp = oldGM.artistExp;
        newGM.previousLevel = oldGM.previousLevel;
        newGM.lastMoneyGain = oldGM.lastMoneyGain;
        newGM.pricesListUnlocked = oldGM.pricesListUnlocked;
        newGM.colorsSchemeUnlocked = oldGM.colorsSchemeUnlocked;
        newGM.fadeTime = oldGM.fadeTime;
    }

    private void Start()
    {
        if (morning)
        {
            nightScreen.SetActive(false);
            mainCamera.backgroundColor = morningSkyColor;
            if (day == 1)
            {
                fausto.SetActive(false);
                gina.SetActive(false);
            }
        }
        else
        {
            nightScreen.SetActive(true);
            mainCamera.backgroundColor = nightSkyColor;
            if (day == 0)
            {
                fausto.SetActive(false);
                gina.SetActive(false);
            }
        }
        CheckFishingAreas();
        moneyCount.text = money + " €";
        StartCoroutine(EnterScene());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject selected = EventSystem.current?.currentSelectedGameObject;
            if (selected != null)
            {
                Button button = selected.GetComponent<Button>();
                if (button != null)
                    button.onClick.Invoke();
            }
        }
    }

    public void AddMoney(int value)
    {
        money += value;
        moneyCount.text = money + " €";
    }

    public void AddRestaurateurExp(int value)
    {
        previousLevel = restaurateurFriendship;
        restaurateurExp += value;
        if (restaurateurExp >= 100 && restaurateurFriendship < 2)
        {
            restaurateurFriendship = 2;
            DialoguesManager.instance.AddNextDialogueRestaurateur(new NextDialogue(24, day));
            upgraded = true;
        }
        if (restaurateurFriendship >= 200 && restaurateurFriendship < 3)
        {
            restaurateurFriendship = 3;
            DialoguesManager.instance.AddNextDialogueRestaurateur(new NextDialogue(40, day));
            upgraded = true;
        }
        if (restaurateurFriendship >= 300 && restaurateurFriendship < 4)
        {
            restaurateurFriendship = 4;
            DialoguesManager.instance.AddNextDialogueRestaurateur(new NextDialogue(79, day));
            upgraded = true;
        }
        if (restaurateurFriendship >= 400 && restaurateurFriendship < 5)
        {
            restaurateurFriendship = 5;
            DialoguesManager.instance.AddNextDialogueRestaurateur(new NextDialogue(99, day));
            upgraded = true;
        }
    }

    public void AddArtistExp(int value)
    {
        previousLevel = artistFriendship;
        artistExp += value;
        if (artistExp >= 100 && artistFriendship < 2)
        {
            artistFriendship = 2;
            DialoguesManager.instance.AddNextDialogueArtist(new NextDialogue(24, day));
            upgraded = true;
        }
        if (artistExp >= 200 && artistFriendship < 3)
        {
            artistFriendship = 3;
            DialoguesManager.instance.AddNextDialogueArtist(new NextDialogue(78, day));
            upgraded = true;
        }
        if (artistExp >= 300 && artistFriendship < 4)
        {
            artistFriendship = 4;
            DialoguesManager.instance.AddNextDialogueArtist(new NextDialogue(50, day));
            upgraded = true;
        }
        if (artistExp >= 500 && artistFriendship < 5)
        {
            artistFriendship = 5;
            DialoguesManager.instance.AddNextDialogueArtist(new NextDialogue(108, day));
            upgraded = true;
        }
    }

    public void CheckForFriendshipUpgrades(int character)
    {
        if (upgraded)
        {
            notification.transform.GetChild(0).GetComponent<TMP_Text>().text = "NUOVO LIVELLO DI CONFIDENZA";
            notification.transform.GetChild(1).GetComponent<TMP_Text>().text = "Nuovo dialogo sbloccato\n" + "(torna domani)";
            StartCoroutine(CreateNotification());
            upgraded = false;
        }
    }

    public void UnlockColorsScheme()
    {
        notification.transform.GetChild(0).GetComponent<TMP_Text>().text = "SBLOCCATO SCHEMA COLORI";
        notification.transform.GetChild(1).GetComponent<TMP_Text>().text = "Premi [I] per controllare l'inventario";
        colorsSchemeUnlocked = true;
        StartCoroutine(CreateNotification());
    }

    public void UnlockPricesList()
    {
        notification.transform.GetChild(0).GetComponent<TMP_Text>().text = "SBLOCCATO LISTINO PREZZI";
        notification.transform.GetChild(1).GetComponent<TMP_Text>().text = "Premi [I] per controllare l'inventario";
        pricesListUnlocked = true;
        StartCoroutine(CreateNotification());
    }

    private IEnumerator CreateNotification()
    {
        notification.SetActive(true);
        yield return new WaitForSeconds(3);
        notification.SetActive(false);
    }

    public IEnumerator ShowLastMoneyGain()
    {
        if (lastMoneyGain > 0)
        {
            moneyGain.transform.GetChild(0).GetComponent<TMP_Text>().text = "+";
            moneyGain.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.green;
        }
        else
        {
            moneyGain.transform.GetChild(0).GetComponent<TMP_Text>().text = "";
            moneyGain.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.red;
        }
        moneyGain.transform.GetChild(0).GetComponent<TMP_Text>().text += lastMoneyGain + "€";
        moneyCount.transform.parent.gameObject.SetActive(true);
        moneyGain.SetActive(true);
        yield return new WaitForSeconds(3);
        moneyCount.transform.parent.gameObject.SetActive(false);
        moneyGain.SetActive(false);
    }

    private void CheckFishingAreas()
    {
        switch (day)
        {
            case 1:
                fishingAreas.list[0].Unlock();
                break;
            case 2:
                fishingAreas.list[1].Unlock();
                break;
            case 4:
                fishingAreas.list[2].Unlock();
                break;
            case 6:
                fishingAreas.list[3].Unlock();
                break;
        }
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
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
        if (day == 0 || day == 1)
        {
            DialoguesManager.instance.StartDialogue("Fisherman");
        }
        else
        {
            talking = false;
        }
    }

    public IEnumerator ChangeScene(string scene)
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

    public IEnumerator NextDay()
    {
        day++;
        morning = true;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
