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
    public DialogueOptions[] optionsChoiceThree;
    public DialogueOptions[] optionsChoiceFour;
    public DialogueOptions[] optionsChoiceFive;

    public RawImage optionsBox;

    [SerializeField] private TMP_Text _textOptionA;
    [SerializeField] private TMP_Text _textOptionB;
    [SerializeField] private TMP_Text _textOptionC;
    [SerializeField] private TMP_Text _textOptionD;

    private DialogueController _dialogueControllerScript;

    public bool choiceSelected = false;
    public bool likedChoice;

    private int choiceNum = 1;

    //this variable is accessed by dialogue script to use button text as response text
    public TMP_Text buttonText;

    private void Awake()
    {
        _dialogueControllerScript = GetComponent<DialogueController>();

        //sets up choice num so correct choices come up for each date
        SetCounter();
    }


    //Set options text to whatever is in the options array per each date
    public void UpdateOptions()
    {
        switch (choiceNum) // change to choice num
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
            case 3:
                _textOptionA.text = optionsChoiceThree[0].text;
                _textOptionB.text = optionsChoiceThree[1].text;
                _textOptionC.text = optionsChoiceThree[2].text;
                _textOptionD.text = optionsChoiceThree[3].text;
                break;
            case 4:
                _textOptionA.text = optionsChoiceFour[0].text;
                _textOptionB.text = optionsChoiceFour[1].text;
                _textOptionC.text = optionsChoiceFour[2].text;
                _textOptionD.text = optionsChoiceFour[3].text;
                break;
            case 5:
                _textOptionA.text = optionsChoiceFive[0].text;
                _textOptionB.text = optionsChoiceFive[1].text;
                _textOptionC.text = optionsChoiceFive[2].text;
                _textOptionD.text = optionsChoiceFive[3].text;
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


        //plug in correct choice response results
        switch (choiceNum)
        {
            case 1:
                ChoiceResponseA();
                break;
            case 2:
                ChoiceResponseB();
                break;
            case 3:
                ChoiceResponseC();
                break;
            case 4:
                ChoiceResponseD();
                break;
            case 5:
                ChoiceResponseE();
                break;
            default:
                Debug.Log("out of dates");
                break;
        }

        //Start text again
        _dialogueControllerScript.NextLine();

        choiceNum++;
    }




    //Switch won't accept TMPTEXT so shitty switch
    public void ChoiceResponseA()
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

    /// <summary>
    /// These choice responses just tell whether it was a good or bad response by setting a bool
    /// </summary>

    public void ChoiceResponseB()
    {
        if (buttonText.text == _textOptionA.text)
        {
            likedChoice = optionsChoiceTwo[0].goodResponse;
        }
        else if (buttonText.text == _textOptionB.text)
        {
            likedChoice = optionsChoiceTwo[1].goodResponse;
        }
        else if (buttonText.text == _textOptionC.text)
        {
            likedChoice = optionsChoiceTwo[2].goodResponse;
        }
        else if (buttonText.text == _textOptionD.text)
        {
            likedChoice = optionsChoiceTwo[3].goodResponse;
        }
    }

    public void ChoiceResponseC()
    {
        if (buttonText.text == _textOptionA.text)
        {
            likedChoice = optionsChoiceThree[0].goodResponse;
        }
        else if (buttonText.text == _textOptionB.text)
        {
            likedChoice = optionsChoiceThree[1].goodResponse;
        }
        else if (buttonText.text == _textOptionC.text)
        {
            likedChoice = optionsChoiceThree[2].goodResponse;
        }
        else if (buttonText.text == _textOptionD.text)
        {
            likedChoice = optionsChoiceThree[3].goodResponse;
        }
    }

    public void ChoiceResponseD()
    {
        if (buttonText.text == _textOptionA.text)
        {
            likedChoice = optionsChoiceFour[0].goodResponse;
        }
        else if (buttonText.text == _textOptionB.text)
        {
            likedChoice = optionsChoiceFour[1].goodResponse;
        }
        else if (buttonText.text == _textOptionC.text)
        {
            likedChoice = optionsChoiceFour[2].goodResponse;
        }
        else if (buttonText.text == _textOptionD.text)
        {
            likedChoice = optionsChoiceFour[3].goodResponse;
        }
    }

    public void ChoiceResponseE()
    {
        if (buttonText.text == _textOptionA.text)
        {
            likedChoice = optionsChoiceFive[0].goodResponse;
        }
        else if (buttonText.text == _textOptionB.text)
        {
            likedChoice = optionsChoiceFive[1].goodResponse;
        }
        else if (buttonText.text == _textOptionC.text)
        {
            likedChoice = optionsChoiceFive[2].goodResponse;
        }
        else if (buttonText.text == _textOptionD.text)
        {
            likedChoice = optionsChoiceFive[3].goodResponse;
        }
    }


    private void SetCounter()
    {
        switch(DialogueController.dayNum)
        {
            case 1:
                choiceNum = 1;
                break;
            case 2:
                choiceNum = 2;
                break;
            case 3:
                choiceNum = 4;
                break;
            default:
                Debug.Log("out of dates");
                break;

        }
    }

}
