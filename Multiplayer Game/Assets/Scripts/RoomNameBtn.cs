using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomNameBtn : MonoBehaviour
{
    public Text RoomName;

    void Start()
    {
        Button btn = GetComponent<Button>();

        btn.onClick.AddListener(() => JoinRoomByName(RoomName.text));
    }

    public void JoinRoomByName(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }
}
