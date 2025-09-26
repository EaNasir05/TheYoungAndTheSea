using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FishList", menuName = "ScriptableObjects/Fish")]
public class FishList : ScriptableObject
{
    public Fish[] list;

    public Fish GetFish(string fishName)
    {
        foreach (Fish fish in list)
        {
            if (fish.GetName() == fishName)
            {
                return fish;
            }
        }
        return null;
    }
}

[Serializable]
public class Fish
{
    [SerializeField] private string name;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int restaurateurValue;
    [SerializeField] private int artistValue;
    [SerializeField] private bool unlocked;

    public string GetName() { return name; }
    public GameObject GetPrefab() { return prefab; }
    public int GetRestaurateurValue() { return restaurateurValue; }
    public int GetArtistValue() { return artistValue; }
    public bool IsUnlocked() { return unlocked; }

    public void Unlock() { unlocked = true; }
}

