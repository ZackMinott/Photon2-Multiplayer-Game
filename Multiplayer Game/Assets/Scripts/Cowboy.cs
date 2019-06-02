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
        var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += movement * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.D))
        {
            sprite.flipX = false;
            anim.SetBool("IsMove", true);
        } else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("IsMove", false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            sprite.flipX = true;
            anim.SetBool("IsMove", true);
        }else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("IsMove", false);
        }
    }
}
