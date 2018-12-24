using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private List<int[]> boxesInHand;
    private List<int[]> boxestoTrade;

    private int points;
    private int choosenBoxIndex;

    public int Points
    {
        get
        {
            return points;
        }

        set
        {
            points = value;
        }
    }

    public int ChoosenBoxIndex
    {
        get
        {
            return choosenBoxIndex;
        }

        set
        {
            choosenBoxIndex = value;
        }
    }

    public List<int[]> BoxesInHand
    {
        get
        {
            return boxesInHand;
        }

        set
        {
            boxesInHand = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        Points = 0;
        ChoosenBoxIndex = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
       
	}

    public void SetUpPlayer()
    {
        BoxesInHand = new List<int[]>();
        boxestoTrade = new List<int[]>();

        for (int i = 0; i < 6; i++)
        {
            BoxesInHand.Add(BoxManagerScript.instance.GetRandomBox());
        }
    }

    public void ChooseBoxToPlay(int index)
    {
        GuiManagerScript.instance.ChooseButton(ChoosenBoxIndex, false);
        ChoosenBoxIndex = index;
        GuiManagerScript.instance.ChooseButton(ChoosenBoxIndex, true);
        BoxManagerScript.instance.CurrentColorIndex = BoxesInHand[index][0];
        BoxManagerScript.instance.CurrentShapeIndex = BoxesInHand[index][1];
    }

    public void RemoveBoxFromHand()
    {
        BoxesInHand.RemoveAt(ChoosenBoxIndex);
    }

    public void SelectBox(int index)
    {
        if (boxestoTrade.Contains(BoxesInHand[index]))
        {
            boxestoTrade.Remove(BoxesInHand[index]);
            GuiManagerScript.instance.ChooseButton(index, false);
        }
        else
        {
            boxestoTrade.Add(BoxesInHand[index]);
            GuiManagerScript.instance.ChooseButton(index, true);
        }
    }

    public void TradeBoxes()
    {
        List<int[]> l = new List<int[]>();
        l = BoxManagerScript.instance.TradeBoxes(boxestoTrade);
        foreach (int[] box in boxestoTrade)
        {
            BoxesInHand.Remove(box);
        }
        for (int i = 0; i < l.Count; i++)
        {
            BoxesInHand.Add(l[i]);
        }
    }

    public void RefillHand()
    {
        while (BoxesInHand.Count < 6 && !BoxManagerScript.instance.IsStackEmpty())
        {
            BoxesInHand.Add(BoxManagerScript.instance.GetRandomBox());
        }
        GuiManagerScript.instance.UpdateStackView();
    }

    public void ResetTrade()
    {
        boxestoTrade = new List<int[]>();
    }

    public List<int[]> GetTradingBoxes()
    {
        return boxestoTrade;
    }

    public void UpdatePlayerBoxes()
    {
        GuiManagerScript.instance.UpdateBoxes(BoxesInHand);
    }

    public bool HasBoxesInHand()
    {
        if (BoxesInHand.Count > 0)
            return true;
        return false;
    }
}
