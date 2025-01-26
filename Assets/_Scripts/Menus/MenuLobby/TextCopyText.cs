using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextCopyText : MonoBehaviour
{

    [SerializeField] TMP_Text textToCopy;

    TMP_Text myText;
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = ModifiedText(textToCopy.text);
    }
    private string ModifiedText(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        // Ensure the string has at least 6 characters for consistent formatting
        if (text.Length > 7)
        {
            return text.Insert(6, "-").Insert(3, "-");
        }
        else if (text.Length > 4)
        {
            return text.Insert(3, "-");
        }

        return text;
    }
}
