using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    private int playersCount = 4;
    private bool humans = true;

    private List<PlayerScript> players;
    private PlayerScript currentPlayer;

    private int currentPlayerIndex;

    private ControlStatesScript controlstate;

    public PlayerScript CurrentPlayer
    {
        get
        {
            return currentPlayer;
        }

        set
        {
            currentPlayer = value;
        }
    }

    public List<PlayerScript> Players
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

    // Use this for initialization
    void Start ()
    {
        if (instance == null)
            instance = this;
        Players = new List<PlayerScript>();
        ChangeControlState(new EmptyState());
        Invoke("SetUpGame", 0.1f);	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //PC
        /*
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 screenPos = Input.mousePosition;
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(screenPos);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    BoxScript box = hitInfo.transform.gameObject.GetComponent<BoxScript>();
                    PlayBox(box);
                }
            }
        }*/
        
        //Mobile
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Vector3 screenPos = Input.GetTouch(0).position;
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(screenPos);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    BoxScript box = hitInfo.transform.gameObject.GetComponent<BoxScript>();
                    PlayBox(box);
                }
            }
        }      
    }

    public void ChangeControlState(ControlStatesScript newState)
    {
        controlstate = newState;
        controlstate.OnStateEnter();
    }

    public void PlayButton(int index)
    {
        controlstate.ButtonAction(index);
    }

    public void PlayBox(BoxScript box)
    {
        controlstate.BoxAction(box);
    }

    public void ConfirmAction()
    {
        controlstate.ConfirmAction();
    }

    public void EndTurn()
    {
        if (!CurrentPlayer.HasBoxesInHand())
        {
            GuiManagerScript.instance.End();
            ChangeControlState(new EmptyState());
        }
        else
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % playersCount;
            CurrentPlayer = Players[currentPlayerIndex];
            GuiManagerScript.instance.UpdateCurrentPlayer(currentPlayerIndex);
            ChangeControlState(new EmptyState());
            if (humans)
            {
                GuiManagerScript.instance.HideUI(false);
            }
            else
            {
                if (currentPlayerIndex == 0)
                {
                    GuiManagerScript.instance.HideUI2(true);
                    ChangeControlState(new PlayState());
                }
                else
                {
                    GuiManagerScript.instance.HideUI2(false);
                    EnemyScript.instance.Play();
                }
            }
        }
    }

    private void SetUpGame()
    {
        humans = DataContainerScript.instance.Humans;
        playersCount = DataContainerScript.instance.Players;
        Destroy(DataContainerScript.instance.gameObject);
        SetUpPlayers(playersCount);
        GuiManagerScript.instance.SetUp(playersCount);
        CurrentPlayer.UpdatePlayerBoxes();
        ChangeControlState(new PlayState());
        GuiManagerScript.instance.cameraPanel.SetActive(true);
    }

    private void SetUpPlayers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Players.Add(gameObject.AddComponent<PlayerScript>());
            Players[i].SetUpPlayer();
        }

        CurrentPlayer = Players[0];
        currentPlayerIndex = 0;
    }
}
