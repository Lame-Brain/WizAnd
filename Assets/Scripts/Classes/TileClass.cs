using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClass : MonoBehaviour
{
    public enum TransitionType { wall, open, door, secret_door, teleport }
    public TransitionType north;
    public TransitionType east;
    public TransitionType south;
    public TransitionType west;
    public GameObject north_Link;
    public GameObject east_Link;
    public GameObject south_Link;
    public GameObject west_Link;
    public bool darkness, antiMagic, spinner, pit;
    public string northPOI, eastPOI, southPOI, westPOI, centerPOI;
    public bool[] treasure;
    
    public void setValue(TileClass _t)
    {
        this.north = _t.north;
        this.north_Link = _t.north_Link;
        this.east = _t.east;
        this.east_Link = _t.east_Link;
        this.south = _t.south;
        this.south_Link = _t.south_Link;
        this.west = _t.west;
        this.west_Link = _t.west_Link;
        this.darkness = _t.darkness; this.antiMagic = _t.antiMagic; this.spinner = _t.spinner; this.pit = _t.pit;
        this.northPOI = _t.northPOI; this.eastPOI = _t.eastPOI; this.southPOI = _t.southPOI; this.westPOI = _t.westPOI; this.centerPOI = _t.centerPOI;
        this.treasure = _t.treasure;
    }
}
