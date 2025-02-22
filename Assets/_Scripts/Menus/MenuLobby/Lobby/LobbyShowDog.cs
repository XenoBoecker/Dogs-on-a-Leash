﻿using photonMenuLobby;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyShowDog : MonoBehaviour
{
    LobbyDogSelector lobbyDogSelector;

    public float DogScale = 1;

    [SerializeField] int colorIndex;

    [SerializeField] Transform dogModelParent;

    [SerializeField] float rotSpeed;
    bool isRotating = true;

    [SerializeField] GameObject menuDogPrefab;
    GameObject currentDogModel;


    [SerializeField] GameObject dogSelectArrows;
    [SerializeField] GameObject selectAccessoriePanel;
    [SerializeField] GameObject showConfirmed;
    [SerializeField] TextMeshProUGUI dogNameText;

    [SerializeField] TMP_Text accessoryNameText;

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

    private void Update()
    {
        if(isRotating) dogModelParent.transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
    }

    private void UpdateUI()
    {
        DogData dogData = lobbyDogSelector.GetDogData();

        if(dogNameText != null)dogNameText.text = dogData.name;

        // TODO: dont need to destroy every time

        if (currentDogModel == null)
        {
            currentDogModel = Instantiate(menuDogPrefab);
            currentDogModel.transform.localScale = new Vector3(DogScale, DogScale, DogScale);
            currentDogModel.transform.SetParent(dogModelParent);
            currentDogModel.transform.position = dogModelParent.transform.position;

            currentDogModel.transform.Rotate(Vector3.up, 180);
        }
        DogVisuals visuals = currentDogModel.GetComponentInChildren<DogVisuals>();

        visuals.SetDogID(dogData.id);
        visuals.SetColorIndex(colorIndex);
        visuals.SetAccessorieIndex(lobbyDogSelector.CurrentSelectedAccessorieIndex);
        accessoryNameText.text = visuals.CurrentAccessory.name;

        if (lobbyDogSelector.IsSelectionConfirmed)
        {
            selectAccessoriePanel.SetActive(true);
            dogSelectArrows.SetActive(false);
            dogModelParent.transform.rotation = Quaternion.identity;
            isRotating = false;
        }
        else
        {
            selectAccessoriePanel.SetActive(false);
            dogSelectArrows.SetActive(true);
            isRotating = true;
        }

        if (lobbyDogSelector.IsReadyToPlay)
        {
            selectAccessoriePanel.SetActive(false);
            showConfirmed.SetActive(true);
        }
        else
        {
            showConfirmed.SetActive(false);
        }
    }
}