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
        gameInstantiator = GetComponent<Instantiator>();
    }
    private void Start()
    {
        levelManager = GameObject.FindWithTag(TagManager.LEVELS_MANAGER_TAG).gameObject.GetComponent<LevelsManager>();
        waveSpawner = GameObject.FindWithTag("GM").gameObject.GetComponent<WaveSpawner>();

        gameTimer.enabled = false;
        isGameOn = false;

        if (PhotonNetwork.PlayerList.Length > 1) photonView.RPC("StartGame", RpcTarget.All, true);

        gameInstantiator.HandlePlayerSpawning();
    }

    private void Update()
    {
        if (isGameOn)
        {
            UpdateGameTimer();
            CheckPlayerDisconnected();
            CheckVictory();
            CheckDefeat();
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
    public void HandleGameTimer(float currentTime)
    {
        currentTime += 1;

        var minutes = Mathf.FloorToInt(currentTime / 60);
        var seconds = Mathf.FloorToInt(currentTime % 60);

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
            photonView.RPC("SetTimerFix", RpcTarget.Others, gameTimer.text);
            syncTimer = 0;
        }
    }

    [PunRPC]
    public void SetTimerFix(string _timer)
    {
        gameTimer.text = _timer;
        Debug.Log("Timer synced");
    }

    void CheckVictory()
    {

        if (PhotonNetwork.PlayerList.Length > 0 && waveSpawner.isWavesCompleted) photonView.RPC("LoadWinScene", RpcTarget.All);
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
