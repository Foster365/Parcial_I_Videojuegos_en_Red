using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviourPun
{

    public enum SpawnState { SPAWNING, WAITING, COUNTING }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float spawnRate;
    }

    public Wave[] waves;
    public int nextWave = 0;

    public Transform[] spawnPoints;
    Transform currSpawnpoint;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 1f;
    public bool isWavesCompleted = false;
    [SerializeField] TextMeshProUGUI wavesLeftText;
    public int wavesLeft;
    public int playersCount = 0;
    bool isWaveOn = false;

    CharacterModel characterTarget;

    private SpawnState state = SpawnState.COUNTING;
    void Start()
    {
        if (PhotonNetwork.PlayerList.Length == 1) isWaveOn = true;
        waveCountdown = timeBetweenWaves;
        wavesLeft = waves.Length;

    }

    void Update()
    {
        playersCount = PhotonNetwork.PlayerList.Length;
        Debug.Log("WAVES CURR: " + nextWave);
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

    public void HandleWin()
    {
        photonView.RPC("WinGame", RpcTarget.All);
    }

    public void HandleWavesCounter(int _waves)
    {
        photonView.RPC("WavesLeft", RpcTarget.All, _waves);
    }

    [PunRPC]
    void WavesLeft(int _index)
    {
        wavesLeft = _index;
        Debug.Log("Waves left: " + wavesLeft);
    }


    [PunRPC]
    void WinGame()
    {
        SceneManager.LoadScene("Win");
    }

    [PunRPC]
    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        Debug.Log("Wave was completed!!!");
        if (nextWave + 1 > waves.Length - 1)
        {
            //nextWave = 0;
            isWavesCompleted = true;

            Debug.Log("All waves complete");
        }
        else
        {
            Debug.Log("PASA");
            photonView.RPC("SetWaveCompleted", RpcTarget.All);
        }
    }

    [PunRPC]
    void SetWaveCompleted()
    {
        nextWave++;
        Debug.Log("Next wave: " + nextWave);
    }

    [PunRPC]
    void HandleWaveSpawning()
    {
        if(nextWave >=0 && nextWave < waves.Length) StartCoroutine(SpawnWave(waves[nextWave]));
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindWithTag("Enemy") == null)
            {
                Debug.Log("Hay enemigos vivos");
                return false;
            }
        }
        Debug.Log("No hay enemigos vivos");
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

    public void HandleSpawnWave(Wave _wave)
    {
        StartCoroutine(SpawnWave(_wave));
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
        PhotonNetwork.Instantiate("Enemy", spawnPoints[_spawnPointIndex].position, spawnPoints[_spawnPointIndex].rotation);
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
