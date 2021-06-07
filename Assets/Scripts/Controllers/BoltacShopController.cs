using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltacShopController : MonoBehaviour
{
    public GameObject WhoShopsPanel, ShopServicesPanel, BuyMenuPanel, BuyScrollPanel, BuyConfirmPanel;
    public GameObject SellMenuPanel, SellScrollPanel, SellConfirmPanel, ID_MenuPanel, ID_ScrollPanel, UncurseMenuPanel, UncurseScrollPanel;
    public GameObject ShopLineItem_PF;
    public TMPro.TextMeshProUGUI[] shopper;
    public TMPro.TextMeshProUGUI goldReport;

    private int _selected_Character;
    private ItemInstance _selected_item;
    private GameObject _go;
    private string _mode;

    public void PickaShopper()
    {
        ShopServicesPanel.SetActive(false);
        BuyMenuPanel.SetActive(false);
        BuyConfirmPanel.SetActive(false);
        SellMenuPanel.SetActive(false);
        ID_MenuPanel.SetActive(false);
        UncurseMenuPanel.SetActive(false);
        WhoShopsPanel.SetActive(true);
        for(int _i = 0; _i < 6; _i++) if (GameManager.PARTY.Count > _i) shopper[_i].text = GameManager.PARTY[_i].name + " LVL" + GameManager.PARTY[_i].level + " " + GameManager.PARTY[_i].alignment.ToString() + " " + GameManager.PARTY[_i].job.ToString();       
        if(GameManager.PARTY.Count == 0) gameObject.SetActive(false);
    }
    private void Update()
    {
        if (GameManager.PARTY.Count > 0) goldReport.text = GameManager.PARTY[_selected_Character].name + " has " + GameManager.PARTY[_selected_Character].gold + "gp";
    }

    public void CharacterLineClicked(int n)
    {
        _selected_Character = n;
        WhoShopsPanel.SetActive(false);
        ShopServicesPanel.SetActive(true);
    }

    private bool CheckRestrictFlags(string f)
    {
        bool _result = false;
        if(GameManager.PARTY[_selected_Character].job == PlayerCharacter.Class.Fighter && f.Contains("F")) _result = true;
        if(GameManager.PARTY[_selected_Character].job == PlayerCharacter.Class.Thief && f.Contains("T")) _result = true;
        if(GameManager.PARTY[_selected_Character].job == PlayerCharacter.Class.Mage && f.Contains("M")) _result = true;
        if(GameManager.PARTY[_selected_Character].job == PlayerCharacter.Class.Priest && f.Contains("P")) _result = true;
        if(GameManager.PARTY[_selected_Character].job == PlayerCharacter.Class.Bishop && f.Contains("B")) _result = true;
        if(GameManager.PARTY[_selected_Character].job == PlayerCharacter.Class.Lord && f.Contains("L")) _result = true;
        if(GameManager.PARTY[_selected_Character].job == PlayerCharacter.Class.Samurai && f.Contains("S")) _result = true;
        if(GameManager.PARTY[_selected_Character].job == PlayerCharacter.Class.Ninja && f.Contains("N")) _result = true;

        return _result;
    }

    public void OpenBuyMenuPanel()
    {
        //Clear existing children
        if (BuyScrollPanel.transform.childCount > 0) foreach (Transform child in BuyScrollPanel.transform) Destroy(child.gameObject);

        //Set mode
        _mode = "BUY";

        //populate Buy List
        for(int _i = 0; _i < GameManager.LISTS.itemList.Count; _i++)
        {
            if(GameManager.LISTS.itemList[_i].shopStock != 0)
            {
                _go = Instantiate(ShopLineItem_PF, BuyScrollPanel.transform);
                _go.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.LISTS.itemList[_i].itemName;
                _go.transform.Find("ItemPrice").GetComponent<TMPro.TextMeshProUGUI>().text = "";
                if (!CheckRestrictFlags(GameManager.LISTS.itemList[_i].allowEquip)) _go.transform.Find("ItemPrice").GetComponent<TMPro.TextMeshProUGUI>().text = "#";
                _go.transform.Find("ItemPrice").GetComponent<TMPro.TextMeshProUGUI>().text += (GameManager.LISTS.itemList[_i].price*2).ToString();
            }
        }        
    }

    public void LineItemClickedOn(string _selected)
    {        
        for (int _i = 0; _i < GameManager.LISTS.itemList.Count; _i++) if (_selected == GameManager.LISTS.itemList[_i].itemName) _selected_item = new ItemInstance(GameManager.LISTS.itemList[_i]); 
        Debug.Log("Clicked on " + _selected + " and it costs " + (GameManager.LISTS.itemList[_selected_item.refID].price*2));

        if(_mode == "BUY")
        {
            BuyConfirmPanel.SetActive(true);
            BuyConfirmPanel.transform.Find("Price Confirm").GetComponent<TMPro.TextMeshProUGUI>().text = "Buy " + GameManager.LISTS.itemList[_selected_item.refID].itemName + " for " + (GameManager.LISTS.itemList[_selected_item.refID].price * 2) + "gp?";
            BuyConfirmPanel.transform.Find("Warning Text").gameObject.SetActive(false);
            if (!CheckRestrictFlags(GameManager.LISTS.itemList[_selected_item.refID].allowEquip)) BuyConfirmPanel.transform.Find("Warning Text").gameObject.SetActive(true);
        }
    }

    public void YES_BUY()
    {
        int _freeslot = -1;
        for (int _i = 9; _i > -1; _i--) if (GameManager.PARTY[_selected_Character].bag[_i] == null) _freeslot = _i;
        if(_freeslot > 0)
        { 
            if (GameManager.PARTY[_selected_Character].gold >= (GameManager.LISTS.itemList[_selected_item.refID].price * 2))
            {
                GameManager.PARTY[_selected_Character].gold -= (GameManager.LISTS.itemList[_selected_item.refID].price * 2);
                GameManager.PARTY[_selected_Character].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_selected_item.refID]);
                if (GameManager.LISTS.itemList[_selected_item.refID].shopStock > 0) GameManager.LISTS.itemList[_selected_item.refID].shopStock--;
                //BUY SUCCESS!
            }
            else
            {
                //BUY FAIL! Not enough Money!
            }
        } else
        {
            //BUY FAIL! Not enough space!
        }
    }
}
