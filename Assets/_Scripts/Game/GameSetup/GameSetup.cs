using UnityEngine;

public class GameSetup : MonoBehaviour
{
    MapManager mm;
    HumanSpawner hs;
    OnlineDogSpawner ds;
    HumanConnectionManager hcm;

    CameraMovement cm;
    HumanProgressBar hpb;

    private void Awake()
    {
        mm = FindObjectOfType<MapManager>();
        hs = FindObjectOfType<HumanSpawner>();
        ds = FindObjectOfType<OnlineDogSpawner>();
        hcm = FindObjectOfType<HumanConnectionManager>();

        cm = FindObjectOfType<CameraMovement>();
        hpb = FindObjectOfType<HumanProgressBar>();

        if (mm != null) mm.Setup();
        if (hs != null) hs.Setup();
        if (hcm != null) hcm.Setup();
        if (ds != null) ds.Setup();
        if (cm != null) cm.Setup();
        if (hpb != null) hpb.Setup();
    }
}