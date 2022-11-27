using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Character_Instantiator : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        HandlePlayerInstantiation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandlePlayerInstantiation()
    {
        //GameObject playerSpawnPoint = playerSpawnPoints[UnityEngine.Random.Range(0, playerSpawnPoints.Length - 1)];
        SpawnCharacter("Player", Vector3.zero, Quaternion.identity);
        //photonView.RPC("SpawnPlayer", RpcTarget.All, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
        var playersCount = PhotonNetwork.PlayerList.Length;

    }
    public void SpawnCharacter(string characterPrefName, Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.Instantiate(characterPrefName, position, rotation);
        Debug.Log("Player spawned");
    }

}
