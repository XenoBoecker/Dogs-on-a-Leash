using TMPro;
using UnityEngine;

public class VirtualKeyboardKey : MonoBehaviour
{
    [SerializeField] private string keyValue; // The value this key represents
    [SerializeField] private TMP_Text keyText;

    private void Start()
    {
        VirtualKeyboard.Instance.OnCapsLockToggled += HandleCapsLockToggled;

        HandleCapsLockToggled(false);
    }

    public void OnKeyPressed()
    {
        VirtualKeyboard.Instance.LetterEntered(keyValue);
    }

    private void HandleCapsLockToggled(bool isCapsLocked)
    {
        keyText.text = isCapsLocked ? keyValue.ToUpper() : keyValue.ToLower();
    }

    private void OnDestroy()
    {
        VirtualKeyboard.Instance.OnCapsLockToggled -= HandleCapsLockToggled;
    }
}
