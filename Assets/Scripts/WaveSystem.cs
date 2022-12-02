using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int numbOfEnemies;
    public string[] typeOfEnemies;
    public float spawnInterval;
}


public class WaveSystem : MonoBehaviourPun
{
    public Wave[] waves;
    public Transform[] spawnPoints;

    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpawnTime;
    public TextMeshProUGUI waveNumber;
    bool isWavesCompleted = false;

    public bool canSpawn = true;

    GameManager gameMgr;

    public bool IsWavesCompleted { get => isWavesCompleted; set => isWavesCompleted = value; }

    private void Awake()
    {
        gameMgr = GetComponent<GameManager>();
    }

    private void Start()
    {
        currentWaveNumber = 0;
        waveNumber.text = currentWaveNumber.ToString();

    }

    private void Update()
    {
        if (photonView.IsMine) UpdateWave();
    }

    public void UpdateWave()
    {
        if (photonView.IsMine && gameMgr.IsGameOn)
        {

            Debug.Log("Curr wave index value: " + currentWaveNumber);
            Debug.Log("waves length: " + (waves.Length - 1));
            currentWave = waves[currentWaveNumber];
            if (canSpawn) SpawnWave();
            GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log("Current wave? " + currentWave);
            if (totalEnemies.Length == 0 && !canSpawn && currentWaveNumber + 1 != waves.Length - 1)
            {
                currentWaveNumber++;
                canSpawn = true;
                photonView.RPC("SetWaveUI", RpcTarget.All, currentWaveNumber.ToString());
                if (currentWaveNumber == 3)
                {
                    isWavesCompleted = true;
                    photonView.RPC("LoadWinScene", RpcTarget.All);
                    //photonView.RPC("SetVictoryConditionToTrue", PhotonNetwork.MasterClient, isWavesCompleted);// TODO # Note: Setear bool o condición para rpc win a todos desde el game mgr.
                }
            }
        }

    }

    [PunRPC]
    public void SetVictoryConditionToTrue(bool _isVictory)
    {
        gameMgr.IsVictory = _isVictory;
    }

    [PunRPC]
    void SetWaveUI(string _currWaveNum)
    {
        waveNumber.text = _currWaveNum;
    }

    void CheckWin()
    {
        //if (isVictory) photonView.RPC("LoadWinScene", RpcTarget.All);
    }

    [PunRPC]
    void LoadWinScene()
    {
        SceneManager.LoadScene("Win");
        PhotonNetwork.Disconnect();
    }

    void SpawnWave()
    {
        if (photonView.IsMine)
        {
            Debug.Log("Start Wave");
            if (canSpawn && nextSpawnTime < Time.time)
            {
                string randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
                Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject enemyGO = PhotonNetwork.Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
                currentWave.numbOfEnemies--;
                EnemyModel eModel = enemyGO.gameObject.GetComponent<EnemyModel>();
                eModel.SetRandomTarget();
                nextSpawnTime = Time.time + currentWave.spawnInterval;
                if (currentWave.numbOfEnemies == 0)
                {
                    canSpawn = false;
                }
            }
        }

    }
}
