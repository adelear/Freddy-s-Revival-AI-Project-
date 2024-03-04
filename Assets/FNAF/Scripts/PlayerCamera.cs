using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public bool lockCursor = true;
    public bool lockCamera = false;
    private float camX = 0;
    public GameObject rotateOnX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.Confined;

        if (!lockCamera)
        {
            float mX = Input.GetAxis("Mouse X");
            float mY = Input.GetAxis("Mouse Y");


            camX += mY;
            camX = Mathf.Clamp(camX, -75, 45);

            transform.eulerAngles = Vector3.up * mX + transform.eulerAngles;
            rotateOnX.transform.eulerAngles = new Vector3(-camX, rotateOnX.transform.eulerAngles.y, rotateOnX.transform.eulerAngles.z); 
        }

    }
}
