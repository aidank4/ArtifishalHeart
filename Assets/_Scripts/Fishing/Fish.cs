using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "New Fish", menuName = "Fish")]
public class Fish : ScriptableObject{
    public Sprite sprite;
    public Color color = Color.white;
    public int cost = 10;
}
