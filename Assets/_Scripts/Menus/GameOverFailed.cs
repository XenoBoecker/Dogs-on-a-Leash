using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverFailed : MonoBehaviour
{
    [SerializeField] TMP_Text distanceText, pickupsText, bumpedText;

    // Start is called before the first frame update
    void Start()
    {
        distanceText.text = PlayerPrefs.GetInt("Distance").ToString() + " / " + PlayerPrefs.GetInt("LevelLength").ToString() + " m";
        pickupsText.text = PlayerPrefs.GetInt("PickupCount").ToString();
        bumpedText.text = PlayerPrefs.GetInt("BumpedCount").ToString();
    }
}
