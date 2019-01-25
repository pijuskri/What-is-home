using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    ItemDef ItemDef;
    int amount;
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
    public Items items;
    public List<Item> inventory;
    float speed = 3f;
    void Start()
    {
        playerRigid = playerObject.GetComponent<Rigidbody>();
        playerRigid.freezeRotation = true;

        items = JsonUtility.FromJson<Items>(File.ReadAllText( Application.dataPath + "/items.json"));
        inventory = new List<Item>();
        inventory.Add(new Item(items.items[0], 10));
    }


    void Update()
    {
        playerRigid.velocity = (Input.GetAxis("Horizontal") * playerObject.transform.right + Input.GetAxis("Vertical") * playerObject.transform.forward) * speed;
        CameraLook();
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
}
