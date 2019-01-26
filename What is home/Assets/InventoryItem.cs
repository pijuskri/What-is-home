using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    // Start is called before the first frame update
    public int[] hotbar;
    public int inventoryIndex;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {

        if (Player.NumKey() != -1)
        {
            Debug.Log("ey");
            for (int i=0;i<hotbar.Length;i++)
            {
                if (hotbar[i] == inventoryIndex) hotbar[i] = -1;
            }
            hotbar[Player.NumKey()] = inventoryIndex;
        }
    }
}
