using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisManager : MonoBehaviourPunCallbacks
{
    public GameObject DisUI;
    public GameObject MenuButton;
    public GameObject ReconnectButton;
    public Text StatusText;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            DisUI.SetActive(true);

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                ReconnectButton.SetActive(true);
                StatusText.text = "Lost connection to Photon, please try to reconnect";
            }

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                MenuButton.SetActive(true);
                StatusText.text = "Lost connection to Photon, please try to reconnect in the main menu";
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        if (DisUI.activeSelf)
        {
            MenuButton.SetActive(false);
            ReconnectButton.SetActive(false);
            DisUI.SetActive(false);
        }
    }

    public void OnClick_TryConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnClick_Menu()
    {
        PhotonNetwork.LoadLevel(0);
    }
}
