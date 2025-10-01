using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TMP_Text moneyCount;
    [SerializeField] private GameObject notification;
    [SerializeField] private TMP_Text moneyGain;
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
        if (instance == null)
        {
            instance = this;
            day = 0;
            money = 100;
            talking = false;
            morning = true;
            upgraded = false;
            lastMoneyGain = 0;
            colorsSchemeUnlocked = false;
            pricesListUnlocked = false;
            Cursor.visible = false;
            Inventory.Awake();
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
            if (character == 2)
            {
                notification.transform.GetChild(1).GetComponent<TMP_Text>().text = previousLevel + " >> " + restaurateurFriendship;
            }
            else
            {
                notification.transform.GetChild(1).GetComponent<TMP_Text>().text = previousLevel + " >> " + artistFriendship;
            }
            StartCoroutine(CreateNotification());
            upgraded = false;
        }
    }

    public void UnlockColorsScheme()
    {
        notification.transform.GetChild(0).GetComponent<TMP_Text>().text = "SBLOCCATO SCHEMA COLORI";
        notification.transform.GetChild(1).GetComponent<TMP_Text>().text = "Premere [I]";
        colorsSchemeUnlocked = true;
        StartCoroutine(CreateNotification());
    }

    public void UnlockPricesList()
    {
        notification.transform.GetChild(0).GetComponent<TMP_Text>().text = "SBLOCCATO LISTINO PREZZI";
        notification.transform.GetChild(1).GetComponent<TMP_Text>().text = "Premere [I]";
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
            moneyGain.text = "+";
            moneyGain.color = Color.green;
        }
        else
        {
            moneyGain.text = "";
            moneyGain.color = Color.red;
        }
        moneyGain.text += lastMoneyGain + "€";
        moneyGain.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        moneyGain.gameObject.SetActive(false);
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
    }
}
