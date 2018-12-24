using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Dropdown list;
    public Dropdown list2;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play()
    {
        DataContainerScript.instance.Players = list.value + 2;
        DataContainerScript.instance.Humans = SetHumans(list2.value);
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public bool SetHumans(int i)
    {
        if (i == 0)
            return false;
        return true;
    }
}
