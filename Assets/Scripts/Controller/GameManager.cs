using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

using Photon.Pun;
using Photon.Realtime;
using System;
using static LevelsManager;

public class GameManager : MonoBehaviourPun
{

    Instantiator gameInstantiator;
    LevelsManager levelManager;
    WaveSpawner waveSpawner;
    [SerializeField] TextMeshProUGUI gameStartTimer;
    [SerializeField] TextMeshProUGUI gameTimer;
    float timeLeft = 200;
    int initTimer = 3;

    bool isGameOn = false;
    [SerializeField] bool isVictory = false;
    [SerializeField] bool isDefeat = false;

    #region Singleton

    public GameManager GameManagerInstance { get; private set; }

    void HandleSingleton()
    {
        if (GameManagerInstance != null && GameManagerInstance != this) Destroy(this);
        else GameManagerInstance = this;

    }

    #endregion;

    private void Awake()
    {
        HandleSingleton();
        gameInstantiator = GetComponent<Instantiator>();
    }
    private void Start()
    {
        levelManager = GameObject.FindWithTag(TagManager.LEVELS_MANAGER_TAG).gameObject.GetComponent<LevelsManager>();
        waveSpawner = GameObject.FindWithTag("GM").gameObject.GetComponent<WaveSpawner>();
        gameTimer.enabled = false;
        gameStartTimer.enabled = false;
        gameInstantiator.HandlePlayerSpawning();
        if (PhotonNetwork.PlayerList.Length == 2) photonView.RPC("StartGameInitCountdown", RpcTarget.All);
        //if (photonView.IsMine) UpdateGameTimer();
        //photonView.RPC("StartGameTimer", RpcTarget.All);
        gameTimer.text = timeLeft.ToString();
    }

    private void Update()
    {
        if (isGameOn) UpdateGameTimer();//photonView.RPC("UpdateGameTimer", RpcTarget.All);
        CheckVictory();
        CheckDefeat();
    }

    [PunRPC]
    void StartGameInitCountdown()
    {
        StartCoroutine(InitCountdown());
    }

    IEnumerator InitCountdown()
    {
        while(initTimer>0)
        {
            gameStartTimer.enabled = true;
            gameStartTimer.text = "Game starts in " + initTimer.ToString();
            yield return new WaitForSeconds(1f);
            initTimer--;
        }
        gameStartTimer.text = "Game starts!";
        StartCoroutine(WaitToStartCoroutine());
    }

    IEnumerator WaitToStartCoroutine()
    {
        yield return new WaitForSeconds(2);
        gameStartTimer.enabled = false;
        StartGameTest();
    }

    IEnumerator WaitToSync()
    {
        yield return new WaitForSeconds(2);
    }

    void StartGameTest()
    {
        photonView.RPC("SetGameOnBoolean", RpcTarget.All, true);
    }
    [PunRPC]
    bool SetGameOnBoolean(bool isOn)
    {
        return isGameOn = isOn;
    }
    [PunRPC]
    void StartGameTimer()
    {
        UpdateGameTimer();
    }
    //[PunRPC]
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

    void CheckVictory()
    {

        if (PhotonNetwork.PlayerList.Length > 0 && waveSpawner.isWavesCompleted) photonView.RPC("LoadWinScene", RpcTarget.All);
    }

    void CheckDefeat()
    {

        if (PhotonNetwork.PlayerList.Length == 0) photonView.RPC("LoadGameOverScene", RpcTarget.All);
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
