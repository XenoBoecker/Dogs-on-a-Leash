using UnityEngine;
using UnityEngine.UI;

public class FixedScrollbar : MonoBehaviour
{
    public Scrollbar scrollbar;
    [Range(0.05f, 1f)] public float fixedHandleSize = 0.2f; // Adjust handle size manually

    void Start()
    {
        if (scrollbar)
            scrollbar.size = fixedHandleSize; // Set the handle size once
    }

    void LateUpdate()
    {
        if (scrollbar)
            scrollbar.size = fixedHandleSize; // Prevents automatic resizing
    }
}
