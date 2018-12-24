using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript instance;
    private const float SPEED = 10.0f;

    private bool isR = false;
    private bool isL = false;
    private bool isU = false;
    private bool isD = false;

    // Use this for initialization
    void Start ()
    {
        if (instance == null)
            instance = this;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Translate(new Vector3(0, 0, -1) * SPEED * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Translate(new Vector3(0, 0, 1) * SPEED * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(new Vector3(1, 0, 0) * SPEED * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(new Vector3(-1, 0, 0) * SPEED * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveCamera(new Vector3(0, 0, 0));
        }
        */
        if (isU)
        {
            GoU();
        }
        else if (isD)
        {
            GoD();
        }

        if (isR)
        {
            GoR();
        }
        else if (isL)
        {
            GoL();
        }
    }

    public void MoveCamera(Vector3 pos)
    {
        gameObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }

    public void ResteCamera()
    {
        MoveCamera(new Vector3(0, 10, 5));
    }

    private void GoR()
    {
        gameObject.transform.Translate(new Vector3(-1, 0, 0) * SPEED * Time.deltaTime, Space.World);
    }

    private void GoL()
    {
        gameObject.transform.Translate(new Vector3(1, 0, 0) * SPEED * Time.deltaTime, Space.World);
    }

    private void GoU()
    {
        gameObject.transform.Translate(new Vector3(0, 0, -1) * SPEED * Time.deltaTime, Space.World);
    }

    public void GoD()
    {
        gameObject.transform.Translate(new Vector3(0, 0, 1) * SPEED * Time.deltaTime, Space.World);
    }

    public void onPointerDownU()
    {
        isU = true;
    }

    public void onPointerUpU()
    {
        isU = false;
    }

    public void onPointerDownD()
    {
        isD = true;
    }

    public void onPointerUpD()
    {
        isD = false;
    }

    public void onPointerDownR()
    {
        isR = true;
    }

    public void onPointerUpR()
    {
        isR = false;
    }

    public void onPointerDownL()
    {
        isL = true;
    }

    public void onPointerUpL()
    {
        isL = false;
    }
}
