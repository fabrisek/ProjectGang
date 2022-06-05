using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/SceneData", order = 1)]
public class SceneObject : ScriptableObject
{
    public string MapName;
    public int IndexScene;
    public float[] TimeStar;
    public Sprite spriteLevel;
    public Sprite BackGroundLoad;
}