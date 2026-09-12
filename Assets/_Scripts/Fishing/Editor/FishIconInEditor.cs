using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Fish))]
public class FishIconInEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Fish fish = (Fish)target;

        if (fish == null || fish.sprite == null || fish.sprite.texture == null)
        {
            return base.RenderStaticPreview(assetPath, subAssets, width, height);
        }

        // Get the texture from the assigned sprite
        Texture2D sourceTex = fish.sprite.texture;

        // Create a temporary RenderTexture to safely sample and scale the pixels
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        // Blit (copy) the source texture onto our sized render texture
        Graphics.Blit(sourceTex, rt);

        // Create the final custom icon texture
        Texture2D customIcon = new Texture2D(width, height, TextureFormat.RGBA32, false);
        customIcon.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        customIcon.Apply();

        // Clean up the temporary render textures
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        // Get all pixels from the scaled texture
        Color[] pixels = customIcon.GetPixels();

        // Multiply every pixel by the Fish ScriptableObject's color value
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] *= fish.color; 
        }

        // Apply the tinted pixels back to the thumbnail
        customIcon.SetPixels(pixels);
        customIcon.Apply();

        return customIcon;
    }
}