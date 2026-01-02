using Photon.Pun;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{

    [SerializeField] GameObject human;

    public void Setup()
    {
        if (PhotonNetwork.IsConnected) PhotonNetwork.Instantiate(nameof(human), Vector3.zero, Quaternion.identity);
        else Instantiate(human, Vector3.zero, Quaternion.identity);
    }
}
