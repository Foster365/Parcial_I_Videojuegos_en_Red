using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System;

public class NetManager : MonoBehaviourPunCallbacks
{

    [SerializeField] Button btnConnection;
    [SerializeField] TextMeshProUGUI connectionStatus;
    [SerializeField] TextMeshProUGUI roomName;
    [SerializeField] TextMeshProUGUI characterNickName;
    string playersMaxNumber = "2";
    string[] genericNicknames = { "Menem", "Chinchulancha", "SinNombre" };
    string genericNickName = "Puflito";
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //Esto hace que los jugadores cambien automáticamente a la misma escena
        //Al utilizar PhotonNetwork.LoadScene();
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        btnConnection.interactable = false;
        connectionStatus.text = "Connecting to Master";
        HandleNickname();
    }
    void HandleNickname()
    {
        Debug.Log("Dale forro estas entrando o no?");
        string defaultNickName = string.Empty;
        if (characterNickName != null)
        {
            if (PlayerPrefs.HasKey(genericNickName))
            {
                defaultNickName = PlayerPrefs.GetString(genericNickName);
                characterNickName.text = defaultNickName;
            }
        }

        //characterNickName.text = genericNicknames[UnityEngine.Random.Range(0, genericNicknames.Length - 1)];
        PhotonNetwork.NickName = defaultNickName;
        characterNickName.text = defaultNickName;
    }
    
    public void SetPlayerNickName(string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            Debug.Log("Character nickname input is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(genericNickName, value);
    }
    public override void OnConnectedToMaster()
    {
        btnConnection.interactable = false;
        PhotonNetwork.JoinLobby();
        connectionStatus.text = "Connecting to Lobby";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectionStatus.text = "Connection with master has failed, cause was: " + cause;
    }

    public override void OnJoinedLobby()
    {
        btnConnection.interactable = true;
        connectionStatus.text = "Connected to Lobby";
    }
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        connectionStatus.text = "Disconnected from lobby";
    }
    public void Connect()
    {

        if (string.IsNullOrEmpty(roomName.text) || string.IsNullOrWhiteSpace(roomName.text)) return;
        else if (string.IsNullOrEmpty(characterNickName.text) || string.IsNullOrWhiteSpace(characterNickName.text)) return;



        RoomOptions options = new RoomOptions();
        options.MaxPlayers = byte.Parse(playersMaxNumber);
        options.IsOpen = true;
        options.IsVisible = true;

        //playersMaxNumber = options.MaxPlayers;
        //TODO setear nickname

        Debug.Log("Room name:" + roomName.text.ToString());

        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, TypedLobby.Default);

        btnConnection.interactable = false;
    }

    public override void OnCreatedRoom()
    {
        connectionStatus.text = "Room" + roomName.text + " was created";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        connectionStatus.text = "Failed to create room " + roomName.text;
        btnConnection.interactable = true;
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == int.Parse(playersMaxNumber))
        {
            connectionStatus.text = "Joined room";
            PhotonNetwork.LoadLevel(TagManager.GAME_SCREEN_TAG);
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        connectionStatus.text = "Failed to join room " + roomName.text;
        btnConnection.interactable = true;
    }

}
