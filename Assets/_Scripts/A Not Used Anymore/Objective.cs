using Photon.Pun;
using System;
using UnityEngine;

/*
public enum ObjectiveType
{
    SingleDog,
    TwoDogs,

    ObjectiveTypeCount
}

[System.Serializable]
public struct VisualByType
{
    public GameObject obj;
    public ObjectiveType type;
}

public class Objective : MonoBehaviour
{
    PhotonView photonView;

    [SerializeField]
    ObjectiveType objectiveType;


    public int scoreCount = 100;
    public ObjectiveType ObjectiveType => objectiveType;

    public event Action OnTypeChanged;
    public event Action<Objective> OnObjectiveCollected;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void RemoveObjective()
    {
        OnObjectiveCollected?.Invoke(this);

        Destroy(gameObject);
    }



    [PunRPC]
    public void SetObjectiveTypeRPC(ObjectiveType type)
    {
        objectiveType = type;

        OnTypeChanged?.Invoke();
    }

    public void SetObjectiveType(ObjectiveType type)
    {
        photonView.RPC(nameof(SetObjectiveTypeRPC), RpcTarget.All, type);
    }
}
*/