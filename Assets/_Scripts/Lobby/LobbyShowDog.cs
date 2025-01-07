using photonMenuLobby;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyShowDog : MonoBehaviour
{
    LobbyDogSelector lobbyDogSelector;


    [SerializeField] Image bgImage;
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] TMPro.TextMeshProUGUI dogNameText;
    [SerializeField] UnityEngine.UI.Image dogImage;

    internal void SetPlayerData(PlayerData playerData)
    {
        throw new NotImplementedException();
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
        playerNameText.text = lobbyDogSelector.PlayerName;
        
        if(lobbyDogSelector.IsPlayerControlled)
        {
            if (lobbyDogSelector.IsSelectionConfirmed) bgImage.color = Color.green;
            else bgImage.color = new Color(1, 0.7f, 0);
        }
        else
        {
            bgImage.color = Color.white;
        }

        DogData dogData = lobbyDogSelector.GetDogData();

        dogNameText.text = dogData.name;
        dogImage.sprite = dogData.dogSprite;
    }
}