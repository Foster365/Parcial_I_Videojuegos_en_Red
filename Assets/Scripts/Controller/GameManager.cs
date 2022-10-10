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

    [SerializeField] TextMeshProUGUI gameTimer;
    float timeLeft = 200;

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
        gameInstantiator.HandlePlayerSpawning();
        if (PhotonNetwork.PlayerList.Length == 2) StartGame();
        //if (photonView.IsMine) UpdateGameTimer();
        //photonView.RPC("StartGameTimer", RpcTarget.All);
        gameTimer.text = timeLeft.ToString();
    }

    private void Update()
    {
        if (isGameOn) photonView.RPC("UpdateGameTimer", RpcTarget.All);
    }

    void StartGame()
    {
        StartCoroutine(StartGameInitCoroutine());
    }

    IEnumerator StartGameInitCoroutine()
    {
        Debug.Log("Starting game in 3 sec");
        yield return new WaitForSeconds(3);
        isGameOn = true;
        Debug.Log("Game begins");
        //photonView.RPC("UpdateGameTimer", RpcTarget.All);
    }
    [PunRPC]
    void StartGameTimer()
    {
        UpdateGameTimer();
    }
    [PunRPC]
    void UpdateGameTimer()
    {

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
