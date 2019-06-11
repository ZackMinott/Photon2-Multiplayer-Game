using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

public class HurtEffect : MonoBehaviourPun
{
    public SpriteRenderer Sprite;

    public enum EventCodes
    {
        ColorChange = 0
    };

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    
    /*
     * byte eventCode, object content, int senderId
     * called on both clients
     */
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        object content = photonEvent.CustomData;

        EventCodes code = (EventCodes) eventCode;
        if (code == EventCodes.ColorChange)
        {
            object[] datas = content as object[]; //gets color code the user sends which is the rgb values
            if (datas.Length == 4)
            {
                //Ensures the only the "other" player executes the event and changes the color of the sprite on the player
                if((int)datas[0] == base.photonView.ViewID)
                    Sprite.color = new Color((float)datas[1], (float)datas[2], (float)datas[3]);
            }
        }
    }

    /*
     * Called from bullet script
     */
    public void GotHit()
    {
        ChangeColor_RED();

        StartCoroutine("ChangeColorOverTime");
    }

    /*
     * Called from gamemanager class when respawning
     */
    public void ResetToWhite()
    {
        ChangeColor_WHITE();
    }

    IEnumerator ChangeColorOverTime()
    {
        yield return new WaitForSeconds(0.2f);
        ChangeColor_WHITE();
    }

    private void ChangeColor_WHITE()
    {
        float r = 1f, g = 1f, b = 1f;

        object[] datas = new object[] {base.photonView.ViewID,r,g,b};

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };

        SendOptions sendOptions = new SendOptions();
        sendOptions.Reliability = true;

        PhotonNetwork.RaiseEvent((byte) EventCodes.ColorChange, datas, options, sendOptions);
    }

    private void ChangeColor_RED()
    {
        float r = 1f, g = 0f, b = 0f;

        object[] datas = new object[] {base.photonView.ViewID,r,g,b};

        RaiseEventOptions options = new RaiseEventOptions()
        {
            CachingOption = EventCaching.DoNotCache, //Does not store event
            Receivers = ReceiverGroup.All //Means this event is called on the client who is raising the event
        };

        SendOptions sendOptions = new SendOptions();
        sendOptions.Reliability = true; //this message should be sent, unreliable means message could be skipped

        PhotonNetwork.RaiseEvent((byte) EventCodes.ColorChange, datas, options, sendOptions); //Raises the event
    }
}
