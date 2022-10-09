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
        gameTimer.text = timeLeft.ToString();
    }

    private void Update()
    {
        if (photonView.IsMine) UpdateGameTimer();
    }

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
