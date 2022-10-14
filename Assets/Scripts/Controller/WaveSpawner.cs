using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    CharacterModel characterTarget;

    private SpawnState state = SpawnState.COUNTING;
    void Start()
    {
        waveCountdown = timeBetweenWaves;

    }

    void Update()
    {
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
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && state != SpawnState.SPAWNING)
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

        if (photonView.IsMine)
        {
            photonView.RPC("RequestSpawnPoint", PhotonNetwork.MasterClient, PhotonNetwork.LocalPlayer);
            //if (currSpawnpoint != null) PhotonNetwork.Instantiate("Enemy", currSpawnpoint.position, Quaternion.identity);
            Debug.Log("Spawnpoint: " + currSpawnpoint);
        }
    }

    [PunRPC]
    void RequestSpawnPoint(Player client)
    {
        currSpawnpoint = SetEnemyRandomSP();
        photonView.RPC("SpawnEnemy", client, currSpawnpoint);
        //photonView.RPC("SetEnemyRandomSP", client, currSpawnpoint);
    }

    [PunRPC]
    void SpawnEnemy(Transform _spawnPoint)
    {
        PhotonNetwork.Instantiate("Enemy", _spawnPoint.position, _spawnPoint.rotation);
    }

    Transform SetEnemyRandomSP()
    {

        List<Transform> list = new List<Transform>();

        if(spawnPoints.Length > 1)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i] != characterTarget) list.Add(spawnPoints[i]);
            }

            int index = UnityEngine.Random.Range(0, list.Count - 1);
            currSpawnpoint = list[index];

        }
        //else photonView.RPC("DestroyEnemy", RpcTarget.All);

        return currSpawnpoint;
    }

    [PunRPC]
    void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

}
