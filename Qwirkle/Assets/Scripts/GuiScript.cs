using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void action(ControlStatesScript newState);
public delegate void action2();

public class GuiScript : MonoBehaviour
{
    public static GuiScript instance;
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

    public GameObject CreatePanel(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
        Vector3 localScale, Vector3 localPosition, Vector2 sizeDelta, Vector2 anchoredPosition,
        Sprite image, Color32 color)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent.transform);
        panel.AddComponent<RectTransform>();
        panel.AddComponent<Image>();

        panel.GetComponent<Image>().sprite = image;
        panel.GetComponent<Image>().type = Image.Type.Sliced;
        panel.GetComponent<Image>().color = color;

        panel.GetComponent<RectTransform>().anchorMin = anchorMin;
        panel.GetComponent<RectTransform>().anchorMax = anchorMax;
        panel.GetComponent<RectTransform>().pivot = pivot;
        panel.GetComponent<RectTransform>().localScale = localScale;
        panel.GetComponent<RectTransform>().localPosition = localPosition;
        panel.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        panel.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        return panel;
    }

    public GameObject CreateText(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
      Vector3 localScale, Vector3 localPosition, Vector2 sizeDelta, Vector2 anchoredPosition, string text, Color32 color)
    {
        GameObject textBlock = new GameObject(name);
        textBlock.transform.SetParent(parent.transform);
        textBlock.AddComponent<RectTransform>();
        textBlock.AddComponent<Text>();

        textBlock.GetComponent<Text>().resizeTextForBestFit = true;
        textBlock.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        textBlock.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        textBlock.GetComponent<Text>().fontStyle = FontStyle.Bold;
        textBlock.GetComponent<Text>().color = color;
        textBlock.GetComponent<Text>().text = text;

        textBlock.GetComponent<RectTransform>().anchorMin = anchorMin;
        textBlock.GetComponent<RectTransform>().anchorMax = anchorMax;
        textBlock.GetComponent<RectTransform>().pivot = pivot;
        textBlock.GetComponent<RectTransform>().localScale = localScale;
        textBlock.GetComponent<RectTransform>().localPosition = localPosition;
        textBlock.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        textBlock.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        return textBlock;
    }

    public GameObject CreateButton(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
    Vector3 localScale, Vector3 localPosition, Vector2 sizeDelta, Vector2 anchoredPosition, Sprite image, Color32 color)
    {
        GameObject button = new GameObject(name);
        button.transform.SetParent(parent.transform);
        button.AddComponent<RectTransform>();
        button.AddComponent<Image>();
        button.AddComponent<Button>();

        button.GetComponent<Image>().sprite = image;
        button.GetComponent<Image>().type = Image.Type.Sliced;
        button.GetComponent<Image>().color = color;

        button.GetComponent<RectTransform>().anchorMin = anchorMin;
        button.GetComponent<RectTransform>().anchorMax = anchorMax;
        button.GetComponent<RectTransform>().pivot = pivot;
        button.GetComponent<RectTransform>().localScale = localScale;
        button.GetComponent<RectTransform>().localPosition = localPosition;
        button.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        button.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        return button;
    }

    public GameObject[] FillWithButtons(GameObject panel, List<int[]> box)
    {
        GameObject[] buttons = new GameObject[box.Count];
        float buttonW = Mathf.Abs(panel.GetComponent<RectTransform>().sizeDelta.x) / 6.0f;
        for (int i = 0; i < box.Count; i++)
        {
            Color32 col = DataScript.instance.GetColor(box[i][0]);
            Sprite shape = DataScript.instance.GetGuiShape(box[i][1]);
            GameObject but = CreateButton(panel, ("Box" + i.ToString()),
                new Vector2(1, 1), new Vector2(1, 1), new Vector2(1, 1),
               new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(buttonW, buttonW),
               new Vector2(-(i * buttonW), 0), shape, col);

            int index = i;
            but.GetComponent<Button>().onClick.AddListener(delegate { GameManagerScript.instance.PlayButton(index); });
            buttons[i] = but;
        }
        return buttons;
    }

    public GameObject[] FillWithButtons(GameObject panel, int count, Sprite image, Color32 col)
    {
        GameObject[] buttons = new GameObject[count];
        float buttonW = Mathf.Abs(panel.GetComponent<RectTransform>().sizeDelta.x) / count;
        for (int i = 0; i < count; i++)
        {
            GameObject but = CreateButton(panel, ("Button" + i.ToString()),
                new Vector2(1, 1), new Vector2(1, 1), new Vector2(1, 1),
               new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(buttonW, buttonW),
               new Vector2(-(i * buttonW), 0), image, col);
            buttons[i] = but;
        }
        return buttons;
    }

    public GameObject[] FillWithText(GameObject panel, int count, string _text, Color32 col)
    {
        GameObject[] text = new GameObject[count];
        float buttonW = Mathf.Abs(panel.GetComponent<RectTransform>().sizeDelta.x) / count;
        for (int i = 0; i < count; i++)
        {
            GameObject tx = CreateText(panel, ("Text" + i.ToString()),
                new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1),
               new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(buttonW, buttonW),
               new Vector2((i * buttonW), 0), _text + (i + 1).ToString() + ":" + "\n" + "0", col);
            text[i] = tx;
        }
        return text;
    }

    public void SetAction(GameObject button, action act, ControlStatesScript newState)
    {
        button.GetComponent<Button>().onClick.AddListener(delegate { act.Invoke(newState); });
    }

    public void SetAction(GameObject button, action2 act)
    {
        button.GetComponent<Button>().onClick.AddListener(delegate { act.Invoke(); });
    }
}
