using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterScreenController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI VitalStatsLine1, VitalStatsLine2, StatBlock, OtherValues, MageSlots, PriestSlots;
    public TMPro.TextMeshProUGUI[] BagSlots;
    public int selected_character;
    public GameObject messagePanel, mageSpellPanel, mageSpellContent, PriestSpellPanel, priestSpellContent, lineItemText_PF, itemInteractPanel, spellInteractPanel, partyTargetPanel;

    private int _selected_Spell_Index, _selected_Item_Index;

    public void UpdateCharacterScreen()
    {
        VitalStatsLine1.text = GameManager.ROSTER[selected_character].name + " " + GameManager.ROSTER[selected_character].alignment.ToString()[0] + "-" + GameManager.ROSTER[selected_character].job.ToString().Substring(0,3) + " ";
        if (GameManager.ROSTER[selected_character].killedWerdna) VitalStatsLine1.text += ">";
        if (GameManager.ROSTER[selected_character].blessedByGnilda) VitalStatsLine1.text += "G";
        if (GameManager.ROSTER[selected_character].knighted) VitalStatsLine1.text += "K";
        if (GameManager.ROSTER[selected_character].starOfLlylgamyn) VitalStatsLine1.text += "*";

        VitalStatsLine2.text = "Level " + GameManager.ROSTER[selected_character].level + "     Age " + (int)GameManager.ROSTER[selected_character].weeksOld / 52;

        StatBlock.text = GameManager.ROSTER[selected_character].strength + "\n" +
                         GameManager.ROSTER[selected_character].iq + "\n" + 
                         GameManager.ROSTER[selected_character].piety + "\n" + 
                         GameManager.ROSTER[selected_character].vitality + "\n" + 
                         GameManager.ROSTER[selected_character].agility + "\n" +
                         GameManager.ROSTER[selected_character].luck + "\n";
        string _status = "OK";
        if (GameManager.ROSTER[selected_character].poisoned) _status = "Poisoned";
        if (GameManager.ROSTER[selected_character].plyze) _status = "Paralyzed";
        if (GameManager.ROSTER[selected_character].stoned) _status = "Stone";
        if (GameManager.ROSTER[selected_character].dead) _status = "Dead";
        if (GameManager.ROSTER[selected_character].ashes) _status = "Ashes";
        if (GameManager.ROSTER[selected_character].lost) _status = "Lost";
        OtherValues.text = "GP " + GameManager.ROSTER[selected_character].gold + "\n" +
                           "XP " + GameManager.ROSTER[selected_character].ep + "\n" + "\n" +
                           "HP " + GameManager.ROSTER[selected_character].hp +
                            "_" + GameManager.ROSTER[selected_character].maxHP + "\n" +
                            "AC " + GameManager.ROSTER[selected_character].ac + "\n" +
                           "Status " + _status;
        MageSlots.text = "Mage: " +
                         GameManager.ROSTER[selected_character].mageSlots[0] + " | " +
                         GameManager.ROSTER[selected_character].mageSlots[1] + " | " +
                         GameManager.ROSTER[selected_character].mageSlots[2] + " | " +
                         GameManager.ROSTER[selected_character].mageSlots[3] + " | " +
                         GameManager.ROSTER[selected_character].mageSlots[4] + " | " +
                         GameManager.ROSTER[selected_character].mageSlots[5] + " | " +
                         GameManager.ROSTER[selected_character].mageSlots[6];

        PriestSlots.text = " Prst: " +
                        GameManager.ROSTER[selected_character].priestSlots[0] + " | " +
                        GameManager.ROSTER[selected_character].priestSlots[1] + " | " +
                        GameManager.ROSTER[selected_character].priestSlots[2] + " | " +
                        GameManager.ROSTER[selected_character].priestSlots[3] + " | " +
                        GameManager.ROSTER[selected_character].priestSlots[4] + " | " +
                        GameManager.ROSTER[selected_character].priestSlots[5] + " | " +
                        GameManager.ROSTER[selected_character].priestSlots[6];
        for(int _i = 0; _i < GameManager.ROSTER[selected_character].bag.Length; _i++)
        {
            //if (GameManager.ROSTER[selected_character].bag[_i] == null) BagSlots[_i].text = "";
            BagSlots[_i].text = "";
            if (GameManager.ROSTER[selected_character].bag[_i] != null)
            {
                //if (GameManager.ROSTER[selected_character].bag[_i].identified) BagSlots[_i].text = "";
                if(!CheckRestrictFlags(GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_i].refID].allowEquip)) if (GameManager.ROSTER[selected_character].bag[_i].identified) BagSlots[_i].text = "#";
                if (GameManager.ROSTER[selected_character].bag[_i].equipped) BagSlots[_i].text = "*";
                if (GameManager.ROSTER[selected_character].bag[_i].identified) BagSlots[_i].text += GameManager.ROSTER[selected_character].bag[_i].name;
                if (!GameManager.ROSTER[selected_character].bag[_i].identified) BagSlots[_i].text += "?" + GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_i].refID].itemType.ToString();
            }
            
        }
    }

    private bool CheckRestrictFlags(string f)
    {
        bool _result = false;
        if (GameManager.ROSTER[selected_character].job == PlayerCharacter.Class.Fighter && f.Contains("F")) _result = true;
        if (GameManager.ROSTER[selected_character].job == PlayerCharacter.Class.Thief && f.Contains("T")) _result = true;
        if (GameManager.ROSTER[selected_character].job == PlayerCharacter.Class.Mage && f.Contains("M")) _result = true;
        if (GameManager.ROSTER[selected_character].job == PlayerCharacter.Class.Priest && f.Contains("P")) _result = true;
        if (GameManager.ROSTER[selected_character].job == PlayerCharacter.Class.Bishop && f.Contains("B")) _result = true;
        if (GameManager.ROSTER[selected_character].job == PlayerCharacter.Class.Lord && f.Contains("L")) _result = true;
        if (GameManager.ROSTER[selected_character].job == PlayerCharacter.Class.Samurai && f.Contains("S")) _result = true;
        if (GameManager.ROSTER[selected_character].job == PlayerCharacter.Class.Ninja && f.Contains("N")) _result = true;

        return _result;
    }

    public void CloseCharacterScreen()
    {
        Destroy(gameObject);
    }

    public void ReadMageSpells()
    {
        mageSpellPanel.SetActive(true);
        //clear old children
        if (mageSpellContent.transform.childCount > 0) foreach (Transform child in mageSpellContent.transform) Destroy(child.gameObject);
        //instantiate spells
        GameObject _go;
        for (int _i = 0; _i < GameManager.LISTS.spellList.Count; _i++)
        {
            if (GameManager.LISTS.spellList[_i].type == Spell.Type.mage)
            {
                _go = Instantiate(lineItemText_PF, mageSpellContent.transform);
                _go.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.LISTS.spellList[_i].spellTitle;
                _go.GetComponent<Line_Item_Click_Controller>().myValue = _i;
                if (!GameManager.ROSTER[selected_character].mageSpells.Contains(GameManager.LISTS.spellList[_i].spellTitle)) _go.SetActive(false);
            }
        }
    }

    public void ReadPriestSpells()
    {
        PriestSpellPanel.SetActive(true);
        //clear old children
        if (priestSpellContent.transform.childCount > 0) foreach (Transform child in priestSpellContent.transform) Destroy(child.gameObject);
        //instantiate spells
        GameObject _go;
        for (int _i = 0; _i < GameManager.LISTS.spellList.Count; _i++)
        {
            if (GameManager.LISTS.spellList[_i].type == Spell.Type.priest)
            {
                _go = Instantiate(lineItemText_PF, priestSpellContent.transform);
                _go.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.LISTS.spellList[_i].spellTitle;
                _go.GetComponent<Line_Item_Click_Controller>().myValue = _i;
                if (!GameManager.ROSTER[selected_character].priestSpells.Contains(GameManager.LISTS.spellList[_i].spellTitle)) _go.SetActive(false);
            }
        }
    }

    public void SpellClickedOn(int n)
    {
        _selected_Spell_Index = n;
        spellInteractPanel.SetActive(true);
        spellInteractPanel.transform.Find("Spell Info Panel").GetComponent<TMPro.TextMeshProUGUI>().text = "Spell: " + GameManager.LISTS.spellList[n].spellTitle 
            + "\nVerbal Component: " + GameManager.LISTS.spellList[n].spellWord + "\n\n" + GameManager.LISTS.spellList[n].spellDescription;
    }

    public void ItemClickedOn(int n)
    {
        if(GameManager.ROSTER[selected_character].bag[n] != null)
        {
            _selected_Item_Index = n;
            itemInteractPanel.SetActive(true);
            if(GameManager.ROSTER[selected_character].bag[n].identified) itemInteractPanel.transform.Find("Name Text").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.ROSTER[selected_character].bag[n].name;
            if (!GameManager.ROSTER[selected_character].bag[n].identified) itemInteractPanel.transform.Find("Name Text").GetComponent<TMPro.TextMeshProUGUI>().text = "?" + GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[n].refID].itemType.ToString(); 
            if (GameManager.ROSTER[selected_character].job == PlayerCharacter.Class.Bishop) itemInteractPanel.transform.Find("InspectButton").gameObject.SetActive(true);
            if (GameManager.ROSTER[selected_character].job != PlayerCharacter.Class.Bishop) itemInteractPanel.transform.Find("InspectButton").gameObject.SetActive(false);
        }
    }

    public void TradeItem()
    {
        if (!GameManager.PARTY.Contains(GameManager.ROSTER[selected_character]) || GameManager.PARTY.Count == 1)
        {
            messagePanel.SetActive(true);
            messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "You may not trade items unless you are in a party with other characters.";
            itemInteractPanel.SetActive(false);
        }
        else
        {
            partyTargetPanel.SetActive(true);
            for(int _i = 0; _i < partyTargetPanel.transform.Find("Panel").childCount; _i++) partyTargetPanel.transform.Find("Panel").GetChild(_i).gameObject.SetActive(false);
            for (int _i = 0; _i < GameManager.PARTY.Count; _i++)
            {
                partyTargetPanel.transform.Find("Panel").GetChild(_i).gameObject.SetActive(true);
                partyTargetPanel.transform.Find("Panel").GetChild(_i).GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[_i].name + " the " + GameManager.PARTY[_i].job.ToString();
                if (GameManager.PARTY[_i] == GameManager.ROSTER[selected_character]) partyTargetPanel.transform.Find("Panel").GetChild(_i).gameObject.SetActive(false);
            }
        }

    }

    public void TradeWithThisTarget(int n)
    {
        partyTargetPanel.SetActive(false);
        itemInteractPanel.SetActive(false);
        messagePanel.SetActive(true);
        bool _trade = false; int _selectSlot = -1;
        for(int _i = 0; _i < GameManager.PARTY[n].bag.Length; _i++) if(!_trade && GameManager.PARTY[n].bag[_i] == null) { _trade = true; _selectSlot = _i; }
        if (GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _trade = false;
        if (!_trade)
        {
            messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "Unable to trade item to this character.";
        }
        else
        {
            messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.ROSTER[selected_character].bag[_selected_Item_Index].name + " traded to " + GameManager.PARTY[n].name;
            GameManager.PARTY[n].bag[_selectSlot] = new ItemInstance(GameManager.ROSTER[selected_character].bag[_selected_Item_Index]);
            GameManager.ROSTER[selected_character].bag[_selected_Item_Index].equipped = false;
            GameManager.ROSTER[selected_character].bag[_selected_Item_Index] = null;
            UpdateCharacterScreen();
            SaveLoadModule.SaveGame();
        }
    }

    public void EquipItem()
    {
        //Determine if the item is already equipped or not
        if (GameManager.ROSTER[selected_character].bag[_selected_Item_Index].equipped)
        {
            //first, check for active curse
            if (GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive)
            {
                //based on item type, clear the equipped reference
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Helm) GameManager.ROSTER[selected_character].head = null;
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Shield) GameManager.ROSTER[selected_character].shield = null;
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Armor) GameManager.ROSTER[selected_character].body = null;
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Gloves ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Ring ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Amulet) GameManager.ROSTER[selected_character].jewelry = null;
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Sword ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Axe ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Flail ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Mace ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Dagger ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Staff) GameManager.ROSTER[selected_character].weapon = null;

                //mark the ItemInstance as unequipped
                GameManager.ROSTER[selected_character].bag[_selected_Item_Index].equipped = false;

                //report result
                itemInteractPanel.SetActive(false);
                UpdateCharacterScreen();
                messagePanel.SetActive(true);
                messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "Item Unequipped.";
            }
            else //item is cursed, cannot unequip.
            {
                itemInteractPanel.SetActive(false);
                UpdateCharacterScreen();
                messagePanel.SetActive(true);
                messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "Item is cursed, cannot unequip.";
            }
        }
        else //the item is not already equipped
        {
            //Check for restrictions  Misc, Key, Rod, Scroll, Potion, Figurine
            if (CheckRestrictFlags(GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].allowEquip) ||
                GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Misc ||
                GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Key ||
                GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Rod ||
                GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Scroll ||
                GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Potion ||
                GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Figurine)
            {
                //based on item type, equip the item reference
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Helm)
                {
                    if (GameManager.ROSTER[selected_character].head != null) GameManager.ROSTER[selected_character].head.equipped = false;
                    GameManager.ROSTER[selected_character].head = GameManager.ROSTER[selected_character].bag[_selected_Item_Index];
                }
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Shield)
                {
                    if (GameManager.ROSTER[selected_character].shield != null) GameManager.ROSTER[selected_character].shield.equipped = false;
                    GameManager.ROSTER[selected_character].shield = GameManager.ROSTER[selected_character].bag[_selected_Item_Index];
                }
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Armor) 
                {
                    if (GameManager.ROSTER[selected_character].body != null) GameManager.ROSTER[selected_character].body.equipped = false;
                    GameManager.ROSTER[selected_character].body = GameManager.ROSTER[selected_character].bag[_selected_Item_Index];
                }
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Gloves ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Ring)
                {
                    if (GameManager.ROSTER[selected_character].jewelry != null) GameManager.ROSTER[selected_character].jewelry.equipped = false;
                    GameManager.ROSTER[selected_character].jewelry = GameManager.ROSTER[selected_character].bag[_selected_Item_Index];
                }
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Sword ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Axe ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Flail ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Mace ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Dagger ||
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Staff)
                {
                    if (GameManager.ROSTER[selected_character].weapon != null) GameManager.ROSTER[selected_character].weapon.equipped = false;
                    GameManager.ROSTER[selected_character].weapon = GameManager.ROSTER[selected_character].bag[_selected_Item_Index];
                }
                //mark the ItemInstance as equipped
                GameManager.ROSTER[selected_character].bag[_selected_Item_Index].equipped = true;

                //CURSES!
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].cursed) GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive = true;
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment != Item.Alignment.Any &&
                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment.ToString() !=
                    GameManager.ROSTER[selected_character].alignment.ToString()) GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive = true;


                //recalculate Armor Class
                int _tAc = 0;
                if (GameManager.ROSTER[selected_character].head != null && !GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _tAc += GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].head.refID].acBonus;
                if (GameManager.ROSTER[selected_character].head != null && GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _tAc -= GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].head.refID].acBonus;
                if (GameManager.ROSTER[selected_character].body != null && !GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _tAc += GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].body.refID].acBonus;
                if (GameManager.ROSTER[selected_character].body != null && !GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _tAc -= GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].body.refID].acBonus;
                if (GameManager.ROSTER[selected_character].shield != null && !GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _tAc += GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].shield.refID].acBonus;
                if (GameManager.ROSTER[selected_character].shield != null && GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _tAc -= GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].shield.refID].acBonus;
                if (GameManager.ROSTER[selected_character].weapon != null && !GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _tAc += GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].weapon.refID].acBonus;
                if (GameManager.ROSTER[selected_character].weapon != null && GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _tAc -= GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].weapon.refID].acBonus;
                if (GameManager.ROSTER[selected_character].jewelry != null && !GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _tAc += GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].jewelry.refID].acBonus;
                if (GameManager.ROSTER[selected_character].jewelry != null && GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _tAc -= GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].jewelry.refID].acBonus;
                GameManager.ROSTER[selected_character].ac = 10 - _tAc;
                GameManager.GAME.UpdatePartyPanel();

                //report result
                itemInteractPanel.SetActive(false);
                UpdateCharacterScreen();
                messagePanel.SetActive(true);
                messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "Item Equipped.";
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].cursed) messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text += "The character is cursed!";
            }
            else //you are not allowed to equip this (restriction fail)
            {
                itemInteractPanel.SetActive(false);
                UpdateCharacterScreen();
                messagePanel.SetActive(true);
                messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "Unable to equip this item to this character.";                
            }
        }
    }

    public void DropItem()
    {
        if (!GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive)
        {
            GameManager.ROSTER[selected_character].bag[_selected_Item_Index] = null;
            itemInteractPanel.SetActive(false);
            UpdateCharacterScreen();
            SaveLoadModule.SaveGame();
            messagePanel.SetActive(true);
            messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "Item dropped.";
        }
        else
        {
            itemInteractPanel.SetActive(false);
            messagePanel.SetActive(true);
            messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "Cannot drop cursed item.";
        }
    }

    public void IdentifyItem()
    {

    }
}
