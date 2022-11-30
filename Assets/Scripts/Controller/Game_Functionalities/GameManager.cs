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
    float timeLeft = 200, syncTimer = 0, timeToSync = 2f;
    //

    bool isGameOn = false, isVictory = false, isDefeat = false;

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
    }

    private void Update()
    {
        if (isGameOn)
        {
            UpdateGameTimer();
            CheckPlayerDisconnected();
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("I'm Master Client");
                WaitToSync();
            }
        }
    }

    void CheckPlayerDisconnected()
    {
        if (!PhotonNetwork.InRoom && !PhotonNetwork.IsConnected)
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
    }

    [PunRPC]
    public void StartGame(bool _activated)
    {
        isGameOn = _activated;
        gameTimer.enabled = _activated;
    }

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

    void CheckDefeat()
    {

        if (PhotonNetwork.PlayerList.Length == 0 || timeLeft <= 0) photonView.RPC("LoadGameOverScene", RpcTarget.All);
    }

    [PunRPC]
    void LoadWinScene()
    {
        string level = levelManager.GetDictionaryValue(Levels.winScreen, LevelsValues.Win).ToString();
        SceneManager.LoadScene(level);
    }

    [PunRPC]
    void LoadGameOverScene()
    {
        string level = levelManager.GetDictionaryValue(Levels.gameOverScreen, LevelsValues.Game_Over).ToString();
        SceneManager.LoadScene(level);
    }

}
