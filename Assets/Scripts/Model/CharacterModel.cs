using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using static LevelsManager;
using UnityEngine.SceneManagement;

public class CharacterModel : MonoBehaviourPun
{
    bool hasPlayerAlreadySpawned = false;
    GameManager gameManager;
    HealthManager healthMgr;

    public bool HasPlayerAlreadySpawned { get => hasPlayerAlreadySpawned; set => hasPlayerAlreadySpawned = value; }
    public HealthManager HealthMgr { get => healthMgr; set => healthMgr = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) Destroy(this);
        healthMgr = GetComponent<HealthManager>();
    }

}
