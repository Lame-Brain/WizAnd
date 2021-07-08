using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGUIController : MonoBehaviour
{
    public Level_Logic ThisLevel;
    public GameObject CharacterScreen_PF;

    private void Start()
    {
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        GameObject _target_WP = null;

        if (GameManager.CONTEXT == "Down")
        {
            foreach (GameObject _this_wp in GameObject.FindGameObjectsWithTag("Teleport")) if (_this_wp.name == "FromAbove") _target_WP = _this_wp;
            _player.transform.position = _target_WP.transform.position;
            _player.transform.rotation = _target_WP.transform.rotation;
            GameManager.CONTEXT = "Dungeon";
        }
        if (GameManager.CONTEXT == "Up")
        {
            foreach (GameObject _this_wp in GameObject.FindGameObjectsWithTag("Teleport")) if (_this_wp.name == "FromBelow") _target_WP = _this_wp;
            _player.transform.position = _target_WP.transform.position;
            _player.transform.rotation = _target_WP.transform.rotation;
            GameManager.CONTEXT = "Dungeon";
        }

        if (_player.transform.rotation.eulerAngles.y >= 315 && _player.transform.rotation.eulerAngles.y <= 45) _player.GetComponent<Level_Logic>().facing = Level_Logic.direction.north;
        if (_player.transform.rotation.eulerAngles.y > 45 && _player.transform.rotation.eulerAngles.y < 135) _player.GetComponent<Level_Logic>().facing = Level_Logic.direction.east;
        if (_player.transform.rotation.eulerAngles.y >= 135 && _player.transform.rotation.eulerAngles.y <= 225) _player.GetComponent<Level_Logic>().facing = Level_Logic.direction.south;
        if (_player.transform.rotation.eulerAngles.y > 225 && _player.transform.rotation.eulerAngles.y < 315) _player.GetComponent<Level_Logic>().facing = Level_Logic.direction.west;
        _player.GetComponent<Level_Logic>().DetermineFacing();
    }

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

    public void GoToTown()
    {
        GameManager.GAME.EnterTown();
    }

    public void GoUpLevel()
    {
        GameManager.GAME.GoUpLevel();
    }

    public void GoDownLevel()
    {
        GameManager.GAME.GoDownLevel();
    }

    public void InspectCharacter(int _p)
    {
        GameObject _go = Instantiate(CharacterScreen_PF, transform);
        _go.GetComponent<CharacterScreenController>().selected_character = _p;
        _go.GetComponent<CharacterScreenController>().UpdateCharacterScreen();
    }
}
