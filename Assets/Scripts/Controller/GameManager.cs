using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun
{
    Instantiator gameInstantiator;

    private void Awake()
    {
        gameInstantiator = GetComponent<Instantiator>();
    }
    private void Start()
    {
        //if(photonView.IsMine)
            gameInstantiator.HandlePlayerSpawning();
    }

}
