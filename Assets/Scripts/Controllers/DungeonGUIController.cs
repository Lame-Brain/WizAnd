using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGUIController : MonoBehaviour
{
    public Level_Logic ThisLevel;
    public GameObject CharacterScreen_PF;

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

    public void GoUpLevel()
    {

    }

    public void GoDownLevel()
    {

    }

    public void InspectCharacter(int _p)
    {
        GameObject _go = Instantiate(CharacterScreen_PF, transform);
        _go.GetComponent<CharacterScreenController>().selected_character = _p;
        _go.GetComponent<CharacterScreenController>().UpdateCharacterScreen();
    }
}
