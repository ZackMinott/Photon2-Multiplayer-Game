using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Cowboy : MonoBehaviourPunCallbacks
{
    public GameObject playerCamera;
    public PhotonView PV;
    public SpriteRenderer sprite;
    public GameObject bulletPrefab;
    public Transform spawnPointRight;
    public Transform spawnPointLeft;

    public float moveSpeed = 5f;
    public float jumpForce;

    public Animator anim;

    private bool AllowMoving = true;

    public Text PlayerName;

    public bool DisableInputs = false;
    public bool IsGrounded = false;

    private Rigidbody2D rb;

    void Awake()
    {
        if (photonView.IsMine)
        {
            GameManager.instance.localPlayer = this.gameObject;
            playerCamera.SetActive(true);
            //playerCamera.transform.SetParent(null, false);
            PlayerName.text = "You : " + PhotonNetwork.NickName;
            PlayerName.color = Color.green;
        }
        else
        {
            PlayerName.text = photonView.Owner.NickName;
        }
           
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (PV.IsMine && !DisableInputs)
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

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            anim.SetBool("IsMove", true);
        }

        if (Input.GetKeyDown(KeyCode.D) && anim.GetBool("IsShot") == false)
        {   
            
            //playerCamera.GetComponent<CameraFollow2D>().offset = new Vector3(1.3f, 1.5f, 0);
            PV.RPC("FlipSprite_Right", RpcTarget.AllBuffered);
        } else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("IsMove", false);
        }

        if (Input.GetKeyDown(KeyCode.A) && anim.GetBool("IsShot") == false)
        {
            //playerCamera.GetComponent<CameraFollow2D>().offset = new Vector3(-1.3f, 1.5f, 0);
            PV.RPC("FlipSprite_Left", RpcTarget.AllBuffered);

        }else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("IsMove", false);
        }
    }

    private void shot()
    {      
        if (sprite.flipX == false)
        {
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(spawnPointRight.position.x, spawnPointRight.position.y),
                Quaternion.identity, 0);
        }
        else if (sprite.flipX == true)
        {
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(spawnPointLeft.position.x, spawnPointLeft.position.y),
                Quaternion.identity, 0);
            bullet.GetComponent<PhotonView>().RPC("ChangeDirection", RpcTarget.AllBuffered);
        }
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            IsGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            IsGrounded = false;
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce * Time.deltaTime));
    }
}
