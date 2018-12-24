using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GuiManagerScript : MonoBehaviour
{
    public static GuiManagerScript instance;

    public GameObject cameraPanel;
    public GameObject nextPanel;
    public GameObject endChoice;

    private GameObject boxesPanel;
    private GameObject[] boxesButtons;

    private GameObject statesPanel;
    private GameObject[] statesButtons;

    private GameObject pointsPanel;
    private GameObject[] pointsText;
    private GameObject stackText;

    private GameObject endPanel;
    private GameObject endText;
    private GameObject endButton;

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

    public void SetUp(int players)
    {
        boxesPanel = GuiScript.instance.CreatePanel(gameObject, "BoxesPanel", new Vector2(1, 0), new Vector2(1, 0),
            new Vector2(1, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(360, 60), new Vector2(0, 0),
            DataScript.instance.panelImage, new Color32(0, 0, 0, 0));
        statesPanel = GuiScript.instance.CreatePanel(gameObject, "StatesPanel", new Vector2(1, 1), new Vector2(1, 1),
            new Vector2(1, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(180, 60), new Vector2(0, 0),
            DataScript.instance.panelImage, new Color32(0, 0, 0, 0));
        statesButtons = GuiScript.instance.FillWithButtons(statesPanel, 3,
            DataScript.instance.panelImage, new Color32(255, 255, 255, 255));
        statesButtons[0].GetComponent<Image>().sprite = DataScript.instance.endTurnImage;
        statesButtons[1].GetComponent<Image>().sprite = DataScript.instance.playImage;
        statesButtons[2].GetComponent<Image>().sprite = DataScript.instance.tradeImage;
        GuiScript.instance.SetAction(statesButtons[0], new action2(GameManagerScript.instance.ConfirmAction));
        GuiScript.instance.SetAction(statesButtons[1], new action(GameManagerScript.instance.ChangeControlState),
            new PlayState());
        GuiScript.instance.SetAction(statesButtons[2], new action(GameManagerScript.instance.ChangeControlState),
            new TradeState());
        pointsPanel = GuiScript.instance.CreatePanel(gameObject, "PointsPanel", new Vector2(0, 1), new Vector2(0, 1),
            new Vector2(0, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0),
            new Vector2(80 * GameManagerScript.instance.Players.Count, 90),
            new Vector2(0, 0), DataScript.instance.panelImage, new Color32(0, 0, 0, 120));
        pointsText = GuiScript.instance.FillWithText(pointsPanel, players, "Player", new Color32(255, 255, 255, 255));
        stackText = GuiScript.instance.CreateText(pointsPanel, "StackText", new Vector2(0.5f, 0), new Vector2(0.5f, 0),
            new Vector2(0.5f, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(160, 30), new Vector2(0, 0),
            "Blocks in stack: " + BoxManagerScript.instance.StackCount().ToString(), new Color32(255, 255, 255, 255));
        UpdateCurrentPlayer(0);
        HideUI(true);
    }

    public void UpdateBoxes(List<int[]> boxes)
    {
        if (boxesButtons != null)
        {
            foreach (GameObject gO in boxesButtons)
            {
                Destroy(gO);
            }
        }
        boxesButtons = GuiScript.instance.FillWithButtons(boxesPanel, boxes);
    }

    public void ChooseButton(int index, bool up)
    {
        if (index < boxesButtons.Length)
        {
            Vector2 pos = boxesButtons[index].GetComponent<RectTransform>().anchoredPosition;

            if (up)
            {
                boxesButtons[index].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x, 15);
            }
            else
            {
                boxesButtons[index].GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x, 0);
            }
        }
    }

    public void BlockTrade(bool block)
    {
        statesButtons[2].GetComponent<Button>().enabled = !block;
        if (block)
        {
            statesButtons[2].GetComponent<Image>().sprite = DataScript.instance.blockImage;
        }
        else
        {
            statesButtons[2].GetComponent<Image>().sprite = DataScript.instance.tradeImage;
        }
    }

    public void BlockEnd(bool block)
    {
        statesButtons[0].GetComponent<Button>().enabled = !block;

        if (block)
        {
            statesButtons[0].GetComponent<Image>().sprite = DataScript.instance.blockImage;
        }
        else
        {
            statesButtons[0].GetComponent<Image>().sprite = DataScript.instance.endTurnImage;
        }
    }

    public void BlockPlay(bool block)
    {
        statesButtons[1].GetComponent<Button>().enabled = !block;
        if (block)
        {
            statesButtons[1].GetComponent<Image>().sprite = DataScript.instance.blockImage;
        }
        else
        {
            statesButtons[1].GetComponent<Image>().sprite = DataScript.instance.playImage;
        }
    }

    public void UpdatePointView(List<PlayerScript> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            pointsText[i].GetComponent<Text>().text =
                "Player" + (i + 1).ToString() + ":" + "\n" + players[i].Points.ToString();
        }
    }

    public void UpdateStackView()
    {
        stackText.GetComponent<Text>().text = "Blocks in stack: " + BoxManagerScript.instance.StackCount().ToString();
    }

    public void UpdateCurrentPlayer(int index)
    {
        for (int i = 0; i < pointsText.Length; i++)
        {
            pointsText[i].GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        }
        pointsText[index].GetComponent<Text>().color = new Color32(0, 255, 0, 255);
    }

    public void HideUI(bool show)
    {
        boxesPanel.SetActive(show);
        statesPanel.SetActive(show);
        nextPanel.SetActive(!show);
        if (show)
        {
            GameManagerScript.instance.ChangeControlState(new PlayState());
        }
    }

    public void HideUI2(bool show)
    {
        boxesPanel.SetActive(show);
        statesPanel.SetActive(show);
        cameraPanel.SetActive(show);
    } 

    public void ShowEndChoice(bool show)
    {
        endChoice.SetActive(show);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void End()
    {
        cameraPanel.SetActive(false);
        pointsPanel.SetActive(false);
        boxesPanel.SetActive(false);
        statesPanel.SetActive(false);
        SetUpEndMenu();
    }

    public void SetUpEndMenu()
    {
        endPanel = GuiScript.instance.CreatePanel(gameObject, "EndPanel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(300, 300), new Vector2(0, 0),
                DataScript.instance.panelImage, new Color32(255, 255, 255, 120));
        endText = GuiScript.instance.CreateText(endPanel, "Text", new Vector2(0.5f, 1), new Vector2(0.5f, 1),
            new Vector2(0.5f, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(280, 200), new Vector2(0, 0),
            EndString(), new Color32(0, 0, 0, 255));
        endButton = GuiScript.instance.CreateButton(endPanel, "Button", new Vector2(0.5f, 0), new Vector2(0.5f, 0),
            new Vector2(0.5f, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(80, 80), new Vector2(0, 0),
            DataScript.instance.endTurnImage, new Color32(255, 255, 255, 255));
        endButton.GetComponent<Button>().onClick.AddListener(delegate { SceneManager.LoadScene("MainMenuScene"); });
    }

    private string EndString()
    {
        string text = "";
        for (int i = 0; i < GameManagerScript.instance.Players.Count; i++)
        {
            text += "Player " + (i + 1).ToString() + " : " +
                GameManagerScript.instance.Players[i].Points.ToString() + "\n";
        }
        return text;
    }
}
