using UnityEngine;

[CreateAssetMenu(fileName = "DogData", menuName = "DogData")]
public class DogData : ScriptableObject
{
    public string dogName;
    public Sprite dogSprite;
    public GameObject dogObject;

    public ObjectiveType objectiveType;
}
