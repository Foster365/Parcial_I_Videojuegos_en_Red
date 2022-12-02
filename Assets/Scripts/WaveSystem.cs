using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public bool canSpawn = true;


    private void Start()
    {
        currentWaveNumber = 0;
        waveNumber.text = currentWaveNumber.ToString();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            Debug.Log("Curr wave index value: " + currentWaveNumber);
            Debug.Log("waves length: " + (waves.Length - 1));
            currentWave = waves[currentWaveNumber];
            SpawnWave();
            GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (totalEnemies.Length == 0 && currentWaveNumber < waves.Length && !canSpawn)
            {
                //if (currentWaveNumber + 1 != waves.Length)
                //{
                //if (canSpawn)
                //{
                SpawnNextWave();
                //rpc
                photonView.RPC("SetWaveUI", RpcTarget.All);
                //}
                //}

            }
            else
            {
                Debug.Log("win");
            }
        }
    }

    [PunRPC]
    void SetWaveUI()
    {
        waveNumber.text = currentWaveNumber.ToString();
    }

    void SpawnNextWave()
    {
        currentWaveNumber++;
        canSpawn = true;
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
                PhotonNetwork.Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
                currentWave.numbOfEnemies--;
                nextSpawnTime = Time.time + currentWave.spawnInterval;
                if (currentWave.numbOfEnemies == 0)
                {
                    canSpawn = false;
                }
            }
        }

    }
}
