using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float speed = 3f;
    void Start()
    {
        playerRigid = playerObject.GetComponent<Rigidbody>();
        playerRigid.freezeRotation = true;
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        //playerRigid.AddForce( Input.GetAxis("Horizontal") * playerObject.transform.right);
        //playerRigid.AddForce(Input.GetAxis("Vertical") * playerObject.transform.forward);
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
