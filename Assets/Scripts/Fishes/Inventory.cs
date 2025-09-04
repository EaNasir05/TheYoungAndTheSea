using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public static Dictionary<string, int> fishOwned;

    public static void Awake()
    {
        fishOwned = new Dictionary<string, int>();
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
        fishOwned.Add("", 0);
    }

    public static void AddFish(string fishName, int value)
    {
        fishOwned[fishName] += value;
    }

    public static void RemoveFish(string fishName, int value)
    {
        fishOwned[fishName] -= value;
    }
}
