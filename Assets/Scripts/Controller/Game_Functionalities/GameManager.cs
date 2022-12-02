using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static LevelsManager;

public class GameManager : MonoBehaviourPunCallbacks
{
    LevelsManager levelManager;
    Character_Instantiator charInstantiator;
    WaveSpawner waveSpawner;

    //Game timer variables
    [SerializeField] TextMeshProUGUI gameTimer;
    float timeLeft = 200, syncTimer = 0, timeToSync = 2f;
    //

    bool isGameOn = false, isVictory = false, isDefeat = false;
    WaveSystem waveSystem;
    [SerializeField]
    bool isAllWavesCompleted = false;

    #region Singleton

    public GameManager GameManagerInstance { get; private set; }
    public bool IsGameOn { get => isGameOn; set => isGameOn = value; }
    public bool IsVictory { get => isVictory; set => isVictory = value; }
    public bool IsDefeat { get => isDefeat; set => isDefeat = value; }

    #endregion;

    private void Awake()
    {
        if (GameManagerInstance != null && GameManagerInstance != this) Destroy(this);
        else
        {
            GameManagerInstance = this;
            waveSystem = GetComponent<WaveSystem>();
            charInstantiator = GetComponent<Character_Instantiator>();
        }
    }
    private void Start()
    {
        //Debug.Log("Max players: " + PhotonNetwork.CurrentRoom.MaxPlayers);
        //charInstantiator.HandlePlayerInstantiation();
        gameTimer.enabled = false;
        isGameOn = false;
        gameTimer.text = timeLeft.ToString();
        if (PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers) photonView.RPC("StartGame", RpcTarget.All, true);
        else if (PhotonNetwork.IsMasterClient)
        {
            //TESTING ONLY
            //float timer = 0;
            //timer += Time.deltaTime;
            //if (timer >= timeToSync) isAllWavesCompleted = true;
            //
        }
    }

    private void Update()
    {
        if (isGameOn)
        {
            UpdateGameTimer();
            //CheckPlayerDisconnected();
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("I'm Master Client");
                CharacterModel[] charModels = GameObject.FindObjectsOfType<CharacterModel>();
                Debug.Log("Char models" + charModels.Length);
                Debug.Log("Is victory?: " + isVictory);
                WaitToSync();
                //CheckWin();
                CheckDefeat();
            }
        }
    }

    [PunRPC]
    public void StartGame(bool _activated)
    {
        isGameOn = _activated;
        gameTimer.enabled = _activated;
    }

    #region Game_Timer
    void UpdateGameTimer()
    {

        gameTimer.enabled = true;
        timeLeft -= Time.deltaTime;
        HandleGameTimer(timeLeft);

    }

    public void HandleGameTimer(float _currentTime)
    {
        _currentTime += 1;

        var minutes = Mathf.FloorToInt(_currentTime / 60);
        var seconds = Mathf.FloorToInt(_currentTime % 60);

        gameTimer.text = String.Format("{0:00}:{1:00} ", minutes, seconds);

    }

    void WaitToSync()
    {
        Debug.Log("Waiting to sync timer");

        syncTimer += Time.deltaTime;
        //Debug.Log("timer: " + syncTimer);
        if (syncTimer >= timeToSync)
        {
            Debug.Log("Syncing timer");
            photonView.RPC("SetTimerFix", RpcTarget.Others, timeLeft);
            syncTimer = 0;
        }
    }

    [PunRPC]
    public void SetTimerFix(float _timeLeft)
    {
        timeLeft = _timeLeft;
        Debug.Log("Timer synced");
    }
    #endregion

    void CheckWin()
    {
        if (isVictory) photonView.RPC("LoadWinScene", RpcTarget.All);
    }

    void CheckDefeat()
    {
        if (timeLeft <= 0) photonView.RPC("LoadGameOverScene", RpcTarget.All);
    }

    [PunRPC]
    void LoadWinScene()
    {
        SceneManager.LoadScene("Win");
        PhotonNetwork.Disconnect();
    }

    [PunRPC]
    void LoadGameOverScene()
    {
        SceneManager.LoadScene("Game_Over");
        PhotonNetwork.Disconnect();
    }

}
