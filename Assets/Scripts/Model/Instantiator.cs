using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;
using System;

public class Instantiator : MonoBehaviourPun
{
    [SerializeField] SpawnPoint[] playerSpawnPoints;
    CharacterModel character;

    public void HandlePlayerSpawning()
    {
        //character = GameObject.FindObjectOfType<CharacterModel>();
        //character = PhotonView.Find(PhotonNetwork.LocalPlayer.ActorNumber).gameObject.GetComponent<CharacterModel>();

        CheckSpawnPoints();

    }

    void CheckSpawnPoints()
    {
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        if (spawnPoints.Length > 0)
        {

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Debug.Log("Is avaiable before spawning player? " + spawnPoints[i].IsAvaiable);
                if (spawnPoints[i].IsAvaiable)
                {
                    spawnPoints[i].IsAvaiable = false;
                    //photonView.RPC("SpawnPlayer", RpcTarget.All, spawnPoints[i].transform.position, Quaternion.identity);
                    SpawnPlayer(spawnPoints[i].transform.position, Quaternion.identity);
                    Debug.Log("Player position: " + spawnPoints[i].transform.position);
                    Debug.Log("Is avaiable after spawning player? " + spawnPoints[i].IsAvaiable);
                    return;
                }
            }
        }
    }

    //[PunRPC]
    void SpawnPlayer(Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.Instantiate("Player", position, rotation);
    }

}
