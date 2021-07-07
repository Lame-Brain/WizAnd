using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectWindow_CharacterLine_Controller : MonoBehaviour
{
    public int thisLine;

    public void Callback()
    {
        if (this.GetComponentInParent<CharacterRosterInspectScreenController>() != null) this.GetComponentInParent<CharacterRosterInspectScreenController>().CharacterLineClicked(transform.GetSiblingIndex());
        if (this.GetComponentInParent<AddMemberToPartyController>() != null) this.GetComponentInParent<AddMemberToPartyController>().CharacterLineClicked(thisLine, transform.parent.name);
        if (this.GetComponentInParent<BoltacShopController>() != null) this.GetComponentInParent<BoltacShopController>().CharacterLineClicked(transform.GetSiblingIndex());
    }
}
