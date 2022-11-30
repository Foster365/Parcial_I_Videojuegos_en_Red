using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Instantiator : MonoBehaviourPun
{
    [SerializeField] Transform playerSpawnSeed;
    CharacterModel character;
    Player[] characters;

    private void Awake()
    {
        //playerSpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    public void HandlePlayerSpawning()
    {
        //GameObject playerSpawnPoint = playerSpawnPoints[UnityEngine.Random.Range(0, playerSpawnPoints.Length - 1)];
        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(playerSpawnSeed.position.x, playerSpawnSeed.position.x + 5),
            playerSpawnSeed.position.y, UnityEngine.Random.Range(playerSpawnSeed.position.z, playerSpawnSeed.position.z + 5));
        SpawnCharacter("Player", spawnPosition, playerSpawnSeed.rotation);
        //photonView.RPC("SpawnPlayer", RpcTarget.All, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
        var playersCount = PhotonNetwork.PlayerList.Length;

        //character = GameObject.FindObjectOfType<CharacterModel>();
        //character = PhotonView.Find(PhotonNetwork.LocalPlayer.ActorNumber).gameObject.GetComponent<CharacterModel>();

        //CheckSpawnPoints();

    }

    public void SpawnCharacter(string characterPrefName, Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.Instantiate(characterPrefName, position, rotation);
    }

}
