using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualKeyboard : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;

    [SerializeField] private GameObject nameInputStartSelected;

    private bool isCapsLockOn = false;


    public event Action<bool> OnCapsLockToggled;

    public static VirtualKeyboard Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("VirtualKeyboard: Subscribing to GameOver OnShowLeaderboard event");
        SetStartSelectedKeyboardKey();
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(Input.inputString))
        {

            Debug.Log("Keyboard input detected, selecting name input field");
            EventSystem.current.SetSelectedGameObject(nameInputField.gameObject);

            if (nameInputField.text.Length < 1)
            {
                nameInputField.text += Input.inputString;
                nameInputField.caretPosition = nameInputField.text.Length;
            }

        }
    }

    private void SetStartSelectedKeyboardKey()
    {
        Debug.Log("Setting selected game object to name input start selected");
        EventSystem.current.SetSelectedGameObject(nameInputStartSelected);
    }

    public void LetterEntered(string letter)
    {
        if (isCapsLockOn)
        {
            letter = letter.ToUpper();
        }
        nameInputField.text += letter;
    }

    public void BackspacePressed()
    {
        if (nameInputField.text.Length > 0)
        {
            nameInputField.text = nameInputField.text.Substring(0, nameInputField.text.Length - 1);
        }
    }

    public void CapsLockToggled()
    {
        isCapsLockOn = !isCapsLockOn;

        OnCapsLockToggled?.Invoke(isCapsLockOn);
    }
}
