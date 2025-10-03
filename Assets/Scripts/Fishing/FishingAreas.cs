using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FishingAreas", menuName = "Scriptable Objects/FishingAreas")]
public class FishingAreas : ScriptableObject
{
    public FishingArea[] list;

    public FishingArea GetArea(string name)
    {
        foreach (FishingArea area in list)
        {
            if (area.GetName() == name)
            {
                return area;
            }
        }
        return null;
    }
}

[Serializable]
public class FishingArea
{
    [SerializeField] private string areaName;
    [SerializeField] private bool unlocked;
    [SerializeField] private string[] fishes;

    public string GetName() { return areaName; }
    public bool IsUnlocked() { return unlocked; }
    public string[] GetFishes() { return fishes; }

    public void Unlock() { unlocked = true; }
}
