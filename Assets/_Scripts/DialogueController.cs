using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    //This code is so shitty
    //At the moment have array of lines that it goes through
    //plan to have it bump the index around depending on what date num is
    //for example - datenum = 1 index = 0, datenum = 3 index = 56
    //really crappy way to have all the text - definetly wont work
    


    [System.Serializable]
    public struct DialogueEntry
    {
        public string text;
        public bool question;
        public bool player;
        public bool fish;
        public bool finishesDate;
    }

    //struct array for dialogue lines
    public DialogueEntry[] dialogueLines;

    //string arrays for varying responses
    public string[] fishResponsesGood;
    public string[] fishResponsesBad;
    public string[] choiceResponsesGood;
    public string[] choiceResponsesBad;




    //references
    private PlayerDialogue _playerDialogueScript;

    [SerializeField] private TextMeshProUGUI _textLover;
    [SerializeField] private RawImage _dialogueBox;
    [SerializeField] private RawImage _playerSprite;
    [SerializeField] private RawImage _loverSprite;

    
    [SerializeField] private float _typeSpeed = 0.05f;
    [SerializeField] private int _index;

    public int dateNum = 1;

    public bool playerDialogue = false;

    private bool _dateOver = false;


    private void Awake()
    {
        //ref
        _playerDialogueScript = GetComponent<PlayerDialogue>();

        //starting active UI
        _dialogueBox.gameObject.SetActive(true);
        _playerDialogueScript.optionsBox.gameObject.SetActive(false);

        _textLover.text = string.Empty;
        _index = 0; // might need to change this to into switch
        StartDialogue();
    }

    private void StartDialogue()
    {
        //Maybe helpful down the line if _index does not save or glitches or something
        /*switch (dateNum)
        {
            case 1:
                _index = 0;
                break;
            case 2:
                _index = 10;
                break;
            case 3:
                _index = 25;
                break;
            default:
                _index = 0;
                break;
        }*/

        StartCoroutine(TypeLine());
    }


    /// <summary>
    /// Types out a line at a certain index in the array character by character
    /// </summary>
    /// <returns></returns>
    IEnumerator TypeLine()
    {
        SpriteSelector();
        foreach (char c in dialogueLines[_index].text.ToCharArray())
        {
            _textLover.text += c;
            yield return new WaitForSeconds(_typeSpeed);
        }
        //if question need to prompt player to make dialogue choice
        if (dialogueLines[_index].question)
        {
            Debug.Log("question");
            //time for player to choose response
            playerDialogue = true;
        }
        if (dialogueLines[_index].finishesDate)
        {
            _dateOver = true;
        }
    }

    public void NextLine()
    {
        if (_index < dialogueLines.Length - 1 && !playerDialogue && !_playerDialogueScript.choiceSelected) // if its a question or a choice has been selecteddont type line
        {
            _index++;
            _textLover.text = string.Empty;
            StartCoroutine(TypeLine());
        }

        if (playerDialogue)
        {
            //reconfigure UI
            _dialogueBox.gameObject.SetActive(false);
            _playerDialogueScript.optionsBox.gameObject.SetActive(true);
            _playerDialogueScript.UpdateOptions();

        }

        if (_dateOver)
        {
            _dateOver = false;
            this.gameObject.SetActive(false);
            //close UI
            //dateNum +=1;

            ///When player reopens date scene, StartDialogue needs to be called
        }

        //Update text to reflect choice
        if (_playerDialogueScript.choiceSelected == true)
        {
            _playerDialogueScript.choiceSelected = false; // reset this bool
            //Set text to response
            _index++;
            _textLover.text = string.Empty;
            dialogueLines[_index].text = _playerDialogueScript.buttonText.text; // befure UI incase it going inactive matters

            //reconfigure UI
            _dialogueBox.gameObject.SetActive(true);
            _playerDialogueScript.optionsBox.gameObject.SetActive(false);

            StartCoroutine(TypeLine());
        }

        if (dialogueLines[_index].fish)
        {
            /*if (FishManager.Fish.like == true)
            {
                _index++;
            _textLover.text = string.Empty;
            dialogueLines[_index].text =

            }
            _index++;
            _textLover.text = string.Empty;
            dialogueLines[_index].text = */
        }


    }


    public void SkipLine(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            if (_textLover.text == dialogueLines[_index].text)
            {
                Debug.Log("next");
                NextLine();

            }
            else
            {
                Debug.Log("assign");
                StopAllCoroutines();
                _textLover.text = dialogueLines[_index].text;

                //check this if statement incase they skip it
                if (dialogueLines[_index].question)
                {
                    Debug.Log("question");
                    //time for player to choose response
                    playerDialogue = true;
                }

                //Check for end of conversation
                if (dialogueLines[_index].finishesDate)
                {
                    _dateOver = true;
                }
            }
        }
    }


    //Can select if a line if said by player or lover and this will change the displayed sprite
    private void SpriteSelector()
    {
        if (dialogueLines[_index].player)
        {
            _playerSprite.gameObject.SetActive(true);
            _loverSprite.gameObject.SetActive(false);
        }
        else
        {
            _playerSprite.gameObject.SetActive(false);
            _loverSprite.gameObject.SetActive(true);
        }
    }














}
