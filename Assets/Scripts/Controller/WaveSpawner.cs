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

    public Transform[] spawnPoint;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 1f;

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
        
        for (int i = 0; i<_wave.count; i++)
        {
            photonView.RPC("SpawnEnemy", RpcTarget.All);//, _wave.enemy);
            yield return new WaitForSeconds(1f / _wave.spawnRate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    [PunRPC]
    void SpawnEnemy()//(Transform _enemy)
    {
        //spawn
        Transform _sp = spawnPoint[UnityEngine.Random.Range(0, spawnPoint.Length) ];
        PhotonNetwork.Instantiate("Enemy", _sp.position, _sp.rotation);
    }

}
