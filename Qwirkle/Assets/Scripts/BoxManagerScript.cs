using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManagerScript : MonoBehaviour
{
    public static BoxManagerScript instance;

    public BoxScript startingBox;

    private List<int[]> boxesInStack;
    private List<BoxScript> playedBoxes;
    private List<BoxScript> freeBoxesInGame;

    private int currentColorIndex;
    private int currentShapeIndex;

    public int CurrentColorIndex
    {
        get
        {
            return currentColorIndex;
        }

        set
        {
            currentColorIndex = value;
        }
    }

    public int CurrentShapeIndex
    {
        get
        {
            return currentShapeIndex;
        }

        set
        {
            currentShapeIndex = value;
        }
    }

    public List<BoxScript> FreeBoxesInGame
    {
        get
        {
            return freeBoxesInGame;
        }

        set
        {
            freeBoxesInGame = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        if (instance == null)
            instance = this;

        boxesInStack = CreateStack();
        playedBoxes = new List<BoxScript>();
        FreeBoxesInGame = new List<BoxScript>();
        FreeBoxesInGame.Add(startingBox);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private List<int[]> CreateStack()
    {
        List<int[]> l = new List<int[]>();
        for (int g = 0; g < 3; g++)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    int[] box = new int[2];
                    box[0] = i;
                    box[1] = j;
                    l.Add(box);
                }
            }
        }
        return l;
    }

    public int[] GetRandomBox()
    {
        int index = Random.Range(0, boxesInStack.Count);
        int[] box = boxesInStack[index];
        boxesInStack.RemoveAt(index);
        return box;
    }

    public List<int[]> TradeBoxes(List<int[]> listToTrade)
    {
        List<int[]> tradedboxes = new List<int[]>();
        if (listToTrade.Count <= boxesInStack.Count)
        {
            for (int i = 0; i < listToTrade.Count; i++)
            {
                tradedboxes.Add(GetRandomBox());
            }
            for (int i = 0; i < listToTrade.Count; i++)
            {
                boxesInStack.Add(listToTrade[i]);
            }
        }
        else
        {
            for (int i = 0; i < listToTrade.Count; i++)
            {
                boxesInStack.Add(listToTrade[i]);
            }
            for (int i = 0; i < listToTrade.Count; i++)
            {
                tradedboxes.Add(GetRandomBox());
            }
        }
        return tradedboxes;
    }

    public bool IsStackEmpty()
    {
        if (boxesInStack.Count > 0)
            return false;
        return true;
    }

    public int StackCount()
    {
        return boxesInStack.Count;
    }

    public void ResetPlayedBoxes()
    {
        playedBoxes = new List<BoxScript>();
    }

    public void AddToPlayedBoxes(BoxScript box)
    {
        playedBoxes.Add(box);
    }

    public List<BoxScript> GetPlayedBoxes()
    {
        return playedBoxes;
    }
}
