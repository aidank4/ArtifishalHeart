using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerDialogue : MonoBehaviour
{
    [System.Serializable]
    public struct DialogueOptions
    {
        public string text;
        public bool goodResponse;
    }

    //Can have an array of these for each question
    public DialogueOptions[] optionsChoiceOne;
    public DialogueOptions[] optionsChoiceTwo;

    public RawImage optionsBox;

    [SerializeField] private TMP_Text _textOptionA;
    [SerializeField] private TMP_Text _textOptionB;
    [SerializeField] private TMP_Text _textOptionC;
    [SerializeField] private TMP_Text _textOptionD;

    private DialogueController _dialogueControllerScript;

    public bool choiceSelected = false;
    public bool likedChoice;


    //this variable is accessed by dialogue script to use button text as response text
    public TMP_Text buttonText;

    private void Awake()
    {
        _dialogueControllerScript = GetComponent<DialogueController>();
    }


    //Set options text to whatever is in the options array per each date
    public void UpdateOptions()
    {
        switch (_dialogueControllerScript.dateNum)
        {
            case 1:
                _textOptionA.text = optionsChoiceOne[0].text;
                _textOptionB.text = optionsChoiceOne[1].text;
                _textOptionC.text = optionsChoiceOne[2].text;
                _textOptionD.text = optionsChoiceOne[3].text;
                break;

            case 2:
                _textOptionA.text = optionsChoiceTwo[0].text;
                _textOptionB.text = optionsChoiceTwo[1].text;
                _textOptionC.text = optionsChoiceTwo[2].text;
                _textOptionD.text = optionsChoiceTwo[3].text;
                break;
            default:
                Debug.Log("out of dates");
                break;
        }
    }


    public void ChoiceSelection()
    {
        //these bools control what the next line text is
        _dialogueControllerScript.playerDialogue = false;
        choiceSelected = true;

        GameObject button = EventSystem.current.currentSelectedGameObject;
        buttonText = button.GetComponentInChildren<TMP_Text>();
        Debug.Log(buttonText.text);

        ChoiceResponse();
        //Start text again
        _dialogueControllerScript.NextLine();
    }




    //Switch won't accept TMPTEXT so shitty switch
    public void ChoiceResponse()
    {
        if (buttonText.text == _textOptionA.text)
        {
            likedChoice = optionsChoiceOne[0].goodResponse;
        }
        else if (buttonText.text == _textOptionB.text)
        {
            likedChoice = optionsChoiceOne[1].goodResponse;
        }
        else if (buttonText.text == _textOptionC.text)
        {
            likedChoice = optionsChoiceOne[2].goodResponse;
        }
        else if (buttonText.text == _textOptionD.text)
        {
            likedChoice = optionsChoiceOne[3].goodResponse;
        }
    }

}
