using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class CharacterModel : MonoBehaviourPun
{
    bool hasPlayerAlreadySpawned = false;

    public bool HasPlayerAlreadySpawned { get => hasPlayerAlreadySpawned; set => hasPlayerAlreadySpawned = value; }

    // Start is called before the first frame update
    void Start()
    {
        print("Sarasa" + photonView.Owner);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
