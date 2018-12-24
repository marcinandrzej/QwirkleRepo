using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataScript : MonoBehaviour
{
    public static DataScript instance;

    public Color32[] colors;
    public Material[] shapes;
    public Sprite[] guiShapes;
    public Sprite panelImage;
    public Sprite endTurnImage;
    public Sprite tradeImage;
    public Sprite playImage;
    public Sprite blockImage;

    // Use this for initialization
    void Start ()
    {
        if (instance == null)
            instance = this;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public Material GetShape(int index)
    {
        return shapes[index];
    }

    public Color32 GetColor(int index)
    {
        return colors[index];
    }

    public Sprite GetGuiShape(int index)
    {
        return guiShapes[index];
    }
}
