using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayField : MonoBehaviour
{
    public PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("asdasd");
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerScript>().HealthImage.fillAmount = -0.1f;
            collision.gameObject.GetComponent<PlayerScript>().Hit();
        }
        else
        {
            int viewID = collision.gameObject.GetComponent<PhotonView>().ViewID;

            if (PV.IsMine)
            {
                PV.RPC("ObjDestroy", RpcTarget.All, viewID);
            }

           /* Destroy(collision.gameObject);*/
            
        }
    }
    [PunRPC]
    public void ObjDestroy(int viewID)
    {
        if (PhotonView.Find(viewID) != null)
        {
            Destroy(PhotonView.Find(viewID).gameObject);
        }
    }
}
