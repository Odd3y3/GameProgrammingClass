using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float speed = 1 ;
    public float max_s = 2;
    public int direction = 1;
    Rigidbody2D rBody;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 pos = rBody.position;
        Debug.Log("1"+pos);
        rBody.position +=  Vector2.left * speed * Time.fixedDeltaTime * -1 * direction;
        Debug.Log("2" + pos);
        rBody.MovePosition(pos);
        Debug.Log("3" + rBody.position);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (direction == 1)
            {
                collision.gameObject.GetComponent<PlayerScript>().maxSpeed_left =collision.gameObject.GetComponent<PlayerScript>().maxSpeed_left + max_s;
                collision.gameObject.GetComponent<PlayerScript>().maxSpeed_right = collision.gameObject.GetComponent<PlayerScript>().maxSpeed_right - max_s;
            }
            else
            {
                collision.gameObject.GetComponent<PlayerScript>().maxSpeed_left = collision.gameObject.GetComponent<PlayerScript>().maxSpeed_left - max_s;
                collision.gameObject.GetComponent<PlayerScript>().maxSpeed_right = collision.gameObject.GetComponent<PlayerScript>().maxSpeed_right + max_s;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (direction == -1)
            {
                collision.gameObject.GetComponent<PlayerScript>().maxSpeed_left = collision.gameObject.GetComponent<PlayerScript>().maxSpeed_left + max_s;
                collision.gameObject.GetComponent<PlayerScript>().maxSpeed_right = collision.gameObject.GetComponent<PlayerScript>().maxSpeed_right - max_s;
            }
            else
            {
                collision.gameObject.GetComponent<PlayerScript>().maxSpeed_left = collision.gameObject.GetComponent<PlayerScript>().maxSpeed_left - max_s;
                collision.gameObject.GetComponent<PlayerScript>().maxSpeed_right = collision.gameObject.GetComponent<PlayerScript>().maxSpeed_right + max_s;
            }
        }
    }
}
