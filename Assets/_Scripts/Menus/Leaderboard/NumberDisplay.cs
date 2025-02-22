﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NumberDisplay : MonoBehaviour
{
    public GameObject digitPrefab;  // Prefab containing the sprite renderer for a digit
    public List<Sprite> numberSprites;  // List of sprites for digits 0-9

    private List<GameObject> digitObjects = new List<GameObject>();


    [SerializeField] List<int> specialNumbers = new List<int>();

    [SerializeField] List<Sprite> specialNumberSprites = new List<Sprite>();

    public void SetNumber(int number)
    {
        if(numberSprites.Count == 0) return;

        // Clear existing digits
        foreach (GameObject digitObject in digitObjects)
        {
            Destroy(digitObject);
        }
        digitObjects.Clear();

        if (specialNumbers.Contains(number))
        {// Instantiate a new digit object
            GameObject digitObject = Instantiate(digitPrefab, transform);

            // Set the sprite to the corresponding digit sprite
            Image image = digitObject.GetComponent<Image>();
            image.sprite = specialNumberSprites[specialNumbers.IndexOf(number)];
            return;
        }


        // Convert number to string to process each digit
        string numberStr = number.ToString();

        // Create and place new digit sprites
        for (int i = 0; i < numberStr.Length; i++)
        {
            char digitChar = numberStr[i];
            int digit = int.Parse(digitChar.ToString());

            // Instantiate a new digit object
            GameObject digitObject = Instantiate(digitPrefab, transform);

            // Set the sprite to the corresponding digit sprite
            Image image = digitObject.GetComponent<Image>();

            image.sprite = numberSprites[digit];

            // Add to list of digit objects
            digitObjects.Add(digitObject);
        }
    }
}