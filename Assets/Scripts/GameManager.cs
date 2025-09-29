using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TMP_Text moneyCount;
    [SerializeField] private GameObject notification;
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

    public int GetDay() { return day; }
    public int GetMoney() { return money; }
    public bool IsTalking() { return talking; }
    public bool IsMorning() { return morning; }
    public int GetRestaurateurFriendship() { return restaurateurFriendship; }
    public int GetArtistFriendship() { return artistFriendship; }

    public void SetTalking(bool value) { talking = value; }
    public void SetMorning(bool value) { morning = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            day = 1;
            money = 0;
            talking = false;
            morning = true;
            upgraded = false;
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
            //DialoguesManager.instance.AddNextDialogueRestaurateur();
            upgraded = true;
        }
        if (restaurateurFriendship >= 200 && restaurateurFriendship < 3)
        {
            restaurateurFriendship = 3;
            //DialoguesManager.instance.AddNextDialogueRestaurateur();
            upgraded = true;
        }
        if (restaurateurFriendship >= 300 && restaurateurFriendship < 4)
        {
            restaurateurFriendship = 4;
            //DialoguesManager.instance.AddNextDialogueRestaurateur();
            upgraded = true;
        }
        if (restaurateurFriendship >= 400 && restaurateurFriendship < 5)
        {
            restaurateurFriendship = 5;
            //DialoguesManager.instance.AddNextDialogueRestaurateur();
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
            //DialoguesManager.instance.AddNextDialogueArtist();
            upgraded = true;
        }
        if (artistExp >= 200 && artistFriendship < 3)
        {
            artistFriendship = 3;
            //DialoguesManager.instance.AddNextDialogueArtist();
            upgraded = true;
        }
        if (artistExp >= 300 && artistFriendship < 4)
        {
            artistFriendship = 4;
            //DialoguesManager.instance.AddNextDialogueArtist();
            upgraded = true;
        }
        if (artistExp >= 500 && artistFriendship < 5)
        {
            artistFriendship = 5;
            //DialoguesManager.instance.AddNextDialogueArtist();
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
                notification.transform.GetChild(0).GetComponent<TMP_Text>().text = previousLevel + " >> " + restaurateurFriendship;
            }
            else
            {
                notification.transform.GetChild(0).GetComponent<TMP_Text>().text = previousLevel + " >> " + artistFriendship;
            }
            StartCoroutine(CreateNotification());
            upgraded = false;
        }
    }

    private IEnumerator CreateNotification()
    {
        notification.SetActive(true);
        yield return new WaitForSeconds(3);
        notification.SetActive(false);
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
