using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class Items
{
    public List<ItemDef> items = new List<ItemDef>();
}
[System.Serializable]
public class ItemDef
{
    public string name;
    public string type;
    public ItemDef(string name, string type)
    {
        this.name = name;
        this.type = type;
    }
}
public class Item
{
    public ItemDef ItemDef;
    public int amount;
    public Item(ItemDef itemDef, int amount)
    {
        this.ItemDef = itemDef;
        this.amount = amount;
    }
}
public class Player : MonoBehaviour
{
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 4F;
    public float sensitivityY = 4F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -70F;
    public float maximumY = 70F;
    float rotationY = 0F;


    // Start is called before the first frame update
    public GameObject playerObject;
    Rigidbody playerRigid;
    public GameObject itemDisplayPrefab;
    public GameObject InventoryContainer;
    public GameObject InventoryPanel;
    public GameObject hotbarUI;
    public GameObject[] hotbarItems;

    public Items items;
    public List<Item> inventory;
    public int[] hotbar = new int[9];

    public int currentItem = 0;
    float speed = 3f;
    bool invOpen = false;

    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            hotbar[i] = -1;
        }
        
        playerRigid = playerObject.GetComponent<Rigidbody>();
        playerRigid.freezeRotation = true;

        items = JsonUtility.FromJson<Items>(File.ReadAllText( Application.dataPath + "/items.json"));
        inventory = new List<Item>();
        inventory.Add(new Item(items.items[0], 10));
        inventory.Add(new Item(items.items[1], 1));
        PopulateInventory();
        InventoryPanel.SetActive(false);
    }


    void Update()
    {
        playerRigid.velocity = (Input.GetAxis("Horizontal") * playerObject.transform.right + Input.GetAxis("Vertical") * playerObject.transform.forward) * speed;
        if(!invOpen)CameraLook();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (invOpen) { invOpen = false; InventoryPanel.SetActive(false); }
            else { invOpen = true; InventoryPanel.SetActive(true); }
        }
        UpdateHotbar();
        if (NumKey() != -1)
        {
            currentItem = NumKey();
        }
    }
    void CameraLook()
    {
        transform.position = new Vector3(playerObject.transform.position.x, playerObject.transform.position.y + 0.5f, playerObject.transform.position.z);
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.eulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.eulerAngles = new Vector3(-rotationY, rotationX, 0);
            //playerObject.transform.eulerAngles = new Vector3(0, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.eulerAngles = new Vector3(-rotationY, transform.eulerAngles.y, 0);
        }
        playerObject.transform.rotation = Quaternion.Euler(playerObject.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, playerObject.transform.rotation.eulerAngles.z);
    }
    void PopulateInventory()
    {
        int index = 0;
        foreach (var item in inventory)
        {
            GameObject temp = Instantiate(itemDisplayPrefab, InventoryContainer.transform);
            InventoryItem inventoryItem = temp.GetComponent<InventoryItem>();
            Text text = temp.GetComponentInChildren<Text>();
            inventoryItem.hotbar = hotbar;
            inventoryItem.inventoryIndex = index;
            text.text = item.ItemDef.name + ":" + item.amount;
            index++;
        }
    }
    void UpdateHotbar()
    {
        int index = 0;
        foreach (var hotbarItem in hotbarItems)
        {
            if (hotbar[index] != -1)
            {
                Item currentItem = inventory[hotbar[index]];
                Text text = hotbarItem.GetComponentInChildren<Text>();
                text.text = currentItem.ItemDef.name + ":" + currentItem.amount;
            }
            else hotbarItem.GetComponentInChildren<Text>().text = "";

            index++;
        }
    }
    public static int NumKey()
    {
        int num = -1;
        if(Input.GetKeyDown(KeyCode.Alpha0))num = 9;
        if (Input.GetKeyDown(KeyCode.Alpha1)) num = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) num = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) num = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) num = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) num = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6)) num = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7)) num = 6;
        if (Input.GetKeyDown(KeyCode.Alpha8)) num = 7;
        if (Input.GetKeyDown(KeyCode.Alpha9)) num = 8;
        return num;
    }
}
