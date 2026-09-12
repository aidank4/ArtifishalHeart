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
    public DialogueOptions[] optionsDateOne;
    public DialogueOptions[] optionsDateTwo;

    public RawImage optionsBox;

    [SerializeField] private TextMeshProUGUI _textOptionA;
    [SerializeField] private TextMeshProUGUI _textOptionB;
    [SerializeField] private TextMeshProUGUI _textOptionC;
    [SerializeField] private TextMeshProUGUI _textOptionD;

    private DialogueController _dialogueControllerScript;

    public bool choiceSelected = false;


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
                _textOptionA.text = optionsDateOne[0].text;
                _textOptionB.text = optionsDateOne[1].text;
                _textOptionC.text = optionsDateOne[2].text;
                _textOptionD.text = optionsDateOne[3].text;
                break;

            case 2:
                _textOptionA.text = optionsDateTwo[0].text;
                _textOptionB.text = optionsDateTwo[1].text;
                _textOptionC.text = optionsDateTwo[2].text;
                _textOptionD.text = optionsDateTwo[3].text;
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


        //Start text again
        _dialogueControllerScript.NextLine();
    }
}
