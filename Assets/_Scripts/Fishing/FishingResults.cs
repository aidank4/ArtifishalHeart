using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishingResults : MonoBehaviour
{
    public GameObject canvas;
    public GameObject inventorycanvas;
    public Image fishIcon;
    public TextMeshProUGUI fishName1;
    public TextMeshProUGUI fishName2;
    [Space]
    public Image fishIconInInventory;
    public TextMeshProUGUI fishName1InInventory;
    public TextMeshProUGUI fishName2InInventory;
    [Space]
    public static Fish currentFishHeld;
    public Fish currentFishInspected;

    public bool showing = false;

    public void ShowResults(Fish fish)
    {
        currentFishInspected = fish;
        
        canvas.SetActive(true);

        fishIcon.sprite = fish.sprite;
        fishIcon.color = fish.color;

        fishName1.text = fish.name;
        fishName2.text = fish.name;

        showing = true;
    }

    public void KeepFish()
    {
        SetCurrentFish(currentFishInspected);
        canvas.SetActive(false);
        inventorycanvas.SetActive(true);

        showing = false;
    }

    public void ReleaseFish()
    {
        canvas.SetActive(false);

        showing = false;
    }

    void SetCurrentFish(Fish fish)
    {
        fishIconInInventory.sprite = fish.sprite;
        fishIconInInventory.color = fish.color;

        fishName1InInventory.text = fish.name;
        fishName2InInventory.text = fish.name;

        currentFishHeld = fish;
    }
}
