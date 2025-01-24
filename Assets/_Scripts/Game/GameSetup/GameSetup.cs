using System;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    HumanSpawner hs;
    MapManager mm;
    OnlineDogSpawner ds;
    HumanConnectionManager hcm;

    HumanProgressBar hpb;
    CameraMovement cm;

    private void Awake()
    {
        hs = FindObjectOfType<HumanSpawner>();
        mm = FindObjectOfType<MapManager>();
        hcm = FindObjectOfType<HumanConnectionManager>();
        ds = FindObjectOfType<OnlineDogSpawner>();

        cm = FindObjectOfType<CameraMovement>();
        hpb = FindObjectOfType<HumanProgressBar>();

        if (hs != null) hs.Setup();
        else Debug.Log("No HumanSpawner found");
        if (mm != null) mm.Setup();
        else Debug.Log("No MapManager found");
        if (hcm != null) hcm.Setup();
        else Debug.Log("No HumanConnectionManager found");
        if (ds != null) ds.Setup();
        else Debug.Log("No OnlineDogSpawner found");
        if (hpb != null) hpb.Setup();
        else Debug.Log("No HumanProgressBar found");
        if (cm != null) cm.Setup();
        else Debug.Log("No CameraMovement found");


    }
}