using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguesManager : MonoBehaviour
{
    public static DialoguesManager instance;

    [SerializeField] private GameObject dialogue;
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private GameObject continueText;
    [SerializeField] private GameObject dialogueOptions;
    private bool firstTimeWithRestaurateur;
    private bool firstTimeWithSoldier;
    private bool firstTimeWithArtist;
    private bool firstTimeWithFisherman;
    private bool ready;

    private void Awake()
    {
        instance ??= this;
        ready = true;
    }

    private void Update()
    {
        if (!ready && Input.GetKeyDown(KeyCode.Space))
        {
            ready = true;
            dialogue.SetActive(false);
            GameManager.instance.SetTalking(false);
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
            case "Soldier":
                TalkWithSoldier();
                break;
            case "Artist":
                TalkWithArtist();
                break;
            default:
                Debug.Log("PERSONAGGIO INESISTENTE");
                break;
        }
    }

    private void TalkWithFisherman()
    {
        //cambia immagine personaggio
        characterName.text = "PESCATORE";
        dialogueText.text = "Sono il pescatore, un vecchio rincoglionito che soffre di alzheimer.";
        continueText.SetActive(true);
        dialogue.SetActive(true);
        ready = false;
    }

    private void TalkWithRestaurateur()
    {
        //cambia immagine personaggio
        characterName.text = "RISTORATORE";
        dialogueText.text = "Sono il ristoratore, un uomo costretto a lavorare contro il proprio volere per mantenere la propria famiglia.";
        continueText.SetActive(true);
        dialogue.SetActive(true);
        ready = false;
    }

    private void TalkWithSoldier()
    {
        //cambia immagine personaggio
        characterName.text = "SERGENTE";
        dialogueText.text = "Sono il sergente, un uomo rigido e severo, estremamente attento alle regole.";
        continueText.SetActive(true);
        dialogue.SetActive(true);
        ready = false;
    }

    private void TalkWithArtist()
    {
        //cambia immagine personaggio
        characterName.text = "ARTISTA";
        dialogueText.text = "Sono l'artista, un uomo dallo spirito libero, che vive la vita nel modo che più gli piace.";
        continueText.SetActive(true);
        dialogue.SetActive(true);
        ready = false;
    }
}
