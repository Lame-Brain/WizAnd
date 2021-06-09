using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ID_LineItemController : MonoBehaviour
{
    public void callback()
    {
        if (this.GetComponentInParent<BoltacShopController>() != null) this.GetComponentInParent<BoltacShopController>().ID_ItemClickedOn(transform.GetSiblingIndex());
    }
}
