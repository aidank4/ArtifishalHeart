using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;       

public class CastFishingRod : MonoBehaviour
{
    [Header("Vars")]
    public float boberDelay = 0.2f;
    public float timetoCatchFishBobber = 0.4f;
    public float castLineDuration = 1f;
    public float castLineHeight = 3f;
    public AnimationCurve castHeightCurve;
    public Vector2 timeToNextFishRange = new Vector2(2f, 5f);

    [Header("Dependencies")]
    public Animator animator;
    public Transform fishingLineOrigin;
    public Transform line;
    public Transform bobber;
    public Transform fisherModel;
    public GameObject fishAvailablePopup;
    public FishingMinigame fishingMinigame;
    public FishingResults fishingResults;
    public PlayerControls playerMovement;
    public AudioSource srcCast;
    public AudioSource srcBite;
    public AudioSource srcReel;
    public AudioSource srcClick;
    [Header("Debug")]
    public bool lineIsCast = false;
    public bool fishOnLine = false;
    public bool castingLineAnimating = false;
    public float timeSinceLineCast = 0f;
    public float timeForNextFish = 0f;
    public bool fishAvailable;

    public InputActionReference _castAction;

    void Start(){
        SetRandomNextFishTime();
            
        bobber.gameObject.SetActive(false);
    }

    void Update(){

        bool clicked = _castAction.action.WasPressedThisFrame() && !castingLineAnimating && !fishingResults.showing;

        // CAST THE LINE
        if(!castingLineAnimating && !lineIsCast && !fishingMinigame.transform.gameObject.activeSelf && clicked)
        {
            lineIsCast = true;

            playerMovement.enabled = false;

            Debug.Log("line IS CAST");

            StartCoroutine(CastLine_cr(false));
        }else

        // Retract cast line
        if(!castingLineAnimating && lineIsCast && !fishOnLine && clicked && !fishAvailable)
        {
            FishingOver(false);
        }else

        if(lineIsCast && !fishOnLine)
        {
            timeSinceLineCast += Time.deltaTime;

            //FISH CAUGHT
            if(timeSinceLineCast > timeForNextFish){
                if(!fishAvailable)
                {
                    PlaySFX(srcBite);
                }
                fishAvailable = true;

                if(timeSinceLineCast > timeForNextFish + timetoCatchFishBobber)
                {
                    fishAvailable = false;
                    timeSinceLineCast = 0f;
                    SetRandomNextFishTime();
                }

                //START
                if(clicked)
                {
                    fishingMinigame.StartGameRandomFish();
                    fishOnLine = true;
                    fishAvailable = false;

                    PlaySFX(srcClick);
                }

                Debug.Log("fishtyy!@!!!");
            }
            
        }else

        // FISHING WIN
        if(fishOnLine && !fishingMinigame.transform.gameObject.activeSelf)
        {
            FishingOver(fishingMinigame.caught);
        }

        if(!lineIsCast)
        {
            timeSinceLineCast = 0f;
        }

    
        fishAvailablePopup.SetActive(fishAvailable);
        line.gameObject.SetActive(bobber.gameObject.activeSelf);
    }

    void LateUpdate()
    {
        SetLine(fishingLineOrigin.position, bobber.position);
    }

    void SetLine(Vector3 a, Vector3 b)
    {
        line.localScale = new Vector3(line.localScale.x, line.localScale.y, Vector3.Distance(a, b));
        line.rotation = Quaternion.LookRotation((b - a).normalized, Vector3.up);
    }

    void SetRandomNextFishTime()
    {
        timeForNextFish = Random.Range(timeToNextFishRange.x, timeToNextFishRange.y);
    }

    IEnumerator CastLine_cr(bool retract, bool caught = false)
    {
        castingLineAnimating = true;

        animator.CrossFade(retract ? "Retract" : "Cast", 0.06f);

        yield return new WaitForSeconds(boberDelay);

        PlaySFX(retract ? srcReel : srcCast);

        bobber.gameObject.SetActive(true);

        Vector3 start = fishingLineOrigin.position;
        Vector3 end = transform.position + (fisherModel.forward * 10f);

        float timeElapsed = 0f;

        while(timeElapsed < castLineDuration)
        {
            float completion = timeElapsed / castLineDuration;
            bobber.position = Vector3.Lerp(retract ? end : start, retract ? start : end, completion);
            bobber.position += Vector3.up * castLineHeight * castHeightCurve.Evaluate(completion);

            timeElapsed += Time.deltaTime;
            yield return null;
        }


        if(retract)
        {
            bobber.gameObject.SetActive(false);
        }

        if(retract && caught    ) fishingResults.ShowResults(fishingMinigame.fish);

        castingLineAnimating = false;
    }

    void FishingOver(bool caught)
    {
        lineIsCast = false;
            fishOnLine = false;
            playerMovement.enabled = true;
            SetRandomNextFishTime(); 
            StartCoroutine(CastLine_cr(true, caught));
            Debug.Log("fishing OVERRERR");
    }

    void PlaySFX(AudioSource src)
    {
        src.PlayOneShot(src.clip);
    }
}
