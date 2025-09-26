using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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
    private List<NextDialogue> nextDialoguesRestaurateur;
    private List<NextDialogue> nextDialoguesArtist;
    private NextDialogue currentDialogue;
    private int nextBranch;
    private int freedom;
    private bool firstTimeWithRestaurateur;
    private bool firstTimeWithArtist;
    private bool firstTimeWithFisherman;
    private bool ready;

    public void AddNextDialogueRestaurateur(NextDialogue nextDialogue)
    {
        nextDialoguesRestaurateur.Add(nextDialogue);
    }

    public void AddNextDialogueArtist(NextDialogue nextDialogue)
    {
        nextDialoguesArtist.Add(nextDialogue);
    }

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
        }
        ready = true;
    }

    private void Update()
    {
        if (!ready && Input.GetKeyDown(KeyCode.Space))
        {
            ready = true;
        }
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

    private IEnumerator ContinueDialogue(int character, int effect)
    {
        yield return new WaitForSeconds(1);
        ready = false;
        continueText.SetActive(true);
        yield return new WaitUntil(() => ready == true);
        switch (nextBranch)
        {
            case 1000:
                StartSelling(character);
                break;
            case 2000:
                dialogue.SetActive(false);
                GameManager.instance.SetTalking(false);
                break;
            default:
                CreateBranch(nextBranch, character, effect);
                break;
        }
    }

    private IEnumerator ShowDialogueOptions()
    {
        yield return new WaitForSeconds(1);
        dialogueOptions.SetActive(true);
    }

    public void CreateBranch(int index, int character, int effect)
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
        switch (character)
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
        continueText.SetActive(true);
        dialogue.SetActive(true);
        CreateAnswers(dialogues[index].GetAnswers(), character);
    }

    private void CreateAnswers(Answer[] answers, int character)
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
                button.onClick.AddListener(() => CreateBranch(availableAnswers[i].GetLink(), character, availableAnswers[i].GetEffect()));
                button.transform.GetChild(0).GetComponent<TMP_Text>().text = availableAnswers[i].GetText();
                button.gameObject.SetActive(true);
            }
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
            StartCoroutine(ContinueDialogue(character, effect));
        }
    }

    private void StartSelling(int character)
    {
        sellingMenu.SetActive(true);
    }

    public void StopSelling(int character)
    {
        sellingMenu.SetActive(false);
    }

    private void TalkWithFisherman()
    {
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
        if (firstTimeWithRestaurateur)
        {
            CreateBranch(2, 2, 6);
        }
        else if (nextDialoguesRestaurateur.Count > 0 && nextDialoguesRestaurateur[0].day != GameManager.instance.GetDay())
        {
            currentDialogue = nextDialoguesRestaurateur[0];
            CreateBranch(nextDialoguesRestaurateur[0].index, 2, 3);
        }
        else
        {
            CreateBranch(3, 2, 0);
        }
    }

    private void TalkWithArtist()
    {
        if (firstTimeWithArtist)
        {
            
        }
        else if (nextDialoguesArtist.Count > 0 && nextDialoguesArtist[0].day != GameManager.instance.GetDay())
        {
            currentDialogue = nextDialoguesArtist[0];
            CreateBranch(nextDialoguesArtist[0].index, 3, 4);
        }
        else
        {

        }
    }

    private void TalkWithGrandpa()
    {
        
    }
}
