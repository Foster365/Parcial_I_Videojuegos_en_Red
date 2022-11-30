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

    Instantiator gameInstantiator;
    LevelsManager levelManager;
    WaveSpawner waveSpawner;

    //Game timer variables
    [SerializeField] TextMeshProUGUI gameTimer;
    float timeLeft = 10, syncTimer = 0, timeToSync = 2f;
    //

    bool isGameOn = false, isVictory = false, isDefeat = false;

    [SerializeField]
    bool isAllWavesCompleted = false;

    #region Singleton

    public GameManager GameManagerInstance { get; private set; }

    #endregion;

    private void Awake()
    {
        if (GameManagerInstance != null && GameManagerInstance != this) Destroy(this);
        else GameManagerInstance = this;
    }
    private void Start()
    {
        gameTimer.enabled = false;
        isGameOn = false;
        gameTimer.text = timeLeft.ToString();
        if (PhotonNetwork.PlayerList.Length > 1) photonView.RPC("StartGame", RpcTarget.All, true);
        else if (PhotonNetwork.IsMasterClient)
        {
            //TESTING ONLY
            float timer = 0;
            timer += Time.deltaTime;
            if (timer >= timeToSync) isAllWavesCompleted = true;
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
                WaitToSync();
                CheckWin();
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
        Debug.Log("timer: " + syncTimer);
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
        if (isAllWavesCompleted) photonView.RPC("LoadWinScene", RpcTarget.All);
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
