using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject player;
    public Transform teleportPoint;
    public Transform teleportFrom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, teleportFrom.position) < 10) ToSecretRoom();
    }
    public void ToSecretRoom()
    {
        Debug.Log("noi");
        player.transform.position = teleportPoint.position;
    }
}
