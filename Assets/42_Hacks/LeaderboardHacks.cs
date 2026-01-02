using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardHacks : MonoBehaviour
{
    LeaderboardManager lbManager;
    // Start is called before the first frame update
    void Start()
    {
        lbManager = FindObjectOfType<LeaderboardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            lbManager.ResetScores();
        }
        
    }
}
