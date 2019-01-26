using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log(collision.collider.gameObject.tag);
        if (collision.collider.gameObject.tag == "Terrain")
        {
            player.OnGround = true;
        }
    }
}
