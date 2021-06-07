using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScreenController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI VitalStatsLine1, VitalStatsLine2, StatBlock, OtherValues, MageSlots, PriestSlots;
    public TMPro.TextMeshProUGUI[] BagSlots;
    public int selected_character;

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
                             "_" + GameManager.ROSTER[selected_character].maxHP + "\n" + "\n" +
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
            if (GameManager.ROSTER[selected_character].bag[_i] == null) BagSlots[_i].text = "";
            if (GameManager.ROSTER[selected_character].bag[_i] != null)
            {
                if (GameManager.ROSTER[selected_character].bag[_i].identified) BagSlots[_i].text = "";
                if(!CheckRestrictFlags(GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_i].refID].allowEquip)) if (GameManager.ROSTER[selected_character].bag[_i].identified) BagSlots[_i].text = "#";
                if (GameManager.ROSTER[selected_character].bag[_i].identified) BagSlots[_i].text += GameManager.ROSTER[selected_character].bag[_i].name;
                if (!GameManager.ROSTER[selected_character].bag[_i].identified) BagSlots[_i].text = "?" + GameManager.LISTS.itemList[GameManager.ROSTER[selected_character].bag[_i].refID].itemType.ToString();
            }
            
        }
    }

    private bool CheckRestrictFlags(string f)
    {
        bool _result = false;
        if (GameManager.PARTY[selected_character].job == PlayerCharacter.Class.Fighter && f.Contains("F")) _result = true;
        if (GameManager.PARTY[selected_character].job == PlayerCharacter.Class.Thief && f.Contains("T")) _result = true;
        if (GameManager.PARTY[selected_character].job == PlayerCharacter.Class.Mage && f.Contains("M")) _result = true;
        if (GameManager.PARTY[selected_character].job == PlayerCharacter.Class.Priest && f.Contains("P")) _result = true;
        if (GameManager.PARTY[selected_character].job == PlayerCharacter.Class.Bishop && f.Contains("B")) _result = true;
        if (GameManager.PARTY[selected_character].job == PlayerCharacter.Class.Lord && f.Contains("L")) _result = true;
        if (GameManager.PARTY[selected_character].job == PlayerCharacter.Class.Samurai && f.Contains("S")) _result = true;
        if (GameManager.PARTY[selected_character].job == PlayerCharacter.Class.Ninja && f.Contains("N")) _result = true;

        return _result;
    }

    public void CloseCharacterScreen()
    {
        Destroy(gameObject);
    }
}
