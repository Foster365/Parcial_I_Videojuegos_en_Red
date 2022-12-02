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
    [SerializeField] int maxWaves;
    public TextMeshProUGUI waveNumber;

    public bool canSpawn = true;


    private void Start()
    {
        currentWaveNumber = 0;
        waveNumber.text = currentWaveNumber.ToString();
    }

    public void UpdateWave()
    {

        //if (photonView.IsMine)
        //{
        Debug.Log("Curr wave index value: " + currentWaveNumber);
        Debug.Log("waves length: " + (waves.Length - 1));
        currentWave = waves[currentWaveNumber];
        SpawnWave();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (totalEnemies.Length == 0 && !canSpawn && currentWaveNumber + 1 != waves.Length)
        {
            currentWaveNumber++;
            canSpawn = true;
            if (currentWaveNumber == waves.Length)
            {
                PhotonNetwork.LoadLevel("Win"); // TODO # Note: Setear bool o condición para rpc win a todos desde el game mgr.
            }
        }
        //}
    }

    [PunRPC]
    void SetWaveUI()
    {
        waveNumber.text = currentWaveNumber.ToString();
    }

    void SpawnWave()
    {
        //if (photonView.IsMine)
        //{
        Debug.Log("Start Wave");
        if (canSpawn && nextSpawnTime < Time.time)
        {
            string randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemyGO = PhotonNetwork.Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            currentWave.numbOfEnemies--;
            EnemyModel eModel = enemyGO.gameObject.GetComponent<EnemyModel>();
            Debug.Log("Enemy owner: " + enemyGO.GetPhotonView().Owner);
            eModel.SetRandomTarget();
            nextSpawnTime = Time.time + currentWave.spawnInterval;
            if (currentWave.numbOfEnemies == 0)
            {
                canSpawn = false;
            }
        }
        //}

    }
}
