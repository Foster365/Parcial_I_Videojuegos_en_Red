using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Instantiator : MonoBehaviourPun
{
    [SerializeField] SpawnPoint[] playerSpawnPoints;

    void Awake()
    {
        if (!photonView.IsMine) Destroy(this);
        CheckSpawnPoints();
    }

    void CheckSpawnPoints()
    {
        foreach (SpawnPoint sp in playerSpawnPoints)
        {
            if (sp.IsAvaiable) SpawnPlayer(sp, sp.transform.position, Quaternion.identity);
        }
    }

    void SpawnPlayer(SpawnPoint sp, Vector3 position, Quaternion rotation)
    {
        sp.IsAvaiable = false;
        PhotonNetwork.Instantiate("Character", position, rotation);
    }

}
