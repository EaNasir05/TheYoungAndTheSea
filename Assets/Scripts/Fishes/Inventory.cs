using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public static Dictionary<string, int> fishOwned = new();

    public static void AddFish(string fishName)
    {
        if (fishOwned.ContainsKey(fishName))
        {
            fishOwned[fishName]++;
        }
        else
        {
            fishOwned.Add(fishName, 1);
        }
    }

    public static void RemoveFish(string fishName)
    {
        fishOwned[fishName]--;
    }
}
