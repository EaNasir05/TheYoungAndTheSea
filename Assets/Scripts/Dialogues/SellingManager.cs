using System.Collections.Generic;
using UnityEngine;

public class SellingManager : MonoBehaviour
{
    public static SellingManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
    }
}
