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
            if(GameManager.ROSTER[selected_character].bag[_i] != null) BagSlots[_i].text = GameManager.ROSTER[selected_character].bag[_i].name;
            if (GameManager.ROSTER[selected_character].bag[_i] == null) BagSlots[_i].text = "";
        }
    }

    public void CloseCharacterScreen()
    {
        Destroy(gameObject);
    }
}
