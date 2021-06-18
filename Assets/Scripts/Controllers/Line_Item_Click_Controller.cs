using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_Item_Click_Controller : MonoBehaviour
{
    public int myValue;

    public void CallBack()
    {
        if (this.GetComponentInParent<CharacterScreenController>() != null) GetComponentInParent<CharacterScreenController>().SpellClickedOn(myValue);
    }
}
