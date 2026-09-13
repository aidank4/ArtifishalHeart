using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    private int _index = 0;
    private int _choiceIndex = 0;
    private int _fishIndex = 0;


    public static int dayNum = 1;
    public static int score = 0;

    public bool playerDialogue = false;

    private bool _dateOver = false;
    public bool fishTime = false;


    public Fish datingFish;

    private void Awake()
    {
        //ref
        _playerDialogueScript = GetComponent<PlayerDialogue>();

        //starting active UI
        _dialogueBox.gameObject.SetActive(true);
        _playerDialogueScript.optionsBox.gameObject.SetActive(false);

        _textLover.text = string.Empty;

        if (FishingResults.currentFishHeld != null)
            datingFish = FishingResults.currentFishHeld;

        StartDialogue();
    }

    private void StartDialogue()
    {
        switch (dayNum)
        {
            case 1:
                _index = 0;
                _fishIndex = 0;
                _choiceIndex = 0;
                break;
            case 2:
                _index = 10;
                break;
            case 3:
                _index = 25;
                break;
            default:
                _index = 1;
                break;
        }

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
        if (dialogueLines[_index].fish)
        {
            fishTime = true;
        }
    }

    public void NextLine()
    {
        if (_index < dialogueLines.Length - 1 && !playerDialogue && !_playerDialogueScript.choiceSelected && !fishTime) // if its a question or a choice has been selecteddont type line
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
            //CHANGE SCENE
            SceneManager.LoadScene("GameScene");
            dayNum++; //increment dayNum
        }

        if (fishTime)
        {
            Debug.Log("FishTime");
            _index++;
            _textLover.text = string.Empty;
            fishTime = false;
            if (datingFish.liked)
            {
                dialogueLines[_index].text = fishResponsesGood[_fishIndex];
                _fishIndex++;
            }
            if (!datingFish.liked)
            {
                dialogueLines[_index].text = fishResponsesBad[_fishIndex];
                _fishIndex++;
            }

            StartCoroutine(TypeLine());
        }

        //Update text to reflect choice
        if (_playerDialogueScript.choiceSelected == true)
        {
            _playerDialogueScript.choiceSelected = false; // reset this bool
            //Set text to response
            _index++;
            _textLover.text = string.Empty;
            dialogueLines[_index].text = _playerDialogueScript.buttonText.text; // befure UI incase it going inactive matters

            //Configure following Line to be dynamic response
            if (_playerDialogueScript.likedChoice)
            {
                dialogueLines[_index + 1].text = choiceResponsesGood[_choiceIndex];
                _choiceIndex++;
            }
            else if (!_playerDialogueScript.likedChoice)
            {
                dialogueLines[_index + 1].text = choiceResponsesBad[_choiceIndex];
                _choiceIndex++;
            }

            //reconfigure UI
            _dialogueBox.gameObject.SetActive(true);
            _playerDialogueScript.optionsBox.gameObject.SetActive(false);

            StartCoroutine(TypeLine());
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
                if (dialogueLines[_index].fish)
                {
                    fishTime = true;
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
