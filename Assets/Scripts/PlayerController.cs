using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow)) this.transform.position += transform.forward * 7;
        if (Input.GetKeyUp(KeyCode.RightArrow)) this.transform.Rotate(0, 90, 0);
        if (Input.GetKeyUp(KeyCode.LeftArrow)) this.transform.Rotate(0, -90, 0);
    }
}
