using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPun, IPunObservable
{
    public PhotonView photonView;
    public GameObject BubbleSpeech;
    public Text ChatText;

    public Cowboy player;
    private InputField ChatInput;
    private bool DisableSend;

    void Awake()
    {
        ChatInput = GameObject.Find("ChatInputField").GetComponent<InputField>();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (ChatInput.isFocused)
            {
                player.DisableInputs = true;
            }
            else
            {
                player.DisableInputs = false;
            }

            if (!DisableSend && ChatInput.isFocused)
            {
                if (ChatInput.text != "" && ChatInput.text.Length > 1 && Input.GetKeyDown(KeyCode.Space))
                {
                    photonView.RPC("SendMsg", RpcTarget.AllBuffered, ChatInput.text);
                    BubbleSpeech.SetActive(true);
                    ChatInput.text = "";
                    DisableSend = true;
                }
            }
        }

        
    }

    [PunRPC]
    void SendMsg(string msg)
    {
        ChatText.text = msg;
        StartCoroutine(hideBubbleSpeech());
    }

    IEnumerator hideBubbleSpeech()
    {
        yield return new WaitForSeconds(3);
        BubbleSpeech.SetActive(false);
        DisableSend = false;
    }

    /* This function is implemented by the IPunObservable Interface
       Used to send and receive data 
       IsWriting used to send to all clients
       IsReading used to receive from all clients
       Must add to observed components on Photon View component to be able to work */
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(BubbleSpeech.activeSelf);
            //
        } else if (stream.IsReading)
        {
            BubbleSpeech.SetActive((bool)stream.ReceiveNext());
            //
        }
    }
}
