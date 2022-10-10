using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class InputFieldHandler : MonoBehaviourPun
{
    [SerializeField] TMP_InputField characterNickName;
    [SerializeField] TMP_InputField roomName;
    public const string playerNamePrefKey = "Charango";
    public const string roomNamePrefKey = "Room";

    public TMP_InputField CharacterNickName { get => characterNickName; set => characterNickName = value; }
    public TMP_InputField RoomName { get => roomName; set => roomName = value; }

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            //DontDestroyOnLoad(this);

            HandleName(characterNickName, playerNamePrefKey);
            HandleName(roomName, roomNamePrefKey);

        }
    }

    private void Update()
    {
        if(photonView.IsMine)
        {

            Debug.Log("AAAAAAAAAAANISMAN!!!: " + PlayerPrefs.GetString(playerNamePrefKey));
            Debug.Log("AAAAAAAAAAANISMAN!!!: " + PlayerPrefs.GetString(roomNamePrefKey));

        }
    }
    public void HandleName(TMP_InputField textInputName, string defaultInputName)
    {

        string defaultName = string.Empty;
        TMP_InputField _inputField = textInputName;
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(defaultInputName))
            {
                defaultName = PlayerPrefs.GetString(defaultInputName);
                _inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;

    }
    public void SetPlayerInputName()
    {
        PlayerPrefs.SetString(playerNamePrefKey, characterNickName.text);
        // #Important
        if (string.IsNullOrEmpty(characterNickName.text))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = characterNickName.text;
        PlayerPrefs.SetString(playerNamePrefKey, characterNickName.text);
    }

    public void SetRoomInputName()
    {

        PlayerPrefs.SetString(roomNamePrefKey, roomName.text);
        // #Important
        if (string.IsNullOrEmpty(roomName.text))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        //PhotonNetwork.CurrentRoom.Name = room.text;
        PlayerPrefs.SetString(roomNamePrefKey, roomName.text);
    }
}
