using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using Photon.Pun;
using Photon.Realtime;
using System;

public class WaveSpawner : MonoBehaviourPun
{

    public enum SpawnState { SPAWNING, WAITING, COUNTING}

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float spawnRate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;
    Transform currSpawnpoint;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 1f;
    public bool isWavesCompleted = false;
    [SerializeField] TextMeshProUGUI wavesLeftText;
    int wavesLeft;
    bool isWaveOn = false;

    CharacterModel characterTarget;

    private SpawnState state = SpawnState.COUNTING;
    void Start()
    {
        if(PhotonNetwork.PlayerList.Length == 2) isWaveOn = true;
        waveCountdown = timeBetweenWaves;
        wavesLeft = waves.Length;

    }

    void Update()
    {
        wavesLeft = waves.Length - nextWave;
        wavesLeftText.text = "Waves left: " + wavesLeft + "/" + waves.Length;
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                photonView.RPC("WaveCompleted", RpcTarget.All);
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (isWaveOn && state != SpawnState.SPAWNING)
            {
                photonView.RPC("HandleWaveSpawning", RpcTarget.All);
                Debug.Log("Changing state to spawning");
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    [PunRPC]
    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            //nextWave = 0;
            isWavesCompleted = true;
            Debug.Log("All waves complete");
        }
        else
        {
            photonView.RPC("SetWaveCompleted", RpcTarget.All);
        }
    }

    [PunRPC]
    void SetWaveCompleted()
    {
        nextWave++;
    }

    [PunRPC]
    void HandleWaveSpawning()
    {
        StartCoroutine(SpawnWave(waves[nextWave]));
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            //photonView.RPC("SpawnEnemy", RpcTarget.All);//, _wave.enemy);
            HandleEnemySpawn();
            yield return new WaitForSeconds(1f / _wave.spawnRate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void HandleEnemySpawn()//(Transform _enemy)
    {

        photonView.RPC("RequestSpawnPoint", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer);
        //if (currSpawnpoint != null) PhotonNetwork.Instantiate("Enemy", currSpawnpoint.position, Quaternion.identity);
        Debug.Log("Spawnpoint: " + currSpawnpoint);
        
    }

    [PunRPC]
    void RequestSpawnPoint(Player client)
    {
        //currSpawnpoint = SetEnemyRandomSP();
        int index = SetEnemyRandomSP();
        photonView.RPC("SpawnEnemy", client, index);
        //photonView.RPC("SetEnemyRandomSP", client, currSpawnpoint);
    }

    [PunRPC]
    void SpawnEnemy(int _spawnPointIndex)
    {
        PhotonNetwork.Instantiate("Enemy2", spawnPoints[_spawnPointIndex].position, spawnPoints[_spawnPointIndex].rotation);
    }

    int SetEnemyRandomSP()
    {

        //List<Transform> list = new List<Transform>();
        //re
        //if(spawnPoints.Length > 1)
        //{
        //    for (int i = 0; i < spawnPoints.Length; i++)
        //    {
        //        if (spawnPoints[i] != characterTarget) list.Add(spawnPoints[i]);
        //    }

        int index = UnityEngine.Random.Range(0, spawnPoints.Length);
        return index;
        //    currSpawnpoint = list[index];

        //}
        ////else photonView.RPC("DestroyEnemy", RpcTarget.All);

        //return currSpawnpoint;
    }

    [PunRPC]
    void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

}
