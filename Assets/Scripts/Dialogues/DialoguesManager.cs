using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NextDialogue
{
    public int index;
    public int day;

    public NextDialogue(int index, int day)
    {
        this.index = index;
        this.day = day;
    }
}

public class DialoguesManager : MonoBehaviour
{
    public static DialoguesManager instance;

    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GrandpaDialogues grandpa;
    [SerializeField] private RestaurateurDialogues restaurateur;
    [SerializeField] private FishermanDialogues fisherman;
    [SerializeField] private ArtistDialogues artist;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject continueText;
    [SerializeField] private GameObject dialogueOptions;
    [SerializeField] private GameObject sellingMenu;
    [SerializeField] private GameObject sellingFishes;
    [SerializeField] private GameObject inventoryFishes;
    [SerializeField] private GameObject gain;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button stopSellingButton;
    [SerializeField] private FishList fishList;
    [SerializeField] private GameObject fishInfo;
    private List<NextDialogue> nextDialoguesRestaurateur;
    private List<NextDialogue> nextDialoguesArtist;
    private NextDialogue currentDialogue;
    private int currentCharacter;
    private int nextBranch;
    private int freedom;
    private bool firstTimeWithRestaurateur;
    private bool firstTimeWithArtist;
    private bool firstTimeWithFisherman;
    private bool ready;
    private bool selling;
    private int moneyGaining;
    private Dictionary<string, int> selectedFishes;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            freedom = 0;
            nextDialoguesRestaurateur = new();
            nextDialoguesArtist = new();
            firstTimeWithFisherman = true;
            firstTimeWithRestaurateur = true;
            firstTimeWithArtist = true;
            selling = false;
            selectedFishes = new Dictionary<string, int>();
        }
        ready = true;
    }

    private void Update()
    {
        if (!ready && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
        {
            ready = true;
        }
    }

    public void AddNextDialogueRestaurateur(NextDialogue nextDialogue)
    {
        nextDialoguesRestaurateur.Add(nextDialogue);
    }

    public void AddNextDialogueArtist(NextDialogue nextDialogue)
    {
        nextDialoguesArtist.Add(nextDialogue);
    }

    private bool CheckCondition(int index)
    {
        switch (index)
        {
            case 1:
                return freedom <= 0;
            case 2:
                return freedom >= 0;
            case 3:
                return freedom >= 1;
            case 4:
                return freedom >= 2;
            case 5:
                return freedom <= 1;
            case 6:
                return freedom <= -1;
            case 7:
                return freedom <= -2;
            case 8:
                return freedom >= -1;
            case 9:
                return GameManager.instance.GetMoney() >= 500;
            default:
                return true;
        }
    }

    private void ApplyEffect(int index)
    {
        switch (index)
        {
            case 1:
                freedom++;
                break;
            case 2:
                freedom--;
                break;
            case 3:
                nextDialoguesRestaurateur.Remove(currentDialogue);
                break;
            case 4:
                nextDialoguesArtist.Remove(currentDialogue);
                break;
            case 5:
                firstTimeWithFisherman = false;
                break;
            case 6:
                firstTimeWithRestaurateur = false;
                break;
            case 7:
                firstTimeWithArtist = false;
                break;
            case 8:
                Debug.Log("SBLOCCA LISTINO PREZZI");
                break;
            case 9:
                Debug.Log("SBLOCCA LA CAPANNA");
                break;
            case 10:
                Debug.Log("SBLOCCA SCHEMA CROMATICO");
                break;
            default:
                return;
        }
    }

    public void StartDialogue(string character)
    {
        GameManager.instance.SetTalking(true);
        switch (character)
        {
            case "Fisherman":
                TalkWithFisherman();
                break;
            case "Restaurateur":
                TalkWithRestaurateur();
                break;
            case "Artist":
                TalkWithArtist();
                break;
            case "Grandpa":
                break;
            default:
                Debug.Log("PERSONAGGIO INESISTENTE");
                break;
        }
    }

    private IEnumerator ContinueDialogue(int effect)
    {
        yield return new WaitForSeconds(1);
        ready = false;
        continueText.SetActive(true);
        yield return new WaitUntil(() => ready == true);
        switch (nextBranch)
        {
            case 1000:
                ApplyEffect(effect);
                StartSelling();
                break;
            case 2000:
                ApplyEffect(effect);
                dialogue.SetActive(false);
                GameManager.instance.SetTalking(false);
                GameManager.instance.CheckForFriendshipUpgrades(currentCharacter);
                break;
            default:
                CreateBranch(nextBranch, effect);
                break;
        }
    }

    private IEnumerator ShowDialogueOptions()
    {
        yield return new WaitForSeconds(1);
        dialogueOptions.SetActive(true);
    }

    public void CreateBranch(int index, int effect)
    {
        dialogue.SetActive(false);
        continueText.SetActive(false);
        dialogueOptions.SetActive(false);
        for (int i = 0; i < 3; i ++)
        {
            dialogueOptions.transform.GetChild(i).gameObject.SetActive(false);
        }
        ApplyEffect(effect);
        Dialogue[] dialogues;
        switch (currentCharacter)
        {
            case 1:
                dialogues = fisherman.dialogues;
                break;
            case 2:
                dialogues = restaurateur.dialogues;
                break;
            case 3:
                dialogues = artist.dialogues;
                break;
            default:
                dialogues = grandpa.dialogues;
                break;
        }
        //cambia immagine personaggio
        characterName.text = dialogues[index].GetCharacter();
        dialogueText.text = dialogues[index].GetText();
        dialogue.SetActive(true);
        CreateAnswers(dialogues[index].GetAnswers());
    }

    private void CreateAnswers(Answer[] answers)
    {
        List<Answer> availableAnswers = new();
        foreach (Answer answer in answers)
        {
            if (CheckCondition(answer.GetCondition()))
            {
                availableAnswers.Add(answer);
            }
        }
        if (availableAnswers.Count > 1)
        {
            for (int i = 0; i < availableAnswers.Count; i++)
            {
                Button button = dialogueOptions.transform.GetChild(i).GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                int index = i;
                button.onClick.AddListener(() => CreateBranch(availableAnswers[index].GetLink(), availableAnswers[index].GetEffect()));
                button.transform.GetChild(0).GetComponent<TMP_Text>().text = availableAnswers[i].GetText();
                button.gameObject.SetActive(true);
            }
            eventSystem.SetSelectedGameObject(dialogueOptions.transform.GetChild(0).gameObject);
            StartCoroutine(ShowDialogueOptions());
        }
        else
        {
            int effect;
            if (availableAnswers.Count == 0)
            {
                nextBranch = 2000;
                effect = 0;
            }
            else if (availableAnswers[0].GetLink() == 1000)
            {
                nextBranch = 1000;
                effect = 0;
            }
            else
            {
                nextBranch = availableAnswers[0].GetLink();
                effect = availableAnswers[0].GetEffect();
            }
            StartCoroutine(ContinueDialogue(effect));
        }
    }

    private void StartSelling()
    {
        fishInfo.SetActive(false);
        moneyGaining = 0;
        gain.GetComponent<TMP_Text>().text = "0 €";
        dialogue.SetActive(false);
        selling = true;
        selectedFishes.Clear();
        int i = 0;
        foreach (KeyValuePair<string, int> kvp in Inventory.fishOwned)
        {
            string fish = kvp.Key;
            int number = kvp.Value;
            Transform fishSlot = inventoryFishes.transform.GetChild(i);
            fishSlot.GetChild(1).GetComponent<TMP_Text>().text = number.ToString();
            foreach (Fish fishInList in fishList.list)
            {
                if (fishInList.GetName() == fish)
                {
                    fishSlot.GetChild(0).GetComponent<Image>().sprite = fishInList.GetSprite();
                    sellingFishes.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = fishInList.GetSprite();
                    if (fishInList.IsUnlocked())
                    {
                        if (number > 0)
                        {
                            int index = i;
                            string fishName = fish;
                            sellingFishes.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                            sellingFishes.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => RemoveFishFromSelling(fishName, index));
                            fishSlot.GetComponent<Button>().onClick.RemoveAllListeners();
                            fishSlot.GetComponent<Button>().onClick.AddListener(() => AddFishToSelling(fishName, index));
                            fishSlot.GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            fishSlot.GetComponent<Button>().interactable = false;
                        }
                        fishSlot.GetChild(0).GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        fishSlot.GetComponent<Button>().interactable = false;
                        fishSlot.GetChild(0).GetComponent<Image>().color = Color.black;
                    }
                    break;
                }
            }
            i++;
        }
        for (int x = 0; x < 12; x++)
        {
            sellingFishes.transform.GetChild(x).GetComponent<Button>().interactable = false;
            sellingFishes.transform.GetChild(x).GetChild(0).gameObject.SetActive(false);
            sellingFishes.transform.GetChild(x).GetChild(1).gameObject.SetActive(false);
        }
        sellButton.interactable = false;
        sellingMenu.SetActive(true);
        SelectAvailableButton();
    }

    private void SelectAvailableButton()
    {
        for (int i = 0; i < 12; i++)
        {
            if (inventoryFishes.transform.GetChild(i).GetComponent<Button>().interactable)
            {
                eventSystem.SetSelectedGameObject(inventoryFishes.transform.GetChild(i).gameObject);
                return;
            }
        }
        for (int i = 0; i < 12; i++)
        {
            if (sellingFishes.transform.GetChild(i).GetComponent<Button>().interactable)
            {
                eventSystem.SetSelectedGameObject(sellingFishes.transform.GetChild(i).gameObject);
                return;
            }
        }
        eventSystem.SetSelectedGameObject(stopSellingButton.gameObject);
    }

    public void AddFishToSelling(string fish, int slot)
    {
        TMP_Text fishCount = inventoryFishes.transform.GetChild(slot).GetChild(1).GetComponent<TMP_Text>();
        int number = int.Parse(fishCount.text);
        number--;
        fishCount.text = number.ToString();
        int n = number;
        if (selectedFishes.ContainsKey(fish))
        {
            selectedFishes[fish]++;
            number = int.Parse(sellingFishes.transform.GetChild(slot).GetChild(1).GetComponent<TMP_Text>().text);
            number++;
            sellingFishes.transform.GetChild(slot).GetChild(1).GetComponent<TMP_Text>().text = number.ToString();
        }
        else
        {
            selectedFishes.Add(fish, 1);
            sellingFishes.transform.GetChild(slot).GetChild(1).gameObject.SetActive(true);
            sellingFishes.transform.GetChild(slot).GetChild(0).gameObject.SetActive(true);
            sellingFishes.transform.GetChild(slot).GetComponent<Button>().interactable = true;
            sellingFishes.transform.GetChild(slot).GetChild(1).GetComponent<TMP_Text>().text = "1";
        }
        if (n == 0)
        {
            inventoryFishes.transform.GetChild(slot).GetComponent<Button>().interactable = false;
            SelectAvailableButton();
        }
        int value = 1;
        if (currentCharacter == 2)
        {
            foreach (Fish fishInList in fishList.list)
            {
                if (fishInList.GetName() == fish)
                {
                    value = fishInList.GetRestaurateurValue();
                    break;
                }
            }
        }
        else if (currentCharacter == 3)
        {
            foreach (Fish fishInList in fishList.list)
            {
                if (fishInList.GetName() == fish)
                {
                    value = fishInList.GetArtistValue();
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Chi cazzo è il currentCharacter?");
        }
        fishInfo.transform.GetChild(0).GetComponent<TMP_Text>().text = fish;
        fishInfo.transform.GetChild(2).GetComponent<TMP_Text>().text = value + " €";
        fishInfo.transform.GetChild(1).GetComponent<Image>().sprite = inventoryFishes.transform.GetChild(slot).GetChild(0).GetComponent<Image>().sprite;
        fishInfo.SetActive(true);
        moneyGaining += value;
        gain.GetComponent<TMP_Text>().text = moneyGaining + " €";
        sellButton.interactable = true;
    }

    public void RemoveFishFromSelling(string fish, int slot)
    {
        int number = int.Parse(sellingFishes.transform.GetChild(slot).GetChild(1).GetComponent<TMP_Text>().text);
        number--;
        sellingFishes.transform.GetChild(slot).GetChild(1).GetComponent<TMP_Text>().text = number.ToString();
        selectedFishes[fish]--;
        inventoryFishes.transform.GetChild(slot).GetComponent<Button>().interactable = true;
        if (number == 0)
        {
            selectedFishes.Remove(fish);
            sellingFishes.transform.GetChild(slot).GetChild(0).gameObject.SetActive(false);
            sellingFishes.transform.GetChild(slot).GetChild(1).gameObject.SetActive(false);
            sellingFishes.transform.GetChild(slot).GetComponent<Button>().interactable = false;
            SelectAvailableButton();
        }
        number = int.Parse(inventoryFishes.transform.GetChild(slot).GetChild(1).GetComponent<TMP_Text>().text);
        number++;
        inventoryFishes.transform.GetChild(slot).GetChild(1).GetComponent<TMP_Text>().text = number.ToString();
        int value = 1;
        if (currentCharacter == 2)
        {
            foreach (Fish fishInList in fishList.list)
            {
                if (fishInList.GetName() == fish)
                {
                    value = fishInList.GetRestaurateurValue();
                    break;
                }
            }
        }
        else if (currentCharacter == 3)
        {
            foreach (Fish fishInList in fishList.list)
            {
                if (fishInList.GetName() == fish)
                {
                    value = fishInList.GetArtistValue();
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Chi cazzo è il currentCharacter?");
        }
        moneyGaining -= value;
        gain.GetComponent<TMP_Text>().text = moneyGaining + " €";
        if (selectedFishes.Count == 0)
        {
            sellButton.interactable = false;
        }
    }

    public void StopSelling()
    {
        selling = false;
        sellingMenu.SetActive(false);
        if (firstTimeWithRestaurateur && currentCharacter == 2)
        {
            CreateBranch(17, 0);
        }
        else
        {
            CreateBranch(3, 0);
        }
    }

    private void TalkWithFisherman()
    {
        currentCharacter = 1;
        if (firstTimeWithFisherman)
        {

        }
        else if (GameManager.instance.IsMorning())
        {

        }
        else
        {
            
        }
    }

    private void TalkWithRestaurateur()
    {
        currentCharacter = 2;
        if (firstTimeWithRestaurateur)
        {
            CreateBranch(5, 0);
        }
        else if (nextDialoguesRestaurateur.Count > 0 && nextDialoguesRestaurateur[0].day != GameManager.instance.GetDay())
        {
            currentDialogue = nextDialoguesRestaurateur[0];
            CreateBranch(nextDialoguesRestaurateur[0].index, 3);
        }
        else
        {
            CreateBranch(0, 0);
        }
    }

    private void TalkWithArtist()
    {
        currentCharacter = 3;
        if (firstTimeWithArtist)
        {
            
        }
        else if (nextDialoguesArtist.Count > 0 && nextDialoguesArtist[0].day != GameManager.instance.GetDay())
        {
            currentDialogue = nextDialoguesArtist[0];
            CreateBranch(nextDialoguesArtist[0].index, 4);
        }
        else
        {

        }
    }

    private void TalkWithGrandpa()
    {
        currentCharacter = 4;
    }

    public void SellFishes()
    {
        foreach (KeyValuePair<string, int> kvp in selectedFishes)
        {
            string fish = kvp.Key;
            int number = kvp.Value;
            int value = 1;
            if (currentCharacter == 2)
            {
                foreach(Fish fishInList in fishList.list)
                {
                    if (fishInList.GetName() == fish)
                    {
                        value = fishInList.GetRestaurateurValue();
                        break;
                    }
                }
                GameManager.instance.AddRestaurateurExp(number * value);
            }
            else if (currentCharacter == 3)
            {
                foreach (Fish fishInList in fishList.list)
                {
                    if (fishInList.GetName() == fish)
                    {
                        value = fishInList.GetArtistValue();
                        break;
                    }
                }
                GameManager.instance.AddArtistExp(number * value);
            }
            else
            {
                Debug.Log("Chi cazzo è il currentCharacter?");
            }
            GameManager.instance.AddMoney(number * value);
            Inventory.RemoveFish(fish, number);
        }
        StopSelling();
    }
}
