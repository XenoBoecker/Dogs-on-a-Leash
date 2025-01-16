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


    [SerializeField] GameObject selectAccessoriePanel;
    [SerializeField] GameObject showConfirmed;
    [SerializeField] TextMeshProUGUI dogNameText;

    private void Awake()
    {
        lobbyDogSelector = GetComponent<LobbyDogSelector>();

        selectAccessoriePanel.SetActive(false);
        showConfirmed.SetActive(false);
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

        if (lobbyDogSelector.IsSelectionConfirmed) selectAccessoriePanel.SetActive(true);
        else selectAccessoriePanel.SetActive(false);

        if (lobbyDogSelector.IsReadyToPlay) showConfirmed.SetActive(true);
        else showConfirmed.SetActive(false);
    }
}