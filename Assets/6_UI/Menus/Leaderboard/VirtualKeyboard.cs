using System;
using TMPro;
using UnityEngine;

public class VirtualKeyboard : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;

    private bool isCapsLockOn = false;


    public event Action<bool> OnCapsLockToggled;

    public static VirtualKeyboard Instance;

    private void Awake()
    {
        Instance = this;
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
