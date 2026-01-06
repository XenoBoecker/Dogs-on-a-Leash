using TMPro;
using UnityEngine;

public class LetterCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private TMP_InputField nameInputField;

    private int counter;
    private int maxLetters = 10;

    // Start is called before the first frame update
    void Start()
    {
        maxLetters = nameInputField.characterLimit;
    }

    // Update is called once per frame
    void Update()
    {
        counter = nameInputField.text.Length;

        counterText.text = counter.ToString() + "/" + maxLetters.ToString();
    }
}
