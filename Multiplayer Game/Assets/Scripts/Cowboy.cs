using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Cowboy : MonoBehaviour
{
    public GameObject playerCamera;
    public PhotonView PV;
    public SpriteRenderer sprite;

    public float moveSpeed = 5f;

    public Animator anim;

    private bool AllowMoving = true;

    void Awake()
    {
        if(PV.IsMine)
            playerCamera.SetActive(true);
    }

    void Update()
    {
        if (PV.IsMine)
        {
            checkInputs();
        }
    }

    private void checkInputs()
    {
        if (AllowMoving)
        {
            var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
            transform.position += movement * moveSpeed * Time.deltaTime;
        }    

        if (Input.GetKeyDown(KeyCode.RightControl) && anim.GetBool("IsMove") == false)
        {
            shot();
        } else if (Input.GetKeyUp(KeyCode.RightControl))
        {
            anim.SetBool("IsShot", false);
            AllowMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.D) && anim.GetBool("IsShot") == false)
        {   
            anim.SetBool("IsMove", true);
            PV.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
        } else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("IsMove", false);
        }

        if (Input.GetKeyDown(KeyCode.A) && anim.GetBool("IsShot") == false)
        {
            anim.SetBool("IsMove", true);
            PV.RPC("FlipSprite_Left", RpcTarget.AllBuffered);

        }else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("IsMove", false);
        }
    }

    private void shot()
    {
        anim.SetBool("IsShot", true);
        AllowMoving = false;
    }

    [PunRPC]
    private void FlipSprite_Right()
    {
        sprite.flipX = false;
    }

    [PunRPC]
    private void FlipSprite_Left()
    {
        sprite.flipX = true;
    }
}
