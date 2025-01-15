using photonMenuLobby;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyShowDog : MonoBehaviour
{
    LobbyDogSelector lobbyDogSelector;


    [SerializeField] int colorIndex;

    [SerializeField] Transform dogModelParent;
    GameObject currentDogModel;

    [SerializeField] Image bgImage;
    [SerializeField] TextMeshProUGUI dogNameText;

    internal void SetPlayerData(PlayerData playerData)
    {
        
    }

    private void Awake()
    {
        lobbyDogSelector = GetComponent<LobbyDogSelector>();
    }


    private void Start()
    {
        lobbyDogSelector.OnDataChanged += UpdateUI;

        Invoke("UpdateUI", 0.1f);
    }

    private void UpdateUI()
    {
        DogData dogData = lobbyDogSelector.GetDogData();

        dogNameText.text = dogData.name;

        if(currentDogModel != null) Destroy(currentDogModel);
        currentDogModel = Instantiate(dogData.dogObjects[colorIndex]);
        currentDogModel.transform.SetParent(dogModelParent);
        currentDogModel.transform.position = dogModelParent.transform.position;
    }
}