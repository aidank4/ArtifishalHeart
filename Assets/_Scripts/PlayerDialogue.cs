using System.Collections;
using TMPro;
using UnityEngine;
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
}
