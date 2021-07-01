using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Logic : MonoBehaviour
{
    public GameObject ThisLevel;
    
    public enum direction { north, east, south, west }
    public direction facing;    

    public bool moving, turning;
    private float _move_delay = 0.15f;
    private float _message_delay = 1f;
    private GameObject _Old_Tile;

    private void Start()
    {
        InitLevel();
    }


    public void InitLevel()
    {
        GameManager.GAME.UpdatePartyPanel();
        moving = false;
        turning = false;
        DetermineFacing();
    }

    public void DetermineFacing()
    {
        if (this.facing == direction.north) this.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (this.facing == direction.east) this.transform.rotation = Quaternion.Euler(0, 90, 0);
        if (this.facing == direction.south) this.transform.rotation = Quaternion.Euler(0, 180, 0);
        if (this.facing == direction.west) this.transform.rotation = Quaternion.Euler(0, -90, 0);

        //Check for POIs
        string _POI = "";
        if (this.facing == direction.north) _POI = ThisTile().GetComponent<TileClass>().northPOI;
        if (this.facing == direction.east) _POI = ThisTile().GetComponent<TileClass>().eastPOI;
        if (this.facing == direction.south) _POI = ThisTile().GetComponent<TileClass>().southPOI;
        if (this.facing == direction.west) _POI = ThisTile().GetComponent<TileClass>().westPOI;


    }

    public GameObject ThisTile()
    {
        GameObject _result = null;
        int _x = (int)this.transform.position.x / 7; int _y = (int)this.transform.position.z / 7; int _child = (_y * 20) + _x;
        _result = GameObject.FindGameObjectWithTag("Level").transform.GetChild(_child).gameObject;
        return _result;
    }





    public void ShortMessage(string _message)
    {
        GameObject _messagePanel = GameObject.FindGameObjectWithTag("GUI_Canvas").transform.Find("MessagePanel").gameObject;
        _messagePanel.SetActive(true);
        _messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = _message;
        StartCoroutine(CloseShortMessage(_messagePanel));
    }

    IEnumerator CloseShortMessage(GameObject _messagePanel)
    {
        yield return new WaitForSeconds(_message_delay);
        _messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "";
        _messagePanel.SetActive(false);
    }

    public void LongMessage(string _message)
    {
        GameObject _messagePanel = GameObject.FindGameObjectWithTag("GUI_Canvas").transform.Find("MessagePanel").gameObject;
        _messagePanel.SetActive(true);
        _messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = _message;        
    }

    public void CancelMessage()
    {
        GameObject _messagePanel = GameObject.FindGameObjectWithTag("GUI_Canvas").transform.Find("MessagePanel").gameObject;
        _messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "";
        _messagePanel.SetActive(false);
    }






    public void MoveForward()
    {
        //Debug.Log("Move Forward");
        if (!moving)
        {
            bool _blocked = false;
            if (facing == direction.north && ThisTile().GetComponent<TileClass>().north == TileClass.TransitionType.wall) _blocked = true;
            if (facing == direction.east && ThisTile().GetComponent<TileClass>().east == TileClass.TransitionType.wall) _blocked = true;
            if (facing == direction.south && ThisTile().GetComponent<TileClass>().south == TileClass.TransitionType.wall) _blocked = true;
            if (facing == direction.west && ThisTile().GetComponent<TileClass>().west == TileClass.TransitionType.wall) _blocked = true;
            if (!_blocked)
            {
                moving = true;
                _Old_Tile = ThisTile();
                this.transform.position += transform.forward * 3.5f;
                StartCoroutine(DelayMoveForward(_move_delay));
            }
            if (_blocked)
            {
                if(facing == direction.north && ThisTile().GetComponent<TileClass>().northPOI == "") ShortMessage("Unable to move through this wall.");
                if(facing == direction.east && ThisTile().GetComponent<TileClass>().eastPOI == "") ShortMessage("Unable to move through this wall.");
                if(facing == direction.south && ThisTile().GetComponent<TileClass>().southPOI == "") ShortMessage("Unable to move through this wall.");
                if(facing == direction.west && ThisTile().GetComponent<TileClass>().westPOI == "") ShortMessage("Unable to move through this wall.");
            }
        }
    }

    IEnumerator DelayMoveForward(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        string _POI = ""; GameObject _moveTarget = null;
        if (facing == direction.north) _POI = _Old_Tile.GetComponent<TileClass>().northPOI;
        if (facing == direction.east) _POI = _Old_Tile.GetComponent<TileClass>().eastPOI;
        if (facing == direction.south) _POI = _Old_Tile.GetComponent<TileClass>().southPOI;
        if (facing == direction.west) _POI = _Old_Tile.GetComponent<TileClass>().westPOI;

        if (_POI != "" && _POI.Substring(0, 8) == "Teleport") //If the POI starts with "Teleport"...
        {
            GameObject[] _teleporters = GameObject.FindGameObjectsWithTag("Teleport"); //...scan for matching teleporters...
            if (_teleporters.Length > 0) for (int _i = 0; _i < _teleporters.Length; _i++) if (_teleporters[_i].name == _POI) _moveTarget = _teleporters[_i];
            this.transform.position = _moveTarget.transform.position; this.transform.rotation = _moveTarget.transform.rotation; //teleport to move target 
        }

        //Move party to next tile
        if (_POI == "")
        {
            if (facing == direction.north) _moveTarget = _Old_Tile.GetComponent<TileClass>().north_Link;
            if (facing == direction.east) _moveTarget = _Old_Tile.GetComponent<TileClass>().east_Link;
            if (facing == direction.south) _moveTarget = _Old_Tile.GetComponent<TileClass>().south_Link;
            if (facing == direction.west) _moveTarget = _Old_Tile.GetComponent<TileClass>().west_Link;
            this.transform.position = _moveTarget.transform.position; //move to move target
        }

        moving = false;
    }

    public void TurnRight()
    {
        CancelMessage();
        //Debug.Log("Turn Right");        
        if (facing == direction.north && !turning) { facing = direction.east; this.transform.rotation = Quaternion.Euler(0,45,0); turning = true; }
        if (facing == direction.east && !turning) { facing = direction.south; this.transform.rotation = Quaternion.Euler(0,135,0); turning = true; }
        if (facing == direction.south && !turning) { facing = direction.west; this.transform.rotation = Quaternion.Euler(0,-135,0); turning = true; }
        if (facing == direction.west && !turning) { facing = direction.north; this.transform.rotation = Quaternion.Euler(0,-45,0); turning = true; }
        StartCoroutine(DelayTurnRight(_move_delay));
    }

    IEnumerator DelayTurnRight(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        DetermineFacing();
        turning = false;
    }

    public void TurnLeft()
    {
        CancelMessage();
        //Debug.Log("Turn Left");
        if (facing == direction.north && !turning) { facing = direction.west; this.transform.rotation = Quaternion.Euler(0, -45, 0); turning = true; }
        if (facing == direction.west && !turning) { facing = direction.south; this.transform.rotation = Quaternion.Euler(0, -135, 0); turning = true; }
        if (facing == direction.south && !turning) { facing = direction.east; this.transform.rotation = Quaternion.Euler(0, 135, 0); turning = true; }
        if (facing == direction.east && !turning) { facing = direction.north; this.transform.rotation = Quaternion.Euler(0, 45, 0); turning = true; }
        StartCoroutine(DelayTurnLeft(_move_delay));
    }

    IEnumerator DelayTurnLeft(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        DetermineFacing();
        turning = false;
    }

    public void CampMenu()
    {
        CancelMessage();
        Debug.Log("Open Camp Menu");
    }

    public void InspectButton()
    {
        Debug.Log("Open Inspect Dialogue");
    }
}
