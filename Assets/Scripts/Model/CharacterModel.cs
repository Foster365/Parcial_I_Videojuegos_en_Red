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
    HealthManager healthManager;
    LevelsManager levelsManager;
    public bool HasPlayerAlreadySpawned { get => hasPlayerAlreadySpawned; set => hasPlayerAlreadySpawned = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) Destroy(this);
        healthManager = GetComponent<HealthManager>();
        levelsManager = GameObject.FindWithTag(TagManager.LEVELS_MANAGER_TAG).gameObject.GetComponent<LevelsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthManager.currentHealth <= 0) photonView.RPC("LoadGameOverScene", PhotonNetwork.LocalPlayer);
    }
    [PunRPC]
    void LoadGameOverScene()
    {
        string level = levelsManager.GetDictionaryValue(Levels.gameOverScreen, LevelsValues.Game_Over).ToString();
        SceneManager.LoadScene(level);
    }
}
