using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenInventory : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private FishList fishList;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = fishList.list[i].GetSprite();
                    inventory.transform.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text = Inventory.fishOwned[fishList.list[i].GetName()].ToString();
                    if (fishList.list[i].IsUnlocked())
                    {
                        inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        inventory.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = Color.black;
                    }
                    inventory.SetActive(true);
                }
            }
        }
    }
}
