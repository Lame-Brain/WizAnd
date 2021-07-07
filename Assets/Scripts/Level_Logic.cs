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
    private float _message_delay = 1.5f;
    private GameObject _Old_Tile;
    private List<GameObject> _SpecialMessages = new List<GameObject>();

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

        //Special Message Panels
        GameObject[] _SMs = GameObject.FindGameObjectsWithTag("Special_Message_Panel");
        _SpecialMessages.Clear();
        for (int _i = 0; _i < _SMs.Length; _i++)
        {
            _SpecialMessages.Add(_SMs[_i]);
            _SMs[_i].SetActive(false);
        }

        //Secret Doors
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

        //if (_POI != "") Debug.Log("_POI is " + _POI + " first 7 is " + _POI.Substring(0, 7));

        if (_POI != "" && _POI.Substring(0, 7) == "Message") SpecialMessage(_POI);

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

    public void SpecialMessage(string _messageName)
    {
        
        if(_SpecialMessages.Count > 0) for(int _i = 0; _i < _SpecialMessages.Count; _i++) if(_SpecialMessages[_i].name == _messageName)
                {
                    _SpecialMessages[_i].SetActive(true);
                }
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
                if(facing == direction.north && ThisTile().GetComponent<TileClass>().northPOI == "") ShortMessage("Unable to move through walls.");
                if(facing == direction.east && ThisTile().GetComponent<TileClass>().eastPOI == "") ShortMessage("Unable to move through walls.");
                if(facing == direction.south && ThisTile().GetComponent<TileClass>().southPOI == "") ShortMessage("Unable to move through walls.");
                if(facing == direction.west && ThisTile().GetComponent<TileClass>().westPOI == "") ShortMessage("Unable to move through walls.");
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
            this.transform.position = _moveTarget.transform.position; //teleport to move target 
            //set facing
            if (_moveTarget.transform.rotation.eulerAngles.y >= 315 && _moveTarget.transform.rotation.eulerAngles.y <= 45) facing = direction.north;
            if (_moveTarget.transform.rotation.eulerAngles.y > 45 && _moveTarget.transform.rotation.eulerAngles.y < 135) facing = direction.east;
            if (_moveTarget.transform.rotation.eulerAngles.y >= 135 && _moveTarget.transform.rotation.eulerAngles.y <= 225) facing = direction.south;
            if (_moveTarget.transform.rotation.eulerAngles.y > 225 && _moveTarget.transform.rotation.eulerAngles.y < 315) facing = direction.west;
            DetermineFacing();
        }

        //Move party to next tile
        if (_POI == "")
        {
            if (facing == direction.north) _moveTarget = _Old_Tile.GetComponent<TileClass>().north_Link;
            if (facing == direction.east) _moveTarget = _Old_Tile.GetComponent<TileClass>().east_Link;
            if (facing == direction.south) _moveTarget = _Old_Tile.GetComponent<TileClass>().south_Link;
            if (facing == direction.west) _moveTarget = _Old_Tile.GetComponent<TileClass>().west_Link;
            this.transform.position = _moveTarget.transform.position; //move to move target
            DetermineFacing();
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



    public void MAPIRO_MAHAMA_DIROMAT()
    {
        //Returns party to town
    }

    public void Awaken_Murphys_Ghost()
    {
        //Start Murphy's Ghost Fight
    }

    public void GetBlueRibbon()
    {
        //Collect Blue Ribbon
    }

    public void CollectFrogFigurine()
    {
        bool _hasThisKey = false; int _key = 136; string _keyName = "Frog Figurine";
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == _key) _hasThisKey = true;

        if (!_hasThisKey) //if the party does not have the bronze key...
        {
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count - 1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length - 1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_key]); //... and give it to them.
                ShortMessage("You have found a " + _keyName);
            }
            else
            {
                ShortMessage("You have found a " + _keyName + ", but do not have a way to carry it."); //...unless they're out of inventory space
            }
        }
        else
        {
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }

    public void CollectBearFigurine()
    {
        bool _hasThisKey = false; int _key = 137; string _keyName = "Bear Figurine";
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == _key) _hasThisKey = true;

        if (!_hasThisKey) //if the party does not have the bronze key...
        {
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count - 1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length - 1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_key]); //... and give it to them.
                ShortMessage("You have found a " + _keyName);
            }
            else
            {
                ShortMessage("You have found a " + _keyName + ", but do not have a way to carry it."); //...unless they're out of inventory space
            }
        }
        else
        {
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }

    public void CollectBronzeKey()
    {
        bool _hasBronzeKey = false;
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == 138) _hasBronzeKey = true; //item ID 138 is BronzeKey
        
        if (!_hasBronzeKey) //if the party does not have the bronze key...
        {
            Debug.Log("Party does not have key");
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count-1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length-1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[138]); //... and give it to them.
                ShortMessage("You have found a Bronze Key");
                Debug.Log("Party gets the key");
            }
            else
            {
                ShortMessage("You have found a Bronze Key, but do not have a way to carry it."); //...unless they're out of inventory space
                Debug.Log("Inventory is full");
            }
        }
        else
        {
            Debug.Log("Party does have key");
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }

    public void CollectSilverKey()
    {
        bool _hasThisKey = false; int _key = 139; string _keyName = "Silver Key";
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == _key) _hasThisKey = true; 

        if (!_hasThisKey) //if the party does not have the bronze key...
        {
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count - 1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length - 1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_key]); //... and give it to them.
                ShortMessage("You have found a " + _keyName);
            }
            else
            {
                ShortMessage("You have found a " + _keyName + ", but do not have a way to carry it."); //...unless they're out of inventory space
            }
        }
        else
        {
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }

    public void CollectGoldKey()
    {
        bool _hasThisKey = false; int _key = 140; string _keyName = "Gold Key";
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == _key) _hasThisKey = true;

        if (!_hasThisKey) //if the party does not have the bronze key...
        {
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count - 1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length - 1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_key]); //... and give it to them.
                ShortMessage("You have found a " + _keyName);
            }
            else
            {
                ShortMessage("You have found a " + _keyName + ", but do not have a way to carry it."); //...unless they're out of inventory space
            }
        }
        else
        {
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }

    public void Collect3Key()
    {
        bool _hasThisKey = false; int _key = 142; string _keyName = "Triangle Key";
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == _key) _hasThisKey = true;

        if (!_hasThisKey) //if the party does not have the bronze key...
        {
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count - 1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length - 1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_key]); //... and give it to them.
                ShortMessage("You have found a " + _keyName);
            }
            else
            {
                ShortMessage("You have found a " + _keyName + ", but do not have a way to carry it."); //...unless they're out of inventory space
            }
        }
        else
        {
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }

    public void Collect4Key()
    {
        bool _hasThisKey = false; int _key = 143; string _keyName = "Square Key";
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == _key) _hasThisKey = true;

        if (!_hasThisKey) //if the party does not have the bronze key...
        {
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count - 1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length - 1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_key]); //... and give it to them.
                ShortMessage("You have found a " + _keyName);
            }
            else
            {
                ShortMessage("You have found a " + _keyName + ", but do not have a way to carry it."); //...unless they're out of inventory space
            }
        }
        else
        {
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }

    public void Collect5Key()
    {
        bool _hasThisKey = false; int _key = 144; string _keyName = "Pentagonal Key";
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == _key) _hasThisKey = true;

        if (!_hasThisKey) //if the party does not have the bronze key...
        {
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count - 1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length - 1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_key]); //... and give it to them.
                ShortMessage("You have found a " + _keyName);
            }
            else
            {
                ShortMessage("You have found a " + _keyName + ", but do not have a way to carry it."); //...unless they're out of inventory space
            }
        }
        else
        {
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }

    public void Collect6Key()
    {
        bool _hasThisKey = false; int _key = 145; string _keyName = "Hexagonal Key";
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == _key) _hasThisKey = true;

        if (!_hasThisKey) //if the party does not have the bronze key...
        {
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count - 1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length - 1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_key]); //... and give it to them.
                ShortMessage("You have found a " + _keyName);
            }
            else
            {
                ShortMessage("You have found a " + _keyName + ", but do not have a way to carry it."); //...unless they're out of inventory space
            }
        }
        else
        {
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }

    public void Collect7Key()
    {
        bool _hasThisKey = false; int _key = 146; string _keyName = "Heptogonal Key";
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == _key) _hasThisKey = true;

        if (!_hasThisKey) //if the party does not have the bronze key...
        {
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count - 1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length - 1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_key]); //... and give it to them.
                ShortMessage("You have found a " + _keyName);
            }
            else
            {
                ShortMessage("You have found a " + _keyName + ", but do not have a way to carry it."); //...unless they're out of inventory space
            }
        }
        else
        {
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }

    public void Collect8Key()
    {
        bool _hasThisKey = false; int _key = 147; string _keyName = "Octagonal Key";
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == _key) _hasThisKey = true;

        if (!_hasThisKey) //if the party does not have the bronze key...
        {
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count - 1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length - 1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_key]); //... and give it to them.
                ShortMessage("You have found a " + _keyName);
            }
            else
            {
                ShortMessage("You have found a " + _keyName + ", but do not have a way to carry it."); //...unless they're out of inventory space
            }
        }
        else
        {
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }

    public void Collect9Key()
    {
        bool _hasThisKey = false; int _key = 148; string _keyName = "Nonagonal Key";
        //Check for Bronze Key in inventory already
        for (int _p = 0; _p < GameManager.PARTY.Count; _p++) //scan each member of the party            
            for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length; _i++) //scan their bag                
                if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] != null && GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i].refID == _key) _hasThisKey = true;

        if (!_hasThisKey) //if the party does not have the bronze key...
        {
            //...Find an empty slot...
            int _freeslot = -1, _freeToon = -1;
            for (int _p = GameManager.PARTY.Count - 1; _p >= 0; _p--)
                for (int _i = GameManager.ROSTER[GameManager.PARTY[_p]].bag.Length - 1; _i >= 0; _i--)
                    if (GameManager.ROSTER[GameManager.PARTY[_p]].bag[_i] == null) { _freeslot = _i; _freeToon = _p; }
            if (_freeslot >= 0 && _freeToon >= 0)
            {
                GameManager.ROSTER[_freeToon].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_key]); //... and give it to them.
                ShortMessage("You have found a " + _keyName);
            }
            else
            {
                ShortMessage("You have found a " + _keyName + ", but do not have a way to carry it."); //...unless they're out of inventory space
            }
        }
        else
        {
            ShortMessage("You do not find anything."); //The party has the key already
        }
    }
}
