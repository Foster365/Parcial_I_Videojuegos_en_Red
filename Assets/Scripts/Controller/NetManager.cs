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
        options.MaxPlayers = 3;
        options.IsOpen = true;
        options.IsVisible = true;
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
        connectionStatus.text = "Joined room";
        PhotonNetwork.LoadLevel("Game");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        connectionStatus.text = "Failed to join room " + roomName.text;
        btnConnection.interactable = true;
    }

}
