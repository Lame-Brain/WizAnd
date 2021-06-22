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
        VitalStatsLine1.text = GameManager.ROSTER[selected_character].name + " " + GameManager.ROSTER[selected_character].alignment.ToString()[0] + "-" + GameManager.ROSTER[selected_character].job.ToString().Substring(0, 3) + " ";
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
        for (int _i = 0; _i < GameManager.ROSTER[selected_character].bag.Length; _i++)
        {
            //if (GameManager.ROSTER[selected_character].bag[_i] == null) BagSlots[_i].text = "";
            BagSlots[_i].text = "";
            if (GameManager.ROSTER[selected_character].bag[_i] != null)
            {
                //if (GameManager.ROSTER[selected_character].bag[_i].identified) BagSlots[_i].text = "";
                BagSlots[_i].text = "";
                if (!CheckRestrictFlags(GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_i].refID].allowEquip)) if (GameManager.ROSTER[selected_character].bag[_i].identified) BagSlots[_i].text = "#";
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
        if (GameManager.ROSTER[selected_character].bag[n] != null)
        {
            _selected_Item_Index = n;
            itemInteractPanel.SetActive(true);
            if (GameManager.ROSTER[selected_character].bag[n].identified) itemInteractPanel.transform.Find("Name Text").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.ROSTER[selected_character].bag[n].name;
            if (!GameManager.ROSTER[selected_character].bag[n].identified) itemInteractPanel.transform.Find("Name Text").GetComponent<TMPro.TextMeshProUGUI>().text = "?" + GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[n].refID].itemType.ToString();
            if (GameManager.ROSTER[selected_character].job == PlayerCharacter.Class.Bishop) itemInteractPanel.transform.Find("InspectButton").gameObject.SetActive(true);
            if (GameManager.ROSTER[selected_character].job != PlayerCharacter.Class.Bishop) itemInteractPanel.transform.Find("InspectButton").gameObject.SetActive(false);
        }
    }

    public void TradeItem()
    {
        if (!GameManager.PARTY.Contains(selected_character) || GameManager.PARTY.Count == 1)
        {
            messagePanel.SetActive(true);
            messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "You may not trade items unless you are in a party with other characters.";
            itemInteractPanel.SetActive(false);
        }
        else
        {
            partyTargetPanel.SetActive(true);
            for (int _i = 0; _i < partyTargetPanel.transform.Find("Panel").childCount; _i++) partyTargetPanel.transform.Find("Panel").GetChild(_i).gameObject.SetActive(false);
            for (int _i = 0; _i < GameManager.PARTY.Count; _i++)
            {
                partyTargetPanel.transform.Find("Panel").GetChild(_i).gameObject.SetActive(true);
                partyTargetPanel.transform.Find("Panel").GetChild(_i).GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.ROSTER[GameManager.PARTY[_i]].name + " the " + GameManager.ROSTER[GameManager.PARTY[_i]].job.ToString();
                if (GameManager.PARTY[_i] == selected_character) partyTargetPanel.transform.Find("Panel").GetChild(_i).gameObject.SetActive(false);
            }
        }

    }

    public void TradeWithThisTarget(int n)
    {
        partyTargetPanel.SetActive(false);
        itemInteractPanel.SetActive(false);
        messagePanel.SetActive(true);
        bool _trade = false; int _selectSlot = -1;
        for (int _i = 0; _i < GameManager.ROSTER[GameManager.PARTY[n]].bag.Length; _i++) if (!_trade && GameManager.ROSTER[GameManager.PARTY[n]].bag[_i] == null) { _trade = true; _selectSlot = _i; }
        if (GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive) _trade = false;
        if (!_trade)
        {
            messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "Unable to trade item to this character.";
        }
        else
        {
            messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.ROSTER[selected_character].bag[_selected_Item_Index].name + " traded to " + GameManager.ROSTER[GameManager.PARTY[n]].name;
            GameManager.ROSTER[GameManager.PARTY[n]].bag[_selectSlot] = new ItemInstance(GameManager.ROSTER[selected_character].bag[_selected_Item_Index]);
            GameManager.ROSTER[selected_character].bag[_selected_Item_Index].equipped = false;
            GameManager.ROSTER[selected_character].bag[_selected_Item_Index] = null;
            UpdateCharacterScreen();
            SaveLoadModule.SaveGame();
        }
    }

    public void EquipItem()
    {
        //Determine if the item is already equipped or not
        if (GameManager.ROSTER[selected_character].bag[_selected_Item_Index].equipped) //If it is equipped, Unequip it.
        {
            //first, check for active curse
            if (!GameManager.ROSTER[selected_character].bag[_selected_Item_Index].curseActive)
            {
                //If it is not curesd, then based on item type, clear the equipped reference
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
        else //the item is not already equipped, and needs to be equipped.
        {
            bool _curseFail = false, _equipFail = false, _cursed = false;
            //1. determine if the slot that this item needs to go in has a cursed item in it. If so, fail.
            //2. if the slot that this item needs to go into has an uncursed item, 
            //   A. clear that item's equipped flag
            //   B. clear the slot
            //   C. Assign the item to the slot
            //   D. mark the item's equipped flag
            //3. is the item that was just equipped a cursed item? Does it not match the alignment of the character who equipped it? if so, mark curse active

            //Cannot equip Keys, Potions, Scrolls, Figurines, Rods, or Misc items.
            if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Key) _equipFail = true;
            if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Potion) _equipFail = true;
            if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Scroll) _equipFail = true;
            if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Figurine) _equipFail = true;
            if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Rod) _equipFail = true;
            if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Misc) _equipFail = true;

            //Check destination slot for active curses
            if (GameManager.ROSTER[selected_character].head != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Helm && GameManager.ROSTER[selected_character].head.curseActive) _curseFail = true;
            if (GameManager.ROSTER[selected_character].shield != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Shield && GameManager.ROSTER[selected_character].shield.curseActive) _curseFail = true;
            if (GameManager.ROSTER[selected_character].body != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Armor && GameManager.ROSTER[selected_character].body.curseActive) _curseFail = true;
            if (GameManager.ROSTER[selected_character].jewelry != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Gloves && GameManager.ROSTER[selected_character].jewelry.curseActive) _curseFail = true;
            if (GameManager.ROSTER[selected_character].jewelry != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Ring && GameManager.ROSTER[selected_character].jewelry.curseActive) _curseFail = true;
            if (GameManager.ROSTER[selected_character].jewelry != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Amulet && GameManager.ROSTER[selected_character].jewelry.curseActive) _curseFail = true;
            if (GameManager.ROSTER[selected_character].weapon != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Sword && GameManager.ROSTER[selected_character].weapon.curseActive) _curseFail = true;
            if (GameManager.ROSTER[selected_character].weapon != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Axe && GameManager.ROSTER[selected_character].weapon.curseActive) _curseFail = true;
            if (GameManager.ROSTER[selected_character].weapon != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Flail && GameManager.ROSTER[selected_character].weapon.curseActive) _curseFail = true;
            if (GameManager.ROSTER[selected_character].weapon != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Mace && GameManager.ROSTER[selected_character].weapon.curseActive) _curseFail = true;
            if (GameManager.ROSTER[selected_character].weapon != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Dagger && GameManager.ROSTER[selected_character].weapon.curseActive) _curseFail = true;
            if (GameManager.ROSTER[selected_character].weapon != null && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Staff && GameManager.ROSTER[selected_character].weapon.curseActive) _curseFail = true;

            //If the slot already has an item, and that item is not cursed, clear the equipped flag from the item
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Helm && GameManager.ROSTER[selected_character].head != null) GameManager.ROSTER[selected_character].head.equipped = false;
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Shield && GameManager.ROSTER[selected_character].shield != null) GameManager.ROSTER[selected_character].shield.equipped = false;
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Armor && GameManager.ROSTER[selected_character].body != null) GameManager.ROSTER[selected_character].body.equipped = false;
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Gloves && GameManager.ROSTER[selected_character].jewelry != null) GameManager.ROSTER[selected_character].jewelry.equipped = false;
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Ring && GameManager.ROSTER[selected_character].jewelry != null) GameManager.ROSTER[selected_character].jewelry.equipped = false;
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Amulet && GameManager.ROSTER[selected_character].jewelry != null) GameManager.ROSTER[selected_character].jewelry.equipped = false;
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Sword && GameManager.ROSTER[selected_character].weapon != null) GameManager.ROSTER[selected_character].weapon.equipped = false;
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Axe && GameManager.ROSTER[selected_character].weapon != null) GameManager.ROSTER[selected_character].weapon.equipped = false;
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Flail && GameManager.ROSTER[selected_character].weapon != null) GameManager.ROSTER[selected_character].weapon.equipped = false;
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Mace && GameManager.ROSTER[selected_character].weapon != null) GameManager.ROSTER[selected_character].weapon.equipped = false;
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Dagger && GameManager.ROSTER[selected_character].weapon != null) GameManager.ROSTER[selected_character].weapon.equipped = false;
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Staff && GameManager.ROSTER[selected_character].weapon != null) GameManager.ROSTER[selected_character].weapon.equipped = false;

            //Equip the item to the slot and mark it equipped
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Helm)
            {
                GameManager.ROSTER[selected_character].head = GameManager.ROSTER[selected_character].bag[_selected_Item_Index];
                GameManager.ROSTER[selected_character].head.equipped = true;
                //if the item is cursed, apply the curse
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].cursed) { GameManager.ROSTER[selected_character].head.curseActive = true; _cursed = true; }
                //if the alignment is mis-matched, apply the curse
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment != Item.Alignment.Any && GameManager.ROSTER[selected_character].alignment.ToString() != GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment.ToString())
                { GameManager.ROSTER[selected_character].head.curseActive = true; _cursed = true; }
            }
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Shield)
            {
                GameManager.ROSTER[selected_character].shield = GameManager.ROSTER[selected_character].bag[_selected_Item_Index];
                GameManager.ROSTER[selected_character].shield.equipped = true;
                //if the item is cursed, apply the curse
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].cursed) { GameManager.ROSTER[selected_character].shield.curseActive = true; _cursed = true; }
                //if the alignment is mis-matched, apply the curse
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment != Item.Alignment.Any && GameManager.ROSTER[selected_character].alignment.ToString() != GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment.ToString())
                { GameManager.ROSTER[selected_character].shield.curseActive = true; _cursed = true; }
            }
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Armor)
            {
                GameManager.ROSTER[selected_character].body = GameManager.ROSTER[selected_character].bag[_selected_Item_Index];
                GameManager.ROSTER[selected_character].body.equipped = true;
                //if the item is cursed, apply the curse
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].cursed) { GameManager.ROSTER[selected_character].body.curseActive = true; _cursed = true; }
                //if the alignment is mis-matched, apply the curse
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment != Item.Alignment.Any && GameManager.ROSTER[selected_character].alignment.ToString() != GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment.ToString())
                { GameManager.ROSTER[selected_character].body.curseActive = true; _cursed = true; }
            }
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Gloves ||
                                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Ring ||
                                    GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Amulet)
            {
                GameManager.ROSTER[selected_character].jewelry = GameManager.ROSTER[selected_character].bag[_selected_Item_Index];
                GameManager.ROSTER[selected_character].jewelry.equipped = true;
                //if the item is cursed, apply the curse
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].cursed) { GameManager.ROSTER[selected_character].jewelry.curseActive = true; _cursed = true; }
                //if the alignment is mis-matched, apply the curse
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment != Item.Alignment.Any && GameManager.ROSTER[selected_character].alignment.ToString() != GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment.ToString())
                { GameManager.ROSTER[selected_character].jewelry.curseActive = true; _cursed = true; }
            }
            if (!_curseFail && GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Sword ||
                                GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Axe ||
                                GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Flail ||
                                GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Mace ||
                                GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Dagger ||
                                GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].itemType == Item.Type.Staff)
            {
                GameManager.ROSTER[selected_character].weapon = GameManager.ROSTER[selected_character].bag[_selected_Item_Index];
                GameManager.ROSTER[selected_character].weapon.equipped = true;
                //if the item is cursed, apply the curse
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].cursed) { GameManager.ROSTER[selected_character].weapon.curseActive = true; _cursed = true; }
                //if the alignment is mis-matched, apply the curse
                if (GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment != Item.Alignment.Any && GameManager.ROSTER[selected_character].alignment.ToString() != GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_selected_Item_Index].refID].alignment.ToString())
                { GameManager.ROSTER[selected_character].weapon.curseActive = true; _cursed = true; }
            }

            //Report results
            itemInteractPanel.SetActive(false);
            messagePanel.SetActive(true);
            if (_equipFail) messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "That item cannot be equipped.";
            if (_curseFail) messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "That item cannot be equipped because of a curse.";
            if (!_equipFail && !_curseFail) messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text = "Item has been equipped!";
            if (_cursed) messagePanel.transform.Find("MessageText").GetComponent<TMPro.TextMeshProUGUI>().text += "\nOh no! The item is cursed!";
        }

        //Update AC, update party panel, update character screen, update save game.
        int _totalACBonus = 0;
        if (GameManager.ROSTER[selected_character].head != null && !GameManager.ROSTER[selected_character].head.curseActive) _totalACBonus += GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].head.refID].acBonus;
        if (GameManager.ROSTER[selected_character].head != null && GameManager.ROSTER[selected_character].head.curseActive) _totalACBonus -= GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].head.refID].acBonus;
        if (GameManager.ROSTER[selected_character].shield != null && !GameManager.ROSTER[selected_character].shield.curseActive) _totalACBonus += GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].shield.refID].acBonus;
        if (GameManager.ROSTER[selected_character].shield != null && GameManager.ROSTER[selected_character].shield.curseActive) _totalACBonus -= GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].shield.refID].acBonus;
        if (GameManager.ROSTER[selected_character].body != null && !GameManager.ROSTER[selected_character].body.curseActive) _totalACBonus += GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].body.refID].acBonus;
        if (GameManager.ROSTER[selected_character].body != null && GameManager.ROSTER[selected_character].body.curseActive) _totalACBonus -= GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].body.refID].acBonus;
        if (GameManager.ROSTER[selected_character].jewelry != null && !GameManager.ROSTER[selected_character].jewelry.curseActive) _totalACBonus += GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].jewelry.refID].acBonus;
        if (GameManager.ROSTER[selected_character].jewelry != null && GameManager.ROSTER[selected_character].jewelry.curseActive) _totalACBonus -= GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].jewelry.refID].acBonus;
        if (GameManager.ROSTER[selected_character].weapon != null && !GameManager.ROSTER[selected_character].weapon.curseActive) _totalACBonus += GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].weapon.refID].acBonus;
        if (GameManager.ROSTER[selected_character].weapon != null && GameManager.ROSTER[selected_character].weapon.curseActive) _totalACBonus -= GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].weapon.refID].acBonus;
        GameManager.ROSTER[selected_character].ac = 10 - _totalACBonus;
        GameManager.GAME.UpdatePartyPanel();
        UpdateCharacterScreen();
        SaveLoadModule.SaveGame();
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
