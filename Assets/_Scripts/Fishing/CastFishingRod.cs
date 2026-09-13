using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;       

public class CastFishingRod : MonoBehaviour
{
    [Header("Vars")]
    public float timetoCatchFishBobber = 0.4f;
    public float castLineDuration = 1f;
    public float castLineHeight = 3f;
    public AnimationCurve castHeightCurve;
    public Vector2 timeToNextFishRange = new Vector2(2f, 5f);

    [Header("Dependencies")]
    public Transform fishingLineOrigin;
    public Transform line;
    public Transform bobber;
    public Transform fisherModel;
    public GameObject fishAvailablePopup;
    public FishingMinigame fishingMinigame;
    public PlayerControls playerMovement;

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

        bool clicked = _castAction.action.WasPressedThisFrame();

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
            FishingOver();
        }else

        if(lineIsCast && !fishOnLine)
        {
            timeSinceLineCast += Time.deltaTime;

            //FISH CAUGHT
            if(timeSinceLineCast > timeForNextFish){
                fishAvailable = true;

                if(timeSinceLineCast > timeForNextFish + timetoCatchFishBobber)
                {
                    fishAvailable = false;
                    timeSinceLineCast = 0f;
                    SetRandomNextFishTime();
                }

                if(clicked)
                {
                    fishingMinigame.StartGameRandomFish();
                    fishOnLine = true;
                    fishAvailable = false;
                }

                Debug.Log("fishtyy!@!!!");
            }
            
        }else

        // FISHING OVER
        if(fishOnLine && !fishingMinigame.transform.gameObject.activeSelf)
        {
            FishingOver();
        }

        if(!lineIsCast)
        {
            timeSinceLineCast = 0f;
        }

    
        fishAvailablePopup.SetActive(fishAvailable);
        line.gameObject.SetActive(bobber.gameObject.activeSelf);

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

    IEnumerator CastLine_cr(bool retract)
    {
        bobber.gameObject.SetActive(true);

        castingLineAnimating = true;

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

        castingLineAnimating = false;

        if(retract)
        {
            bobber.gameObject.SetActive(false);
        }
    }

    void FishingOver()
    {
        lineIsCast = false;
            fishOnLine = false;
            playerMovement.enabled = true;
            SetRandomNextFishTime(); 
            StartCoroutine(CastLine_cr(true));
            Debug.Log("fishing OVERRERR");
    }
}
