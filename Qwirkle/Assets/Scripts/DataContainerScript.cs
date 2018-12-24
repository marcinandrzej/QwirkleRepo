using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainerScript : MonoBehaviour
{
    public static DataContainerScript instance;
    private int players = 2;
    private bool humans = false;

    public int Players
    {
        get
        {
            return players;
        }

        set
        {
            players = value;
        }
    }

    public bool Humans
    {
        get
        {
            return humans;
        }

        set
        {
            humans = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        if (instance == null)
            instance = this;
        DontDestroyOnLoad(gameObject);		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


}
