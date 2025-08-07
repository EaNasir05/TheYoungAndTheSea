using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private int soldierValue;
    [SerializeField] private int artistValue;

    public string GetName() { return name; }
    public int GetRestaurateurValue() { return restaurateurValue; }
    public int GetSoldierValue() { return soldierValue; }
    public int GetArtistValue() { return artistValue; }
}

