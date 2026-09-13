using UnityEngine;
using UnityEngine.UI;

public class MinigameFillbar : MonoBehaviour
{
    public Color zeroCompletionColor;
    public Color halfCompletionColor;
    public Color fullCompletionColor;
    [Space]
    public Color zeroCompletionColor_losing;
    public Color halfCompletionColor_losing;
    public Color fullCompletionColor_losing;
    [Space]
    public FishingMinigame minigame;
    public RectTransform fillRect;
    public Image fillImage;

    private void LateUpdate()
    {
        fillRect.localScale = new Vector3(
            fillRect.localScale.x,
            minigame.minigameCompletion,
            fillRect.localScale.z
        );

        fillImage.color = GetBarColor(
            minigame.minigameCompletion,
            minigame.reelingInFish ? zeroCompletionColor : zeroCompletionColor_losing,
            minigame.reelingInFish ? halfCompletionColor : halfCompletionColor_losing,
            minigame.reelingInFish ? fullCompletionColor : fullCompletionColor_losing
        );
    }

    // Goes from Red to Yellow to Green (0 - 1)
    Color GetBarColor(float completion, Color c1, Color c2, Color c3)
    {
        if(completion < 0.5f){
            return Color.Lerp(c1, c2, completion * 2f);
        }else{
            return Color.Lerp(c2, c3, (completion - 0.5f) * 2f);
        }
    }
}
