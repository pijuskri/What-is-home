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
    public GameObject itemObject;
    public ItemDef(string name, string type)
    {
        this.name = name;
        this.type = type;
    }
    public ItemDef() { }
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
    public Item()
    {
    }
}
public class Player : MonoBehaviour
{
    #region camera
    enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    RotationAxes axes = RotationAxes.MouseXAndY;
    float sensitivityX = 4F;
    float sensitivityY = 4F;
    float minimumX = -360F;
    float maximumX = 360F;
    float minimumY = -70F;
    float maximumY = 70F;
    float rotationY = 0F;
    #endregion

    #region publics in editor
    public GameObject playerObject;
    Rigidbody playerRigid;
    public GameObject itemDisplayPrefab;
    public GameObject InventoryContainer;
    public GameObject InventoryPanel;
    public GameObject hotbarUI;
    public GameObject[] hotbarItems;
    public GameObject[] itemObjectPrefabs;
    [HideInInspector] public GameObject currentItemObject;
    public Transform HandSocket;
    #endregion

    [HideInInspector] public Items items;
    [HideInInspector] public List<Item> inventory;
    [HideInInspector] public int[] hotbar = new int[9];

    public int currentItemIndex = 0;
    float speed = 3f;
    bool invOpen = false;
    float coolDown = 0;
    GameObject placeObjectPreview;

    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            hotbar[i] = -1;
        }
        
        playerRigid = playerObject.GetComponent<Rigidbody>();
        playerRigid.freezeRotation = true;

        items = JsonUtility.FromJson<Items>(File.ReadAllText( Application.dataPath + "/items.json"));
        InstantiateItems();
        inventory = new List<Item>();
        inventory.Add(new Item(items.items[0], 10));
        inventory.Add(new Item(items.items[1], 1));
        PopulateInventory();
        InventoryPanel.SetActive(false);
        Cursor.visible = false;
        hotbarItems[0].GetComponent<Image>().color = new Color(255, 255, 255, 0.7f);
    }


    void Update()
    {
        playerRigid.velocity = (Input.GetAxis("Horizontal") * playerObject.transform.right + Input.GetAxis("Vertical") * playerObject.transform.forward) * speed;
        if(!invOpen)CameraLook();
        transform.position = new Vector3(playerObject.transform.position.x + 0.1f, playerObject.transform.position.y + 0.5f, playerObject.transform.position.z);
        if (Input.GetKeyDown(KeyCode.Space)) playerRigid.AddForce(Vector3.up * 100);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (invOpen) { invOpen = false; InventoryPanel.SetActive(false); Cursor.visible = false; }
            else { invOpen = true; InventoryPanel.SetActive(true); Cursor.visible = true; }
        }
        UpdateHotbar();
        HotbarSelection();
       
        if ( hotbar[currentItemIndex] != -1 && !invOpen)
        {
            Item currentItem = inventory[hotbar[currentItemIndex]];
            if (currentItem.ItemDef.name == "Axe" && coolDown < 0 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.tag == "Tree")
                    {
                        AddItemToInventory(new Item(FindItemByName("Wood"), 1));
                        hit.collider.gameObject.GetComponent<TreeLogic>().woodLeft--;
                        coolDown = 2;
                    }
                }
            }
            if (currentItem.ItemDef.type == "block")
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 5))
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        currentItem.amount--;
                        placeObjectPreview.GetComponent<Collider>().enabled = true;
                        placeObjectPreview = null;
                    }
                    else
                    {
                        if (placeObjectPreview == null)
                        {
                            placeObjectPreview = Instantiate(currentItem.ItemDef.itemObject, hit.point, new Quaternion());
                            placeObjectPreview.GetComponent<Collider>().enabled = false;
                        }
                        else placeObjectPreview.transform.position = hit.point;
                    }
                }
                else if (placeObjectPreview != null) Destroy(placeObjectPreview);
            }
        }
        coolDown -= Time.deltaTime;
    }
    void CameraLook()
    {
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
    void InstantiateItems()
    {
        int index = 0;
        foreach (var item in items.items)
        {
            item.itemObject = itemObjectPrefabs[index];
            index++;
        }
    }
    void AddItemToInventory(Item itemAdd)
    {
        int index = 0;
        foreach (var item in inventory)
        {
            if (item.ItemDef == itemAdd.ItemDef)
            {
                item.amount += itemAdd.amount;
                Text text = InventoryContainer.transform.GetChild(index).GetComponentInChildren<Text>();

                if (text.text.Contains(itemAdd.ItemDef.name))
                {
                    text.text = itemAdd.ItemDef.name + ":" + item.amount;
                }
                return;
            }
            index++;
        }
        inventory.Add(itemAdd);
        GameObject temp = Instantiate(itemDisplayPrefab, InventoryContainer.transform);
        InventoryItem inventoryItem = temp.GetComponent<InventoryItem>();
        Text text1 = temp.GetComponentInChildren<Text>();
        inventoryItem.hotbar = hotbar;
        inventoryItem.inventoryIndex = index;
        text1.text = itemAdd.ItemDef.name + ":" + itemAdd.amount;


    }
    #region UI
    void PopulateInventory()
    {
        int index = 0;
        foreach (Transform item in InventoryContainer.transform)
        {
            Destroy(item.gameObject);
        }
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
                Item currentItemIndex = inventory[hotbar[index]];
                Text text = hotbarItem.GetComponentInChildren<Text>();
                text.text = currentItemIndex.ItemDef.name + ":" + currentItemIndex.amount;
            }
            else hotbarItem.GetComponentInChildren<Text>().text = "";

            index++;
        }
    }
    void UpdateHand()
    {
        Destroy(currentItemObject);
        currentItemObject = null;
        if (hotbar[currentItemIndex] != -1)
        {
            currentItemObject = Instantiate(inventory[hotbar[currentItemIndex]].ItemDef.itemObject, HandSocket);
            Collider col = currentItemObject.GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }
        
    }
    void HotbarSelection()
    {
        if (NumKey() != -1 && !invOpen)
        {
            hotbarItems[currentItemIndex].GetComponent<Image>().color = new Color(255, 255, 255, 0.39f);
            currentItemIndex = NumKey();
            hotbarItems[currentItemIndex].GetComponent<Image>().color = new Color(255, 255, 255, 0.7f);
            UpdateHand();
        }
    }
    #endregion
    #region Utils
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
    public ItemDef FindItemByName(string name)
    {
        ItemDef itemResult = new ItemDef();
        foreach (var item in items.items)
        {
            if (item.name == name) { itemResult = item;break; }
        } 
        return itemResult;
    }
    #endregion
}
