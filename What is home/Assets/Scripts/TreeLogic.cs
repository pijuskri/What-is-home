using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public int woodLeft;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (woodLeft < 1) Destroy(gameObject);
    }
}
