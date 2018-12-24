using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ControlStatesScript
{
    void OnStateEnter();
    void BoxAction(BoxScript box);
    void ButtonAction(int index);
    void ConfirmAction();
}

public class PlayState : ControlStatesScript
{
    public void BoxAction(BoxScript box)
    {
        if (GameManagerScript.instance.CurrentPlayer.HasBoxesInHand())
        {
            if (box.FreePlace)
            {
                int col = BoxManagerScript.instance.CurrentColorIndex;
                int sha = BoxManagerScript.instance.CurrentShapeIndex;
                if (box.CanBePlaced1(sha, col) && box.CanBePlaced3() && box.CanBePlaced2(sha, col))
                {
                    box.PlaceBox(sha, col);
                    BoxManagerScript.instance.FreeBoxesInGame.Remove(box);
                    BoxManagerScript.instance.AddToPlayedBoxes(box);
                    GameManagerScript.instance.CurrentPlayer.RemoveBoxFromHand();
                    GameManagerScript.instance.CurrentPlayer.UpdatePlayerBoxes();
                    GuiManagerScript.instance.BlockTrade(true);
                    GuiManagerScript.instance.BlockEnd(false);
                }
            }
        }
    }

    public void ButtonAction(int index)
    {
        GameManagerScript.instance.CurrentPlayer.ChooseBoxToPlay(index);
    }

    public void OnStateEnter()
    {
        GameManagerScript.instance.CurrentPlayer.ResetTrade();
        GameManagerScript.instance.CurrentPlayer.UpdatePlayerBoxes();
        GameManagerScript.instance.CurrentPlayer.ChooseBoxToPlay(0);
        if (BoxManagerScript.instance.GetPlayedBoxes().Count == 0)
        {
            if (!BoxManagerScript.instance.IsStackEmpty())
            {
                GuiManagerScript.instance.BlockTrade(false);
            }
            else
            {
                GuiManagerScript.instance.BlockTrade(true);
            }
            GuiManagerScript.instance.BlockEnd(true);
        }
    }

    public void ConfirmAction()
    {
        GameManagerScript.instance.CurrentPlayer.Points += 
            PointsScript.instance.CountPoints(BoxManagerScript.instance.GetPlayedBoxes());
        GuiManagerScript.instance.UpdatePointView(GameManagerScript.instance.Players);
        BoxManagerScript.instance.ResetPlayedBoxes();
        GameManagerScript.instance.CurrentPlayer.RefillHand();
        GameManagerScript.instance.EndTurn();
    }
}

public class TradeState : ControlStatesScript
{
    public void BoxAction(BoxScript box)
    {

    }

    public void ButtonAction(int index)
    {
        GameManagerScript.instance.CurrentPlayer.SelectBox(index);
        if (GameManagerScript.instance.CurrentPlayer.GetTradingBoxes().Count > 0)
        {
            GuiManagerScript.instance.BlockEnd(false);
        }
        else
        {
            GuiManagerScript.instance.BlockEnd(true);
        }
    }

    public void OnStateEnter()
    {
        GameManagerScript.instance.CurrentPlayer.ResetTrade();
        GameManagerScript.instance.CurrentPlayer.UpdatePlayerBoxes();
        GuiManagerScript.instance.BlockEnd(true);
    }

    public void ConfirmAction()
    {
        GameManagerScript.instance.CurrentPlayer.TradeBoxes();
        BoxManagerScript.instance.ResetPlayedBoxes();
        GameManagerScript.instance.EndTurn();
    }
}

public class EmptyState : ControlStatesScript
{
    public void BoxAction(BoxScript box)
    {

    }

    public void ButtonAction(int index)
    {

    }

    public void OnStateEnter()
    {

    }

    public void ConfirmAction()
    {

    }
}
