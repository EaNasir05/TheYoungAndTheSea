using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenInventory : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private FishList fishList;

    private void Awake()
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject fish = inventory.transform.GetChild(12).GetChild(i).gameObject;
            if (fishList.GetFish(fish.name).IsUnlocked())
            {
                fish.GetComponent<Image>().color = Color.white;
            }
            else
            {
                fish.GetComponent<Image>().color = Color.black;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !GameManager.instance.IsTalking())
        {
            if (inventory.activeSelf)
            {
                inventory.SetActive(false);
            }
            else
            {
                if (GameManager.instance.IsPricesListUnlocked())
                {
                    inventory.transform.GetChild(12).gameObject.SetActive(true);
                }
                if (GameManager.instance.IsColorsSchemeUnlocked())
                {
                    inventory.transform.GetChild(13).gameObject.SetActive(true);
                }
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
