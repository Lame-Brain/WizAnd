using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterGenScreenController : MonoBehaviour
{
    public  TMPro.TextMeshProUGUI statOutputText;
    public TMPro.TMP_InputField NameInput;
    public TMPro.TMP_Dropdown AlignDropValue;
    public TMPro.TMP_Dropdown RaceDropValue;    
    public GameObject panelFighter, panelMage, panelPriest, panelThief, panelLord, panelBishop, panelSamurai, panelNinja, panelFinishCharacter, rerollButton;
    [HideInInspector]
    public int valueSTR, valueIQ, valuePIETY, valueVITALITY, valueAGILITY, valueLUCK, valueBONUS;
    [HideInInspector]
    public string selected_name, selected_alignment, selected_race;

    private PlayerCharacter NewToon = new PlayerCharacter();

    public void InitCharacterGeneration()
    {
        NameInput.text = "";
        AlignDropValue.value = 0;
        RaceDropValue.value = 0;
        RollStats();
    }

    public void UpdateScreen()
    {
        statOutputText.text = valueSTR + "\n" + valueIQ + "\n" + valuePIETY + "\n" + valueVITALITY + "\n" + valueAGILITY + "\n" + valueLUCK + "\n" + "\n" + valueBONUS;

        panelFighter.SetActive(false); panelMage.SetActive(false); panelPriest.SetActive(false); panelThief.SetActive(false); panelLord.SetActive(false); panelBishop.SetActive(false); panelSamurai.SetActive(false); panelNinja.SetActive(false);
        if (valueSTR >= 11) panelFighter.SetActive(true);
        if (valueIQ >= 11) panelMage.SetActive(true);
        if (valuePIETY >= 11 && (AlignDropValue.value == 1 || AlignDropValue.value == 3)) panelPriest.SetActive(true);
        if (valueAGILITY >= 11 && (AlignDropValue.value == 2 || AlignDropValue.value == 3)) panelThief.SetActive(true);
        if (valueSTR > 14 && valueIQ > 11 && valuePIETY > 11 && valueVITALITY > 14 && valueAGILITY > 13 && valueLUCK > 14 && AlignDropValue.value == 1) panelLord.SetActive(true);
        if (valueIQ > 11 && valuePIETY > 11 && (AlignDropValue.value == 1 || AlignDropValue.value == 3)) panelBishop.SetActive(true);
        if (valueSTR > 14 && valueIQ > 10 && valuePIETY > 9 && valueVITALITY > 13 && valueAGILITY > 9 && (AlignDropValue.value == 1 || AlignDropValue.value == 2)) panelSamurai.SetActive(true);
        if (valueSTR > 16 && valueIQ > 16 && valuePIETY > 16 && valueVITALITY > 16 && valueAGILITY > 16 && valueLUCK > 16 && AlignDropValue.value == 3) panelNinja.SetActive(true);

        if (NameInput.text != "") selected_name = NameInput.text;

        if (AlignDropValue.value == 0) selected_alignment = "";
        if (AlignDropValue.value == 1) selected_alignment = "Good";
        if (AlignDropValue.value == 2) selected_alignment = "Neutral";
        if (AlignDropValue.value == 3) selected_alignment = "Evil";

        if (selected_race == "") rerollButton.SetActive(false);
        if (selected_race != "") rerollButton.SetActive(true);

    }

    public void RollStats()
    {
        if(RaceDropValue.value == 0)
        {
            selected_race = "";
            valueSTR = 3;
            valueIQ = 3;
            valuePIETY = 3;
            valueAGILITY = 3;
            valueVITALITY = 3;
            valueLUCK = 3;
        }

        if (RaceDropValue.value == 1)
        {
            selected_race = "Human";
            valueSTR = 8;
            valueIQ = 8;
            valuePIETY = 5;
            valueAGILITY = 8;
            valueVITALITY = 8;
            valueLUCK = 9;
        }

        if(RaceDropValue.value == 2)
        {
            selected_race = "Elf";
            valueSTR = 7;
            valueIQ = 10;
            valuePIETY = 10;
            valueAGILITY = 6;
            valueVITALITY = 9;
            valueLUCK = 6;
        }

        if(RaceDropValue.value == 3)
        {
            selected_race = "Dwarf";
            valueSTR = 10;
            valueIQ = 7;
            valuePIETY = 10;
            valueAGILITY = 10;
            valueVITALITY = 5;
            valueLUCK = 6;
        }

        if(RaceDropValue.value == 4)
        {
            selected_race = "Gnome";
            valueSTR = 7;
            valueIQ = 7;
            valuePIETY = 10;
            valueAGILITY = 8;
            valueVITALITY = 10;
            valueLUCK = 7;
        }

        if(RaceDropValue.value == 5)
        {
            selected_race = "Hobbit";
            valueSTR = 5;
            valueIQ = 7;
            valuePIETY = 7;
            valueAGILITY = 6;
            valueVITALITY = 10;
            valueLUCK = 15;            
        }

        valueBONUS = 0;
        valueBONUS += Random.Range(1, 4) + 6;
        if (Random.Range(1, 11) == 1) valueBONUS += 10;
        if (valueBONUS <= 20 && Random.Range(1, 11) == 1) valueBONUS += 10;        
        if (selected_race == "") valueBONUS = 3;

        UpdateScreen();
    }

    public void IncreaseSTR()
    {
        if (valueBONUS > 0 && valueSTR < 18)
        {
            valueSTR++;
            valueBONUS--;
            UpdateScreen();
        }
    }

    public void DecreaseSTR()
    {
        if(valueSTR > 3)
        {
            valueSTR--;
            valueBONUS++;
            UpdateScreen();
        }
    }

    public void IncreaseIQ()
    {
        if (valueBONUS > 0 && valueIQ < 18)
        {
            valueIQ++;
            valueBONUS--;
            UpdateScreen();
        }
    }

    public void DecreaseIQ()
    {
        if(valueIQ > 3)
        {
            valueIQ--;
            valueBONUS++;
            UpdateScreen();
        }
    }

    public void IncreasePIETY()
    {
        if (valueBONUS > 0 && valuePIETY < 18)
        {
            valuePIETY++;
            valueBONUS--;
            UpdateScreen();
        }
    }

    public void DecreasePIETY()
    {
        if(valuePIETY > 3)
        {
            valuePIETY--;
            valueBONUS++;
            UpdateScreen();
        }
    }

    public void IncreaseVITALITY()
    {
        if (valueBONUS > 0 && valueVITALITY < 18)
        {
            valueVITALITY++;
            valueBONUS--;
            UpdateScreen();
        }
    }

    public void DecreaseVITALITY()
    {
        if(valueVITALITY > 3)
        {
            valueVITALITY--;
            valueBONUS++;
            UpdateScreen();
        }
    }

    public void IncreaseAGILITY()
    {
        if (valueBONUS > 0 && valueAGILITY < 18)
        {
            valueAGILITY++;
            valueBONUS--;
            UpdateScreen();
        }
    }

    public void DecreaseAGILITY()
    {
        if(valueAGILITY > 3)
        {
            valueAGILITY--;
            valueBONUS++;
            UpdateScreen();
        }
    }

    public void IncreaseLUCK()
    {
        if (valueBONUS > 0 && valueLUCK < 18)
        {
            valueLUCK++;
            valueBONUS--;
            UpdateScreen();
        }
    }

    public void DecreaseLUCK()
    {
        if(valueLUCK > 3)
        {
            valueLUCK--;
            valueBONUS++;
            UpdateScreen();
        }
    }

    public void YouClickedAClassCard(int c)
    {
        if(selected_name != "" && selected_race != "" && selected_alignment != "")
        {
            //Create Character
            NewToon.name = selected_name;

            //Set class
            if (c == 1) NewToon.job = PlayerCharacter.Class.Fighter;
            if (c == 2) NewToon.job = PlayerCharacter.Class.Mage;
            if (c == 3) NewToon.job = PlayerCharacter.Class.Priest;
            if (c == 4) NewToon.job = PlayerCharacter.Class.Thief;
            if (c == 5) NewToon.job = PlayerCharacter.Class.Lord;
            if (c == 6) NewToon.job = PlayerCharacter.Class.Bishop;
            if (c == 7) NewToon.job = PlayerCharacter.Class.Samurai;
            if (c == 8) NewToon.job = PlayerCharacter.Class.Ninja;

            //Set race
            if (selected_race == "Human") NewToon.race = PlayerCharacter.Race.Human;
            if (selected_race == "Elf") NewToon.race = PlayerCharacter.Race.Elf;
            if (selected_race == "Dwarf") NewToon.race = PlayerCharacter.Race.Dwarf;
            if (selected_race == "Gnome") NewToon.race = PlayerCharacter.Race.Gnome;
            if (selected_race == "Hobbit") NewToon.race = PlayerCharacter.Race.Hobbit;

            //Set Alignment
            if (selected_alignment == "Good") NewToon.alignment = PlayerCharacter.Alignment.Good;
            if (selected_alignment == "Neutral") NewToon.alignment = PlayerCharacter.Alignment.Neutral;
            if (selected_alignment == "Evil") NewToon.alignment = PlayerCharacter.Alignment.Evil;

            //Set Age (936 to 1235)
            NewToon.weeksOld = Random.Range(936, 1236);

            //Set Status
            NewToon.poisoned = false;
            NewToon.afraid = false;
            NewToon.asleep = false;
            NewToon.plyze = false;
            NewToon.stoned = false;
            NewToon.dead = false;
            NewToon.ashes = false;
            NewToon.lost = false;

            //Set Awards
            NewToon.killedWerdna = false;
            NewToon.blessedByGnilda = false;
            NewToon.knighted = false;
            NewToon.starOfLlylgamyn = false;

            //Set Stats
            NewToon.strength = valueSTR;
            NewToon.iq = valueIQ;
            NewToon.piety = valuePIETY;
            NewToon.vitality = valueVITALITY;
            NewToon.agility = valueAGILITY;
            NewToon.luck = valueLUCK;
            NewToon.gold = Random.Range(90, 190);
            NewToon.ep = 0;
            NewToon.level = 1;
            NewToon.maxLevel = 1;
            NewToon.ac = 10;

            //Set Health
            int _vitalityBonus = 0;
            if (NewToon.vitality < 6) _vitalityBonus = -1;
            if (NewToon.vitality < 4) _vitalityBonus = -2;
            if (NewToon.vitality == 16) _vitalityBonus = 1;
            if (NewToon.vitality == 17) _vitalityBonus = 2;
            if (NewToon.vitality == 18) _vitalityBonus = 3;            
            if (NewToon.job == PlayerCharacter.Class.Fighter) NewToon.hp += Random.Range(6, 11) + _vitalityBonus;
            if (NewToon.job == PlayerCharacter.Class.Thief) NewToon.hp += Random.Range(5, 7) + _vitalityBonus;
            if (NewToon.job == PlayerCharacter.Class.Priest) NewToon.hp += Random.Range(6, 9) + _vitalityBonus;
            if (NewToon.job == PlayerCharacter.Class.Mage) NewToon.hp += Random.Range(3, 5) + _vitalityBonus;
            if (NewToon.job == PlayerCharacter.Class.Lord) NewToon.hp += Random.Range(6, 11) + _vitalityBonus;
            if (NewToon.job == PlayerCharacter.Class.Samurai) NewToon.hp += Random.Range(11, 17) + _vitalityBonus;
            if (NewToon.job == PlayerCharacter.Class.Ninja) NewToon.hp += Random.Range(5, 7) + _vitalityBonus;
            if (NewToon.job == PlayerCharacter.Class.Bishop) NewToon.hp += Random.Range(4, 7) + _vitalityBonus;
            NewToon.maxHP = NewToon.hp; //set new max hp

            //set equipment
            NewToon.head = null;
            NewToon.body = null;
            NewToon.shield = null;
            NewToon.weapon = null;
            NewToon.jewelry = null;
            NewToon.bag = new ItemInstance[10];
            for (int _i = 0; _i < NewToon.bag.Length; _i++) NewToon.bag[_i] = null;

            //Set Spells
            for (int _i = 0; _i < 7; _i++) NewToon.mageSlots[_i] = 0;
            for (int _i = 0; _i < 7; _i++) NewToon.priestSlots[_i] = 0;
            if(NewToon.job == PlayerCharacter.Class.Bishop || NewToon.job == PlayerCharacter.Class.Mage)
            {
                NewToon.mageSlots[0] = 2;
                NewToon.mageSpells.Add("Fire Bolt");
                NewToon.mageSpells.Add("Sleep");
            }
            if(NewToon.job == PlayerCharacter.Class.Priest)
            {
                NewToon.priestSlots[0] = 2;
                NewToon.priestSpells.Add("Heal");
                NewToon.priestSpells.Add("Harm");
            }

            //Set Saving Throws
            NewToon.CalculateSaves();

            //Ask to Finish Character and save
            panelFinishCharacter.SetActive(true);
        }
    }

    public void SaveCharacter()
    {
        GameManager.ROSTER.Add(NewToon);
        SaveLoadModule.SaveGame();
    }
}
