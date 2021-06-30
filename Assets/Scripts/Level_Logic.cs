using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Logic : MonoBehaviour
{
    public GameObject ThisLevel;
    
    public enum direction { north, east, south, west }
    public direction facing;    

    public bool moving, turning;
    private float _move_delay = 0.25f;
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
    }

    public GameObject ThisTile()
    {
        GameObject _result = null;
        int _x = (int)this.transform.position.x / 7; int _y = (int)this.transform.position.z / 7; int _child = (_y * 20) + _x;
        _result = GameObject.FindGameObjectWithTag("Level").transform.GetChild(_child).gameObject;
        return _result;
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
        }
    }

    IEnumerator DelayMoveForward(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        if (facing == direction.north) this.transform.position = _Old_Tile.GetComponent<TileClass>().north_Link.transform.position;
        if (facing == direction.east) this.transform.position = _Old_Tile.GetComponent<TileClass>().east_Link.transform.position;
        if (facing == direction.south) this.transform.position = _Old_Tile.GetComponent<TileClass>().south_Link.transform.position;
        if (facing == direction.west) this.transform.position = _Old_Tile.GetComponent<TileClass>().west_Link.transform.position;
        moving = false;
    }

    public void TurnRight()
    {
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
        Debug.Log("Open Camp Menu");
    }

    public void InspectButton()
    {
        Debug.Log("Open Inspect Dialogue");
        Debug.Log("Standing in " + ThisTile().name);
    }
}
