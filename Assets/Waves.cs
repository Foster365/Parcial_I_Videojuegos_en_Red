using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    WaveSpawner waveSpawner;
    // Start is called before the first frame update
    void Start()
    {

        waveSpawner = GameObject.FindWithTag("GM").gameObject.GetComponent<WaveSpawner>();
    }

    public void SetNewWave()
    {
        int testWaveIndex = waveSpawner.nextWave++;
        waveSpawner.HandleWavesCounter(testWaveIndex);
        waveSpawner.HandleSpawnWave(waveSpawner.waves[testWaveIndex]);

        if (waveSpawner.wavesLeft == 0 && waveSpawner.playersCount >= 1) waveSpawner.HandleWin();
    }
}
