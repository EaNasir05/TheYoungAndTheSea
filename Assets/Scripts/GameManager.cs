using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int day;
    private int money;
    private bool morning;
    private bool talking;
    private bool restaurateurFriendship;
    private bool artistFriendship;
    private int restaurateurExp;
    private int artistExp;

    public int GetDay() { return day; }
    public int GetMoney() { return money; }
    public bool IsTalking() { return talking; }
    public bool IsMorning() { return morning; }
    public bool GetRestaurateurFriendship() { return restaurateurFriendship; }
    public bool GetArtistFriendship() { return artistFriendship; }

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
            Cursor.visible = false;
            Inventory.Awake();
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
}
