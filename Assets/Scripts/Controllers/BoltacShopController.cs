using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltacShopController : MonoBehaviour
{
    public GameObject WhoShopsPanel, ShopServicesPanel, BuyMenuPanel, BuyScrollPanel, BuyConfirmPanel;
    public GameObject SellMenuPanel, SellScrollPanel, SellConfirmPanel, ID_MenuPanel, ID_ConfirmPanel, UncurseMenuPanel, UncurseConfirmPanel, PoolGoldPanel;
    public GameObject ShopLineItem_PF;
    public TMPro.TextMeshProUGUI[] shopper, ID_PanelSlots, Uncurse_PanelSlots;
    public TMPro.TextMeshProUGUI goldReport;

    private int _selected_Character, _selectedInventorySlot;
    private ItemInstance _selected_item;
    private GameObject _go;

    public void PickaShopper()
    {
        ShopServicesPanel.SetActive(false);
        BuyMenuPanel.SetActive(false);
        BuyConfirmPanel.SetActive(false);
        SellMenuPanel.SetActive(false);
        SellConfirmPanel.SetActive(false);
        ID_MenuPanel.SetActive(false);
        ID_ConfirmPanel.SetActive(false);
        UncurseMenuPanel.SetActive(false);
        UncurseConfirmPanel.SetActive(false);
        PoolGoldPanel.SetActive(false);
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
        ShopServicesPanel.SetActive(false);
        BuyMenuPanel.SetActive(true);
        BuyConfirmPanel.SetActive(false);
        SellMenuPanel.SetActive(false);
        SellConfirmPanel.SetActive(false);
        ID_MenuPanel.SetActive(false);
        ID_ConfirmPanel.SetActive(false);
        UncurseMenuPanel.SetActive(false);
        UncurseConfirmPanel.SetActive(false);
        PoolGoldPanel.SetActive(false);
        WhoShopsPanel.SetActive(false);

        //Clear existing children
        if (BuyScrollPanel.transform.childCount > 0) foreach (Transform child in BuyScrollPanel.transform) Destroy(child.gameObject);

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

    public void OpenSellMenuPanel()
    {
        ShopServicesPanel.SetActive(false);
        BuyMenuPanel.SetActive(false);
        BuyConfirmPanel.SetActive(false);
        SellMenuPanel.SetActive(true);
        SellConfirmPanel.SetActive(false);
        ID_MenuPanel.SetActive(false);
        ID_ConfirmPanel.SetActive(false);
        UncurseMenuPanel.SetActive(false);
        WhoShopsPanel.SetActive(false);

        //populate Sell List
        for (int _i = 0; _i < GameManager.PARTY[_selected_Character].bag.Length; _i++)
        {
            if (GameManager.PARTY[_selected_Character].bag[_i] == null)
            {
                SellScrollPanel.transform.GetChild(_i).transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>().text = "";
                SellScrollPanel.transform.GetChild(_i).transform.Find("ItemPrice").GetComponent<TMPro.TextMeshProUGUI>().text = "";
            }
            if (GameManager.PARTY[_selected_Character].bag[_i] != null)
            {
                SellScrollPanel.transform.GetChild(_i).Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>().text = "";
                if (!CheckRestrictFlags(GameManager.LISTS.itemList[_i].allowEquip)) SellScrollPanel.transform.GetChild(_i).Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>().text = "#";
                SellScrollPanel.transform.GetChild(_i).transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>().text += GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_i].refID].itemName;                
                SellScrollPanel.transform.GetChild(_i).transform.Find("ItemPrice").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_i].refID].price + "gp";
            }
            if (GameManager.PARTY[_selected_Character].bag[_i] != null && GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_i].refID].cursed)
            {                
                SellScrollPanel.transform.GetChild(_i).transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>().text = "?" + GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_i].refID].itemType.ToString();
                SellScrollPanel.transform.Find("ItemPrice").GetComponent<TMPro.TextMeshProUGUI>().text = ((int)GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_i].refID].price / 2) + "gp";
            }
            if (GameManager.PARTY[_selected_Character].bag[_i] != null && !GameManager.PARTY[_selected_Character].bag[_i].identified)
            {
                SellScrollPanel.transform.GetChild(_i).transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>().text = "?" + GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_i].refID].itemType.ToString();
                SellScrollPanel.transform.Find("ItemPrice").GetComponent<TMPro.TextMeshProUGUI>().text = "0gp";                                
            }
        }
    }

    public void OpenIDPanel()
    {
        ShopServicesPanel.SetActive(false);
        BuyMenuPanel.SetActive(false);
        BuyConfirmPanel.SetActive(false);
        SellMenuPanel.SetActive(false);
        SellConfirmPanel.SetActive(false);
        ID_MenuPanel.SetActive(true);
        ID_ConfirmPanel.SetActive(false);
        UncurseMenuPanel.SetActive(false);
        UncurseConfirmPanel.SetActive(false);
        PoolGoldPanel.SetActive(false);
        WhoShopsPanel.SetActive(false);

        //populate the identify list
        for(int _i = 0; _i < ID_PanelSlots.Length; _i++)
        {
            if (GameManager.PARTY[_selected_Character].bag[_i].identified) ID_PanelSlots[_i].text = "";
            if (!GameManager.PARTY[_selected_Character].bag[_i].identified) ID_PanelSlots[_i].text = GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_i].refID].itemType.ToString() + "     " + GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_i].refID].price;
        }

    }

    public void LineItemClickedOn(string _selected)
    {        
        for (int _i = 0; _i < GameManager.LISTS.itemList.Count; _i++) if (_selected == GameManager.LISTS.itemList[_i].itemName) _selected_item = new ItemInstance(GameManager.LISTS.itemList[_i]); 
        //Debug.Log("Clicked on " + _selected + " and it costs " + (GameManager.LISTS.itemList[_selected_item.refID].price*2));

            BuyConfirmPanel.SetActive(true);
            BuyConfirmPanel.transform.Find("Price Confirm").GetComponent<TMPro.TextMeshProUGUI>().text = "Buy " + GameManager.LISTS.itemList[_selected_item.refID].itemName + " for " + (GameManager.LISTS.itemList[_selected_item.refID].price * 2) + "gp?";
            BuyConfirmPanel.transform.Find("Warning Text").gameObject.SetActive(false);
            if (!CheckRestrictFlags(GameManager.LISTS.itemList[_selected_item.refID].allowEquip)) BuyConfirmPanel.transform.Find("Warning Text").gameObject.SetActive(true);
    }

    public void InventoryItemClickedOn(int _selected)
    {
        _selectedInventorySlot = _selected;
        Debug.Log("clicked on " + GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].itemName + " in slot " + _selected);

        SellConfirmPanel.SetActive(true);
        if (!GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].cursed) //if it is not a inactive cursed item
        {
            if (!GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].identified) SellConfirmPanel.transform.Find("Price Confirm").GetComponent<TMPro.TextMeshProUGUI>().text = "Sell ?" +
                     GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].itemType.ToString() + " for 0 gp?";
            if (GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].identified) SellConfirmPanel.transform.Find("Price Confirm").GetComponent<TMPro.TextMeshProUGUI>().text = "Sell " +
                    GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].itemName + " for " +
                    GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].price + " gp?";
        }
        if (GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].cursed) //if it has an inactive curse, it sells for less
        {
            SellConfirmPanel.transform.Find("Price Confirm").GetComponent<TMPro.TextMeshProUGUI>().text = "Sell " +
                    GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].itemName + " for " +
                    ((int)GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].price / 2) + " gp?";
        }
    }

    public void ID_ItemClickedOn(int _selected)
    {
        if(GameManager.PARTY[_selected_Character].bag[_selected] != null && !GameManager.PARTY[_selected_Character].bag[_selected].identified)
        {
            _selectedInventorySlot = _selected;
            ID_ConfirmPanel.SetActive(true);
            ID_ConfirmPanel.transform.Find("Title").GetComponent<TMPro.TextMeshProUGUI>().text = "Identify " + 
                GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].itemType.ToString() +
                " for " + GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].price.ToString() + "gp?";
        }
    }

    public void UNCURSE_ItemClickedOn(int _selected)
    {
        if (GameManager.PARTY[_selected_Character].bag[_selected] != null && GameManager.PARTY[_selected_Character].bag[_selected].curseActive)
        {
            _selectedInventorySlot = _selected;
            UncurseConfirmPanel.SetActive(true);
            ID_ConfirmPanel.transform.Find("Title").GetComponent<TMPro.TextMeshProUGUI>().text = "Uncurse this item for " 
                + GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].price.ToString() + "gp?";
        }
    }

    public void YES_BUY()
    {
        int _freeslot = -1;
        for (int _i = 9; _i > -1; _i--) if (GameManager.PARTY[_selected_Character].bag[_i] == null) _freeslot = _i;
        if(_freeslot >= 0)
        {
            if (GameManager.PARTY[_selected_Character].gold >= (GameManager.LISTS.itemList[_selected_item.refID].price * 2))
            {
                GameManager.PARTY[_selected_Character].gold -= (GameManager.LISTS.itemList[_selected_item.refID].price * 2);
                GameManager.PARTY[_selected_Character].bag[_freeslot] = new ItemInstance(GameManager.LISTS.itemList[_selected_item.refID]);
                GameManager.PARTY[_selected_Character].bag[_freeslot].identified = true;
                //GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_freeslot].refID].cursed = true;
                if (GameManager.LISTS.itemList[_selected_item.refID].shopStock > 0) GameManager.LISTS.itemList[_selected_item.refID].shopStock--;
                //BUY SUCCESS!
                OpenBuyMenuPanel();
            }
            else
            {
                //BUY FAIL! Not enough Money!
                BuyMenuPanel.transform.Find("BuyErrorPanel").gameObject.SetActive(true);
                BuyMenuPanel.transform.Find("BuyErrorPanel").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Sorry! You don't have enough GP.";
            }
        } else
        {
            //BUY FAIL! Not enough space!
            BuyMenuPanel.transform.Find("BuyErrorPanel").gameObject.SetActive(true);
            BuyMenuPanel.transform.Find("BuyErrorPanel").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Sorry! Inventory Full!";
        }
    }

    public void YES_SELL()
    {
        //!GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].cursed
        if (!GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].curseActive)
        {
            //If the item is not cursed, give the player gold.
            if (!GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].cursed) GameManager.PARTY[_selected_Character].gold += GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].price;

            //If the item is (inactive) cursed, give the player less gold.
            if (GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].cursed) GameManager.PARTY[_selected_Character].gold += (int) GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].price / 2;

            //add the item to shop stock
            if (GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].shopStock > -1) GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].shopStock++;

            //remove item from player inventory
            GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot] = null;

            OpenSellMenuPanel();
        }
        else
        {
            //SELL FAIL! Cursed!
            SellMenuPanel.transform.Find("SellErrorPanel").gameObject.SetActive(true);
        }
    }

    public void YES_ID()
    {
        if (GameManager.PARTY[_selected_Character].gold >= (GameManager.LISTS.itemList[_selected_item.refID].price))
        {
            GameManager.PARTY[_selected_Character].gold -= (GameManager.LISTS.itemList[_selected_item.refID].price);
            GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].identified = true;
            //ID SUCCESS!
            OpenIDPanel();
        }
        else
        {
            //ID FAIL! Not enough money
            ID_ConfirmPanel.transform.Find("ID_ErrorPanel").gameObject.SetActive(true);
        }
    }

    public void YES_UNCURSE()
    {
        GameManager.PARTY[_selected_Character].gold -= GameManager.LISTS.itemList[GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot].refID].price;
        GameManager.PARTY[_selected_Character].bag[_selectedInventorySlot] = null;
    }

    public void PoolGold()
    {
        int _totalGOld = 0;
        for (int _i = 0; _i < GameManager.PARTY.Count; _i++)
        {
            _totalGOld += GameManager.PARTY[_i].gold;
            GameManager.PARTY[_i].gold = 0;
        }
        GameManager.PARTY[_selected_Character].gold = _totalGOld;
    }
    
    public void UpdateSaveGame()
    {
        SaveLoadModule.SaveGame();
    }
}
