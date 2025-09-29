using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public static Dictionary<string, int> fishOwned;

    public static void Awake()
    {
        fishOwned = new Dictionary<string, int>
        {
            { "Sardina", 2 },
            { "Acciuga", 2 },
            { "Spigolo", 0 },
            { "Branzino", 0 },
            { "Pesce angelo", 0 },
            { "Pesce mandarino", 0 },
            { "Pesce vela", 0 },
            { "Tonno", 0 },
            { "Marlin", 0 },
            { "Sogliola", 0 },
            { "Pesce1", 0 },
            { "Pesce2", 0 }
        };
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
