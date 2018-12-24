using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public static EnemyScript instance;

    private ControlStatesScript enemyState;

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

    private void ChangeControlState(ControlStatesScript newState)
    {
        enemyState = newState;
        enemyState.OnStateEnter();
    }

    public void Play()
    {
        ChangeControlState(new PlayState());

        StartCoroutine(playCoroutine());
    }

    private IEnumerator playCoroutine()
    {
        BoxScript g = null;
        bool canPlay = true;
        while (canPlay)
        {
            g = PlanMove();
            if (g != null)
            {
                canPlay = true;
                yield return new WaitForSeconds(1);
                Vector3 destination = g.gameObject.transform.position;
                destination = new Vector3(destination.x, destination.y + 10, destination.z + 5);
                while (CameraScript.instance.gameObject.transform.position != destination)
                {
                    CameraScript.instance.gameObject.transform.position = Vector3.MoveTowards(
                        CameraScript.instance.gameObject.transform.position, destination, 5.0f * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(0.5f);
                enemyState.BoxAction(g);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                canPlay = false;
                yield return new WaitForSeconds(1);
            }
        }
        enemyState.ConfirmAction();
    }

    private BoxScript PlanMove()
    {
        if (BoxManagerScript.instance.FreeBoxesInGame.Count ==  1)
        {
            GameManagerScript.instance.CurrentPlayer.ChooseBoxToPlay(HandSeek());
            //enemyState.BoxAction(BoxManagerScript.instance.FreeBoxesInGame[0]);
            return BoxManagerScript.instance.FreeBoxesInGame[0];
        }
        else
        {
            int i = HandSeek2();
            if (i >= 0)
            {
                //enemyState.BoxAction(BoxManagerScript.instance.FreeBoxesInGame[i]);
                return BoxManagerScript.instance.FreeBoxesInGame[i];
            }
        }
        return null;
    }

    private int HandSeek()
    {
        List<int[]> hand = GameManagerScript.instance.CurrentPlayer.BoxesInHand;

        List<int> indexes = new List<int>();

        for (int j = 0; j < hand.Count; j++)
        {
            List<int> ind = new List<int>();
            for (int i = 0; i < hand.Count; i++)
            {
                if (i != j)
                {
                    if (hand[i][0] == hand[j][0] && hand[i][1] != hand[j][1])
                    {
                        ind.Add(i);
                    }
                }
            }
            if (ind.Count > indexes.Count)
            {
                indexes = ind;
            }
        }

        for (int j = 0; j < hand.Count; j++)
        {
            List<int> ind = new List<int>();
            for (int i = 0; i < hand.Count; i++)
            {
                if (i != j)
                {
                    if (hand[i][1] == hand[j][1] && hand[i][0] != hand[j][0])
                    {
                        ind.Add(i);
                    }
                }
            }
            if (ind.Count > indexes.Count)
            {
                indexes = ind;
            }
        }

        if (indexes.Count > 1)
        {
            return indexes[Random.Range(0, indexes.Count - 1)];
        }
        return 0;
    }

    private int HandSeek2()
    {
        int indexHand = 0;
        int indexBox = 0;
        int max = 0;

        List<int[]> hand = GameManagerScript.instance.CurrentPlayer.BoxesInHand;
        List<BoxScript> tab = BoxManagerScript.instance.FreeBoxesInGame;
        int[,] pointTable = new int[tab.Count, 2];
        for (int i = 0; i < tab.Count; i++)
        {
            pointTable[i, 0] = 0;
            pointTable[i, 1] = 0;
        }

        for (int i = 0; i < hand.Count; i++)
        {
            GameManagerScript.instance.CurrentPlayer.ChooseBoxToPlay(i);
            for (int j = 0; j < tab.Count; j++)
            {
                if (tab[j].FreePlace)
                {
                    int col = BoxManagerScript.instance.CurrentColorIndex;
                    int sha = BoxManagerScript.instance.CurrentShapeIndex;
                    if (tab[j].CanBePlaced1(sha, col) && tab[j].CanBePlaced3() && tab[j].CanBePlaced2(sha, col))
                    {
                        List<BoxScript> boxList = new List<BoxScript>();

                        boxList.Add(tab[j]);

                        if (BoxManagerScript.instance.GetPlayedBoxes().Count > 0)
                        {
                            for (int z = 0; z < BoxManagerScript.instance.GetPlayedBoxes().Count; z++)
                            {
                                boxList.Add(BoxManagerScript.instance.GetPlayedBoxes()[z]);
                            }
                        }
                        int point = PointsScript.instance.CountPoints(boxList);
                        if (point > pointTable[j, 0])
                        {
                            pointTable[j, 0] = point;
                            pointTable[j, 1] = i;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < pointTable.GetLength(0); i++)
        {
            if (pointTable[i, 0] > max)
            {
                max = pointTable[i, 0];
                indexHand = pointTable[i, 1];
                indexBox = i;
            }
        }

        if (max <= 0)
        {
            return -1;
        }
        else
        {
            GameManagerScript.instance.CurrentPlayer.ChooseBoxToPlay(indexHand);
            return indexBox;
        }
    }
}
