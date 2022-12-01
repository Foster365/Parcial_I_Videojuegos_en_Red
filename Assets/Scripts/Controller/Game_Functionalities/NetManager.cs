using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LevelsManager;

public class NetManager : MonoBehaviourPunCallbacks
{

    [SerializeField] Button btnConnection;
    [SerializeField] TextMeshProUGUI connectionStatus;
    [SerializeField] TextMeshProUGUI playersCount;
    [SerializeField] InputFieldHandler inputFieldHandler;
    LevelsManager levelsManager;
    string playersMaxNumber = "2";
    string[] genericNicknames = { "Menem", "Chinchulancha", "SinNombre" };
    string genericNickName = "Carlos";
    bool isRoomCreated = false;

    private void Awake()
    {

        levelsManager = GameObject.FindWithTag(TagManager.LEVELS_MANAGER_TAG).GetComponent<LevelsManager>();

    }



    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        btnConnection.interactable = false;

        connectionStatus.text = "Connecting to Master";

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

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = byte.Parse(inputFieldHandler.MaxPlayers.text);
        options.IsOpen = true;
        options.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom(inputFieldHandler.RoomName.text, options, TypedLobby.Default);

        btnConnection.interactable = false;
    }

    public override void OnCreatedRoom()
    {
        isRoomCreated = true;

        connectionStatus.text = "Room " + inputFieldHandler.RoomName.text + " was created!";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {

        connectionStatus.text = "Failed to create room " + inputFieldHandler.RoomName.text;

        btnConnection.interactable = true;

    }

    public override void OnJoinedRoom()
    {
        connectionStatus.text = "Joined room";

        if (levelsManager != null && levelsManager.LevelsDictionary.Count > 0)
        {
            string level = levelsManager.GetDictionaryValue(Levels.gameScreen, LevelsValues.Game).ToString();
            PhotonNetwork.LoadLevel("Non_Authoritative");
        }
        //}
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        connectionStatus.text = "Failed to join room " + inputFieldHandler.RoomName.text;
        btnConnection.interactable = true;
    }

}
