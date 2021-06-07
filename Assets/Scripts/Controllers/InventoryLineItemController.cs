using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLineItemController : MonoBehaviour
{
    public void callback()
    {
        if (this.GetComponentInParent<BoltacShopController>() != null) this.GetComponentInParent<BoltacShopController>().InventoryItemClickedOn(this.transform.GetSiblingIndex());
    }
}
