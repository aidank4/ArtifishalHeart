using UnityEngine;
using UnityEngine.InputSystem;

public class FishingMinigame : MonoBehaviour
{
    [Header("Reel-in Variables")]
    public float reelInFishSpeed = 0.2f;
    public float loseFishSpeed = 0.5f;
    public float mouseDistanceToFishToCount = 50f;
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

    [Header("Debug")]
    public float minigameCompletion = 0.5f;
    public Vector2 currentFishPosition;
    public Vector2 currentMousePosition;
    public Vector2 currentMouseVelocity;
    public Vector2 mouseDelta;

    private void Start()
    {
        HideCursor();

        var randomStartingPosition = new Vector2(
            Random.Range(-maxOffset.x, maxOffset.x),
            Random.Range(-maxOffset.y, maxOffset.y)
        );

        currentFishPosition = randomStartingPosition;
        currentMousePosition = randomStartingPosition;
    }

    private void Update()
    {
        MoveFish_PerlinNoise();
        
        MoveMouse();

        RotateHookVisual();
        
        RotateFishingLineVisual();

        // Reel in fish if mouse is close enough
        bool reelingInFish = Vector2.Distance(currentMousePosition, currentFishPosition) < mouseDistanceToFishToCount;
        float minigameCompletionSpeed = reelingInFish ? reelInFishSpeed : -loseFishSpeed;
        minigameCompletion += minigameCompletionSpeed * Time.deltaTime;
        minigameCompletion = Mathf.Clamp01(minigameCompletion);

        mouseSprite.color = reelingInFish ? mouseColorReeling : mouseColorLosing;
    }

    void HideCursor()
    {
        // Lock the cursor to the center of the screen and hide it 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
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

        mouseVisual.position = currentMousePosition;
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
        float noiseX = (Mathf.PerlinNoise(Time.time, 2242.457678f) * 2f) - 1f;
        float noiseY = (Mathf.PerlinNoise(Time.time + 4444.879432f, 9999.512321f) * 2) - 1f;

        currentFishPosition = new Vector2(noiseX * maxOffset.x, noiseY * maxOffset.y);

        fishParent.localPosition = currentFishPosition;
    }
}
