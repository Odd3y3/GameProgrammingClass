using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class BulletScript : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public Animator AN;
    public int killed = 0;
    int dir;    //방향
    int BulletUp = 0; //각도
    int fire = 0;
    int BSpeed = 10;
    PlayerScript playerscript;


    void Start()
    {
        transform.localScale = new Vector3(dir, 1, 1);
        transform.eulerAngles = new Vector3(0, 0, BulletUp);
        Destroy(gameObject, 3.5f);
    }

    void FixedUpdate()
    {

        if (fire == 0)
        {
            transform.Translate(Vector3.right * BSpeed * Time.deltaTime * dir);//속도
        }
        if (PV.IsMine && killed == 1)
        {
            NetworkManager manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
            //manager.kill += 1;
            killed = 0;
        }
    }


    void OnTriggerEnter2D(Collider2D col) // col을 RPC의 매개변수로 넘겨줄 수 없다
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground") || col.gameObject.layer == LayerMask.NameToLayer("Grass")) PV.RPC("DestroyRPC", RpcTarget.AllBuffered);

        if(col.gameObject.layer == LayerMask.NameToLayer("Ground")){


            col.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad))*500);
        }

        if (PV.IsMine && col.tag == "Player" && col.GetComponent<PhotonView>().IsMine==false && col.GetComponent<PlayerScript>().HealthImage.fillAmount>0)
        {
            PV.RPC("DestroyRPC", RpcTarget.AllBuffered);
            col.GetComponent<PhotonView>().RPC("Hit", RpcTarget.All);
            if(col.GetComponent<PlayerScript>().HealthImage.fillAmount == 0)
            {
                playerscript.kill += 1;
                GameObject.Find("Canvas2").transform.Find("KillPanel").transform.Find("Text").GetComponent<Text>().text = "Kill : " + playerscript.kill;

                GameObject.Find("Canvas2").transform.Find("Notice").transform.Find("Text").GetComponent<Text>().text = playerscript.nick + " 님이 " + col.GetComponent<PlayerScript>().nick + " 님을 처치하였습니다.";
                GameObject.Find("Canvas2").transform.Find("Notice").gameObject.SetActive(true);
            }
        }
    }


    [PunRPC]
    void DirRPC(int dir, int skin, int Up, int Speed, int actor1)
    {
        this.dir = dir; //총알 방향 플레이어에서 받음
        GameManager gamemanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        int i;
        for(i=0; i<4; i++)
        {
            if (gamemanager.Players[i].actor == actor1)
                break;
        }

        playerscript = gamemanager.Players[i].GetComponent<PlayerScript>();
        AN.SetFloat("Blend", skin);
        this.BulletUp = Up;
        BSpeed = Speed;
    }

    [PunRPC]
    void DestroyRPC()
    {
        fire = 1;
        AN.SetTrigger("shoot");
        Destroy(gameObject, 0.5f);//파괴
    }
}
