using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLineItemController : MonoBehaviour
{
    public void callback()
    {
        if (this.GetComponentInParent<BoltacShopController>() != null) this.GetComponentInParent<BoltacShopController>().LineItemClickedOn(this.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>().text.ToString());
        if (this.GetComponentInParent<TempleShopController>() != null) this.GetComponentInParent<TempleShopController>().ChooseServiceLine(this.transform.GetSiblingIndex());
    }
}
