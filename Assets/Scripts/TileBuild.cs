using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileClass))]
public class TileBuild : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Build this Tile"))
        {            
            MonoBehaviour _tile = (MonoBehaviour)target;

            //clear existing children
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