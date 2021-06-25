using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileGridClass))]
public class TilePlacement : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Build the Tiles"))
        {
            int _gridSize = GameObject.FindGameObjectWithTag("Level").GetComponent<TileGridClass>().gridSize;
            float _tileWidth = GameObject.FindGameObjectWithTag("Level").GetComponent<TileGridClass>().tileWidth;
            float _tileHeight = GameObject.FindGameObjectWithTag("Level").GetComponent<TileGridClass>().tileHeight;
            GameObject tile_PF = GameObject.FindGameObjectWithTag("Level").GetComponent<TileGridClass>().tile_PF;
            GameObject wall_PF = GameObject.FindGameObjectWithTag("Level").GetComponent<TileGridClass>().wall_PF;
            GameObject door_PF = GameObject.FindGameObjectWithTag("Level").GetComponent<TileGridClass>().door_PF;
            GameObject dark_PF = GameObject.FindGameObjectWithTag("Level").GetComponent<TileGridClass>().dark_PF;
            GameObject[,] tileGrid = new GameObject[_gridSize, _gridSize];
            List<TileClass> oldTile = new List<TileClass>();

            //remove tiles            
            int _num = GameObject.FindGameObjectWithTag("Level").transform.childCount;
            oldTile.Clear();
            if (_num > 0)
            {
                
                for (int _i = 0; _i < _num; _i++)
                {
                    oldTile.Add(GameObject.FindGameObjectWithTag("Level").transform.GetChild(0).GetComponent<TileClass>());
                    DestroyImmediate(GameObject.FindGameObjectWithTag("Level").transform.GetChild(0).gameObject);
                }
            }

            //Instantiate tiles
            for (int _y = 0; _y < _gridSize; _y++)
            {
                for (int _x = 0; _x < _gridSize; _x++)
                {
                    tileGrid[_x, _y] = Instantiate(tile_PF, new Vector3(_x * _tileWidth, 0, _y * _tileWidth), Quaternion.identity, GameObject.FindGameObjectWithTag("Level").transform);
                    if (oldTile.Count > 0) tileGrid[_x, _y].GetComponent<TileClass>().setValue(oldTile[_y * _gridSize + _x]);
                    tileGrid[_x, _y].name = "Tile (" + _x + ", " + _y + ")";
                }
            }

            //set the default directional links
            for (int _y = 0; _y < _gridSize; _y++)
            {
                for (int _x = 0; _x < _gridSize; _x++)
                {
                    if (_y == (_gridSize - 1)) tileGrid[_x, _y].GetComponent<TileClass>().north_Link = tileGrid[_x, 0];
                    if (_y < (_gridSize - 1)) tileGrid[_x, _y].GetComponent<TileClass>().north_Link = tileGrid[_x, _y + 1];

                    if (_x == (_gridSize - 1)) tileGrid[_x, _y].GetComponent<TileClass>().east_Link = tileGrid[0, _y];
                    if (_x < (_gridSize - 1)) tileGrid[_x, _y].GetComponent<TileClass>().east_Link = tileGrid[_x + 1, _y];

                    if (_y == 0) tileGrid[_x, _y].GetComponent<TileClass>().south_Link = tileGrid[_x, (_gridSize - 1)];
                    if (_y > 0) tileGrid[_x, _y].GetComponent<TileClass>().south_Link = tileGrid[_x, _y - 1];

                    if (_x == 0) tileGrid[_x, _y].GetComponent<TileClass>().west_Link = tileGrid[(_gridSize - 1), _y];
                    if (_x > 0) tileGrid[_x, _y].GetComponent<TileClass>().west_Link = tileGrid[_x - 1, _y];
                }
            }

            //Build the level structures
            for (int _y = 0; _y < _gridSize; _y++)
            {
                for (int _x = 0; _x < _gridSize; _x++)
                {
                    GameObject _tile = tileGrid[_x, _y];

                    //remove tiles            
                    //foreach (GameObject _oldTile in GameObject.FindGameObjectsWithTag("Tile")) DestroyImmediate(_oldTile);
                    //clear children
                    if (_tile.transform.childCount > 0) for (int _i = _tile.transform.childCount; _i > 0; _i--) DestroyImmediate(_tile.transform.GetChild(_i - 1).gameObject);

                    //create new children
                    if (_tile.GetComponent<TileClass>().north == TileClass.TransitionType.wall) Instantiate(_tile.GetComponentInParent<TileGridClass>().wall_PF, new Vector3(_tile.transform.position.x, _tile.transform.position.y, _tile.transform.position.z + 3.45f), Quaternion.Euler(0, 90, 0), _tile.transform);
                    if (_tile.GetComponent<TileClass>().north == TileClass.TransitionType.secret_door) Instantiate(_tile.GetComponentInParent<TileGridClass>().secret_door_PF, new Vector3(_tile.transform.position.x, _tile.transform.position.y, _tile.transform.position.z + 3.45f), Quaternion.Euler(0, 90, 0), _tile.transform);
                    if (_tile.GetComponent<TileClass>().north == TileClass.TransitionType.door) Instantiate(_tile.GetComponentInParent<TileGridClass>().door_PF, new Vector3(_tile.transform.position.x, _tile.transform.position.y, _tile.transform.position.z + 3.45f), Quaternion.Euler(0, 90, 0), _tile.transform);

                    if (_tile.GetComponent<TileClass>().east == TileClass.TransitionType.wall) Instantiate(_tile.GetComponentInParent<TileGridClass>().wall_PF, new Vector3(_tile.transform.position.x + 3.45f, _tile.transform.position.y, _tile.transform.position.z), Quaternion.Euler(0, 0, 0), _tile.transform);
                    if (_tile.GetComponent<TileClass>().east == TileClass.TransitionType.secret_door) Instantiate(_tile.GetComponentInParent<TileGridClass>().secret_door_PF, new Vector3(_tile.transform.position.x + 3.45f, _tile.transform.position.y, _tile.transform.position.z), Quaternion.Euler(0, 0, 0), _tile.transform);
                    if (_tile.GetComponent<TileClass>().east == TileClass.TransitionType.door) Instantiate(_tile.GetComponentInParent<TileGridClass>().door_PF, new Vector3(_tile.transform.position.x + 3.45f, _tile.transform.position.y, _tile.transform.position.z), Quaternion.Euler(0, 0, 0), _tile.transform);

                    if (_tile.GetComponent<TileClass>().south == TileClass.TransitionType.wall) Instantiate(_tile.GetComponentInParent<TileGridClass>().wall_PF, new Vector3(_tile.transform.position.x, _tile.transform.position.y, _tile.transform.position.z - 3.45f), Quaternion.Euler(0, -90, 0), _tile.transform);
                    if (_tile.GetComponent<TileClass>().south == TileClass.TransitionType.secret_door) Instantiate(_tile.GetComponentInParent<TileGridClass>().secret_door_PF, new Vector3(_tile.transform.position.x, _tile.transform.position.y, _tile.transform.position.z - 3.45f), Quaternion.Euler(0, -90, 0), _tile.transform);
                    if (_tile.GetComponent<TileClass>().south == TileClass.TransitionType.door) Instantiate(_tile.GetComponentInParent<TileGridClass>().door_PF, new Vector3(_tile.transform.position.x, _tile.transform.position.y, _tile.transform.position.z - 3.45f), Quaternion.Euler(0, -90, 0), _tile.transform);

                    if (_tile.GetComponent<TileClass>().west == TileClass.TransitionType.wall) Instantiate(_tile.GetComponentInParent<TileGridClass>().wall_PF, new Vector3(_tile.transform.position.x - 3.45f, _tile.transform.position.y, _tile.transform.position.z), Quaternion.Euler(0, 180, 0), _tile.transform);
                    if (_tile.GetComponent<TileClass>().west == TileClass.TransitionType.secret_door) Instantiate(_tile.GetComponentInParent<TileGridClass>().secret_door_PF, new Vector3(_tile.transform.position.x - 3.45f, _tile.transform.position.y, _tile.transform.position.z), Quaternion.Euler(0, 180, 0), _tile.transform);
                    if (_tile.GetComponent<TileClass>().west == TileClass.TransitionType.door) Instantiate(_tile.GetComponentInParent<TileGridClass>().door_PF, new Vector3(_tile.transform.position.x - 3.45f, _tile.transform.position.y, _tile.transform.position.z), Quaternion.Euler(0, 180, 0), _tile.transform);

                    if (_tile.GetComponent<TileClass>().darkness) Instantiate(_tile.GetComponentInParent<TileGridClass>().dark_PF, new Vector3(_tile.transform.position.x, _tile.transform.position.y, _tile.transform.position.z), Quaternion.identity, _tile.transform);
                }
            }
        }
    }
}
