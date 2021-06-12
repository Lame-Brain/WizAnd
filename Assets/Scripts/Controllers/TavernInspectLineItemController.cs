using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernInspectLineItemController : MonoBehaviour
{
    public GameObject logicPanelHolder;

    public void callback(int i)
    {
        logicPanelHolder.GetComponent<AddMemberToPartyController>().Inspect_THIS_Character(i);
    }
}
