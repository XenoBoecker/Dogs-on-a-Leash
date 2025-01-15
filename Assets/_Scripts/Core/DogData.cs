using UnityEngine;

[CreateAssetMenu(fileName = "DogData", menuName = "DogData")]
public class DogData : ScriptableObject
{
    public enum DogColor
    {
        Blue,
        Green,
        Red,
        Yellow
    }

    public int id;

    public Sprite dogSprite;
    public GameObject[] dogObjects;
}
