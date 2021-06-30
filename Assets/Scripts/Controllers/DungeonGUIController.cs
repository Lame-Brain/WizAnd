using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGUIController : MonoBehaviour
{
    public Level_Logic ThisLevel;

    public void UpdateGUI()
    {
        GameManager.GAME.UpdatePartyPanel();
    }

    public void ButtonClicked(string button_ID)
    {
        if(button_ID == "Forward")
        {
            ThisLevel.MoveForward();
        }

        if(button_ID == "Left")
        {
            ThisLevel.TurnLeft();
        }

        if(button_ID == "Right")
        {
            ThisLevel.TurnRight();
        }

        if(button_ID == "Camp")
        {
            ThisLevel.CampMenu();
        }

        if(button_ID == "Inspect")
        {
            ThisLevel.InspectButton();
        }

    }
}
