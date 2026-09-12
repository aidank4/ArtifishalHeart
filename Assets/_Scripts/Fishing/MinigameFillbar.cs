using UnityEngine;
using UnityEngine.UI;

public class MinigameFillbar : MonoBehaviour
{
    public Color zeroCompletionColor;
    public Color halfCompletionColor;
    public Color fullCompletionColor;
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

        fillImage.color = GetBarColor(minigame.minigameCompletion);
    }

    // Goes from Red to Yellow to Green (0 - 1)
    Color GetBarColor(float completion)
    {
        if(completion < 0.5f){
            return Color.Lerp(zeroCompletionColor, halfCompletionColor, completion * 2f);
        }else{
            return Color.Lerp(halfCompletionColor, fullCompletionColor, (completion - 0.5f) * 2f);
        }
    }
}
