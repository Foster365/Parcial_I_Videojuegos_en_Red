using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Character_Instantiator : MonoBehaviourPun
{
    [SerializeField] Transform playerSpawnSeed;

    // Start is called before the first frame update
    void Start()
    {
        HandlePlayerInstantiation();
    }

    public void HandlePlayerInstantiation()
    {
        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(playerSpawnSeed.position.x, playerSpawnSeed.position.x + 5),
            playerSpawnSeed.position.y, UnityEngine.Random.Range(playerSpawnSeed.position.z, playerSpawnSeed.position.z + 5));
        SpawnCharacter("Player", spawnPosition, playerSpawnSeed.rotation);
        var playersCount = PhotonNetwork.PlayerList.Length;

    }
    public void SpawnCharacter(string characterPrefName, Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.Instantiate(characterPrefName, position, rotation);
        Debug.Log("Player spawned");
    }

}
