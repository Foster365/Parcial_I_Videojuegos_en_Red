using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

using Photon.Pun;
using Photon.Realtime;
using System;

public class GameManager : MonoBehaviourPun
{

    Instantiator gameInstantiator;

    [SerializeField] TextMeshProUGUI gameStartTimer;
    [SerializeField] TextMeshProUGUI gameTimer;
    float timeLeft = 200;
    int initTimer = 3;

    bool isGameOn = false;

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
        gameTimer.enabled = false;
        gameInstantiator.HandlePlayerSpawning();
        if (PhotonNetwork.PlayerList.Length == 1) StartGameInitCountdown();
        //if (photonView.IsMine) UpdateGameTimer();
        //photonView.RPC("StartGameTimer", RpcTarget.All);
        gameTimer.text = timeLeft.ToString();
    }

    private void Update()
    {
        if (isGameOn) UpdateGameTimer();//photonView.RPC("UpdateGameTimer", RpcTarget.All);
    }

    void StartGameInitCountdown()
    {
        StartCoroutine(InitCountdown());
    }

    IEnumerator InitCountdown()
    {
        while(initTimer>0)
        {
            gameStartTimer.text = initTimer.ToString();
            yield return new WaitForSeconds(1f);
            initTimer--;
        }
        StartGameTest();
    }
    void StartGameTest()
    {

        photonView.RPC("SetGameOnBoolean", RpcTarget.All, true);
        Debug.Log("Game begins");
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

}
