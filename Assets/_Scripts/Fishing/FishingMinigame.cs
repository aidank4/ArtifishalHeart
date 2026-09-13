using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;       

public class FishingMinigame : MonoBehaviour
{
    public Fish fish;
    public Fish[] allFish;

    [Header("Audio Variables")]
    public float reelingDonePitch = 1.3f;
    public float reelingPitch = 0.7f;
    public float losingPitch = 1.1f;

    [Header("Reel-in Variables")]
    public float fishMoveSpeed = 0.6f;
    public float reelInFishSpeed = 0.2f;
    public float loseFishSpeed = 0.5f;
    public float mouseDistanceToFishToCount = 50f;
    public float endingDelay = 0.3f;
    public float endingPopupDuration = 1f;
    public float catchAnimationDuration = 0.4f;
    public float mouseVisualSizeChangeSpeed = 2f;
    public float mouseLocalScaleOnFish = 0.45f;
    public float originalMouseLocalScale = 0.7f;
    public Color mouseColorReeling;
    public Color mouseColorLosing;

    [Header("Mouse Variables")]
    public float mouseSensitivity = 10f;
    public float mouseDeceleration = 1f;
    public float mouseMaxVelocity = 999f;
    public Vector2 maxOffset;

    [Header("Dependencies")]
    public Transform fishParent;
    public Transform hookParent;
    public Transform fishingLineOrigin;
    public Transform fishingLineVisual;
    public Transform mouseVisual;
    public SpriteRenderer mouseSprite;
    public SpriteRenderer fishSprite;
    public AudioSource audioSource;
    public GameObject fishCaughtPopup;
    public FishingResults fishingResults;

    [Header("Debug")]
    public float minigameCompletion = 0.5f;
    public Vector2 currentFishPosition;
    public Vector2 currentMousePosition;
    public Vector2 currentMouseVelocity;
    public Vector2 mouseDelta;
    public bool reelingInFish;
    public bool startedFishingEnd;

    public void StartGameRandomFish()
    {
        gameObject.SetActive(true);

        fish = allFish[Random.Range(0, allFish.Length)];

        StartGame();
    }

    private void StartGame()
    {
        audioSource.Play();
        fishCaughtPopup.SetActive(false);

        HideCursor(true);

        var randomStartingPosition = new Vector2(
            Random.Range(-maxOffset.x, maxOffset.x),
            Random.Range(-maxOffset.y, maxOffset.y)
        );

        currentFishPosition = randomStartingPosition;
        currentMousePosition = randomStartingPosition;

        fishSprite.sprite = fish.sprite;
        fishSprite.color = fish.color;

        minigameCompletion = 0.5f;

        RotateHookVisual();
            
        RotateFishingLineVisual();

        startedFishingEnd = false;

        fishParent.localPosition = currentFishPosition;
        mouseVisual.localPosition = currentMousePosition;
    }

    private void Update()
    {
        if(minigameCompletion < 1f) 
        {
            MoveFish_PerlinNoise();
        
            MoveMouse();

            RotateHookVisual();
            
            RotateFishingLineVisual();

            // Reel in fish if mouse is close enough
            reelingInFish = Vector2.Distance(currentMousePosition, currentFishPosition) < mouseDistanceToFishToCount;
            float minigameCompletionSpeed = reelingInFish ? reelInFishSpeed : -loseFishSpeed;
            minigameCompletion += minigameCompletionSpeed * Time.deltaTime;
            minigameCompletion = Mathf.Clamp01(minigameCompletion);

            mouseSprite.color = reelingInFish ? mouseColorReeling : mouseColorLosing;

            audioSource.pitch = reelingInFish ? reelingPitch : losingPitch;

            Vector3 targetMouseSize = (reelingInFish ? mouseLocalScaleOnFish :  originalMouseLocalScale) * Vector3.one;
            mouseVisual.localScale = Vector3.Lerp(mouseVisual.localScale, targetMouseSize, Time.deltaTime * mouseVisualSizeChangeSpeed);
        }
        else
        {
            if(!startedFishingEnd)
            {
                startedFishingEnd = true;
                StartCoroutine(FishCaught_cr());

                HideCursor(false);
            }
        }
    }

    void HideCursor(bool hide)
    {
        // Lock the cursor to the center of the screen and hide it 
        Cursor.lockState = hide ? CursorLockMode.Locked : CursorLockMode.None; 
        Cursor.visible = !hide; 
    }

    void MoveMouse()
    {
        // Decelerate mouse
        currentMouseVelocity = Vector2.Lerp(currentMouseVelocity, Vector2.zero, Time.deltaTime * mouseDeceleration);

        // Affect mouse velocity with mouse input
        mouseDelta = Mouse.current.delta.ReadValue();
        Vector2 inputVelocity = mouseDelta * mouseSensitivity;
        currentMouseVelocity += inputVelocity;
    
        // clamp velocity
        currentMouseVelocity = Vector2.ClampMagnitude(currentMouseVelocity, mouseMaxVelocity);

        // affect mouse position from its velocity
        currentMousePosition += currentMouseVelocity * Time.deltaTime;

        // Clamp mouse to be onscreen
        currentMousePosition = new Vector2(
            Mathf.Clamp(currentMousePosition.x, -maxOffset.x, maxOffset.x),
            Mathf.Clamp(currentMousePosition.y, -maxOffset.y, maxOffset.y)
        );

        mouseVisual.localPosition = currentMousePosition;
    }

    void RotateFishingLineVisual()
    {
        // point the base of the fishing line toward the fishing rod
        fishingLineVisual.up = fishingLineOrigin.position - fishingLineVisual.position;
    }

    void RotateHookVisual()
    {
        // point the hook toward the fishing rod
        hookParent.up = fishingLineOrigin.position - hookParent.position;
    }

    void MoveFish_PerlinNoise()
    {
        // 2 random noise values changing over time, mapped to the range: (-1, 1)
        float noiseX = (Mathf.PerlinNoise((Time.time * fishMoveSpeed), 2242.457678f) * 2f) - 1f;
        float noiseY = (Mathf.PerlinNoise((Time.time * fishMoveSpeed) + 4444.879432f, 9999.512321f) * 2) - 1f;

        currentFishPosition = new Vector2(noiseX * maxOffset.x, noiseY * maxOffset.y);

        fishParent.localPosition = currentFishPosition;
    }

    IEnumerator FishCaught_cr()
    {
        audioSource.Stop();

        fishCaughtPopup.SetActive(true);

        yield return new WaitForSeconds(endingDelay);

        audioSource.pitch = reelingDonePitch;
        audioSource.Play();

        float timeElapsed = 0f;

        Vector3 start = currentFishPosition;
        Vector3 end = fishingLineOrigin.localPosition;

        while(timeElapsed < catchAnimationDuration)
        {
            float completion = timeElapsed / catchAnimationDuration;
            currentFishPosition = Vector3.Lerp(start, end, completion);
            fishParent.localPosition = currentFishPosition;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        audioSource.Stop();

        yield return new WaitForSeconds(endingPopupDuration);

        fishCaughtPopup.SetActive(false);

        gameObject.SetActive(false);
    }
}
