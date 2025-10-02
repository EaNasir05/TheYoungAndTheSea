using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FishingAreas", menuName = "Scriptable Objects/FishingAreas")]
public class FishingAreas : ScriptableObject
{
    public FishingArea[] list;
}

[Serializable]
public class FishingArea
{
    [SerializeField] private string areaName;
    [SerializeField] private bool unlocked;
    [SerializeField] private FishInTheSea[] fishes;

    public string GetName() { return areaName; }
    public bool IsUnlocked() { return unlocked; }
    public FishInTheSea[] GetFishes() { return fishes; }

    public void Unlock() { unlocked = true; }
}

[Serializable]
public class FishInTheSea
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int spawningLine;
    [SerializeField] private float spawnRate;
    
    public int GetSpawningLine() { return spawningLine; }
    public GameObject GetPrefab() { return prefab; }
    public float GetSpawnRate() { return spawnRate; }
}
