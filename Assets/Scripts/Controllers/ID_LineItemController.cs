using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ID_LineItemController : MonoBehaviour
{
    public void callback()
    {
        if (this.GetComponentInParent<BoltacShopController>() != null && gameObject.name.Substring(0,2) == "ID") this.GetComponentInParent<BoltacShopController>().ID_ItemClickedOn(transform.GetSiblingIndex());
        if (this.GetComponentInParent<BoltacShopController>() != null && gameObject.name.Substring(0,7) == "Uncurse") this.GetComponentInParent<BoltacShopController>().UNCURSE_ItemClickedOn(transform.GetSiblingIndex());
    }
}
