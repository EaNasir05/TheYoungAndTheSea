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
    [SerializeField] private int velocityValue;
    [SerializeField] private int strengthValue;
    [SerializeField] private float spawnRateValue;

    public string GetName() { return name; }
    public int GetRestaurateurValue() { return restaurateurValue; }
    public int GetSoldierValue() { return soldierValue; }
    public int GetArtistValue() { return artistValue; }
    public int GetVelocityValue() { return velocityValue; }
    public int GetStrengthValue() { return strengthValue; }
    public float GetSpawnRateValue() { return spawnRateValue; }

}

