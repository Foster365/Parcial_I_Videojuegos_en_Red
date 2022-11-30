using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameTimerHandler : MonoBehaviourPun
{

    [SerializeField] TextMeshProUGUI gameTimer;
    [SerializeField] bool isGameOn;
    float timeLeft = 200, syncTimer = 0, timeToSync = 2f;

    // Start is called before the first frame update
    void Start()
    {
        gameTimer.enabled = false;
        isGameOn = false;
        gameTimer.text = timeLeft.ToString();
        if (PhotonNetwork.PlayerList.Length > 1) photonView.RPC("StartGame", RpcTarget.All, true);
    }

    void Update()
    {
        if (isGameOn)
        {
            UpdateGameTimer();
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("I'm Master Client");
                WaitToSync();
            }
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

}
