using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltacShopController : MonoBehaviour
{
    public GameObject WhoShopsPanel, ShopServicesPanel, BuyMenuPanel, SellMenuPanel, IdentifyMenuPanel, UncurseMenuPanel;
    public TMPro.TextMeshProUGUI[] shopper;

    private int _selected_Character;

    public void PickaShopper()
    {
        for(int _i = 0; _i < 6; _i++) if (GameManager.PARTY.Count > _i) shopper[_i].text = GameManager.PARTY[_i].name + " LVL" + GameManager.PARTY[_i].level + " " + GameManager.PARTY[_i].alignment.ToString() + " " + GameManager.PARTY[_i].job.ToString();
        
    }

    public void CharacterLineClicked(int n)
    {
        _selected_Character = n;
        WhoShopsPanel.SetActive(false);
        ShopServicesPanel.SetActive(true);
    }
}
