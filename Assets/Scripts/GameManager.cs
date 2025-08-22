using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int days;
    private int money;
    private bool talking;
    private bool restaurateurFriendship;
    private bool soldierFriendship;
    private bool artistFriendship;
    private int freedom;
    private int restaurateurExp;
    private int soldierExp;
    private int artistExp;

    public int GetDays() { return days; }
    public int GetMoney() { return money; }
    public bool IsTalking() { return talking; }
    public bool GetRestaurateurFriendship() { return restaurateurFriendship; }
    public bool GetSoldierFriendship() { return soldierFriendship; }
    public bool GetArtistFriendship() { return artistFriendship; }
    public int GetFreedom() { return freedom; }

    public void SetTalking(bool value) { talking = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            days = 1;
            money = 0;
            freedom = 0;
            talking = false;
        }
    }
}
