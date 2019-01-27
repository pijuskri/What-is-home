using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject playerObject;
    public Player player;
    public Transform teleportPoint;
    public Transform teleportFrom;
    public Transform glasses;
    bool teleported = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(playerObject.transform.position, teleportFrom.position) < 10 && !teleported) ToSecretRoom();
        if (Vector3.Distance(playerObject.transform.position, glasses.position) < 3 && teleported) FromSecretRoom();
    }
    public void ToSecretRoom()
    {
        playerObject.transform.position = teleportPoint.position;
        teleported = true;
    }
    public void FromSecretRoom()
    {
        playerObject.transform.position = teleportFrom.position;
        player.glassesOn = true;
    }
}
