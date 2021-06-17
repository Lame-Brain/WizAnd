using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inn_ScreenPanelController : MonoBehaviour
{
    public GameObject WhoShopsPanel, ServicesPanel, MessagePanel, partyPanel;

    private int _selected_Character;

    public void DrawWhoShopsMenu()
    {
        if (GameManager.PARTY.Count == 0) this.gameObject.SetActive(false);
        if (GameManager.PARTY.Count > 0)
        {
            WhoShopsPanel.SetActive(true);
            ServicesPanel.SetActive(false);
            MessagePanel.SetActive(false);
            for (int _i = 0; _i < 6; _i++) this.transform.Find("WhoShopsMenuPanel").GetChild(_i).GetComponent<TMPro.TextMeshProUGUI>().text = "";
            for (int _i = 0; _i < GameManager.PARTY.Count; _i++) this.transform.Find("WhoShopsMenuPanel").GetChild(_i).GetComponent<TMPro.TextMeshProUGUI>().text =
                    GameManager.PARTY[_i].name + " (" + GameManager.PARTY[_i].alignment.ToString().Substring(0, 1) + "-" + GameManager.PARTY[_i].job.ToString().Substring(0, 3) + ") has " + GameManager.PARTY[_i].gold + "gp";
        }
    }

    public void SelectPartyMember(int n)
    {
        if(this.transform.Find("WhoShopsMenuPanel").GetChild(n).GetComponent<TMPro.TextMeshProUGUI>().text != "")
        {
            _selected_Character = n;
            WhoShopsPanel.SetActive(false);
            ServicesPanel.SetActive(true);
            MessagePanel.SetActive(false);
        }
    }

    public void SelectServiceLine(int n)
    {
        bool _stayedinroom = false;

        WhoShopsPanel.SetActive(false);
        ServicesPanel.SetActive(false);
        MessagePanel.SetActive(true);
        this.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";


        //0 = stables, heals 0, costs 0;
        if (n == 0)
        {            
            MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[_selected_Character].name + " stays a single night in the stable, curled up in the hay.\n\n";
            _stayedinroom = true;
        }
        //1 = common room, heals 1, costs 10
        if (n == 1)
        {
            if (GameManager.PARTY[_selected_Character].gold >= 10)
            {
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[_selected_Character].name + " stays 7 nights on a cot in the common room.\n\n";
                GameManager.PARTY[_selected_Character].gold -= 10;
                GameManager.PARTY[_selected_Character].weeksOld++;
                GameManager.PARTY[_selected_Character].hp += 7;
                if (GameManager.PARTY[_selected_Character].hp > GameManager.PARTY[_selected_Character].maxHP) GameManager.PARTY[_selected_Character].hp = GameManager.PARTY[_selected_Character].maxHP;
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += GameManager.PARTY[_selected_Character].name + "'s health is now " + GameManager.PARTY[_selected_Character].hp + "|" + GameManager.PARTY[_selected_Character].maxHP + "\n\n";
                _stayedinroom = true;
            }
            else
            {
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[_selected_Character].name + " cannot afford to stay on a cot in the common room.\nPerhaps another option would be more affordable?";
                _stayedinroom = false;
            }
        }
        //2 = small room, heals 3, costs 50
        if (n == 2)
        {
            if (GameManager.PARTY[_selected_Character].gold >= 50)
            {
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[_selected_Character].name + " stays 7 nights in small private room.\n\n";
                GameManager.PARTY[_selected_Character].gold -= 50;
                GameManager.PARTY[_selected_Character].weeksOld++;
                GameManager.PARTY[_selected_Character].hp += 21;
                if (GameManager.PARTY[_selected_Character].hp > GameManager.PARTY[_selected_Character].maxHP) GameManager.PARTY[_selected_Character].hp = GameManager.PARTY[_selected_Character].maxHP;
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += GameManager.PARTY[_selected_Character].name + "'s health is now " + GameManager.PARTY[_selected_Character].hp + "|" + GameManager.PARTY[_selected_Character].maxHP + "\n\n";
                _stayedinroom = true;
            }
            else
            {
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[_selected_Character].name + " cannot afford to stay in a small private room.\nPerhaps another option would be more affordable?";
                _stayedinroom = false;
            }
        }
        //3 = merchant suite = 7, costs 200
        if (n == 3)
        {
            if (GameManager.PARTY[_selected_Character].gold >= 200)
            {
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[_selected_Character].name + " stays 7 nights in a simple but comfortable suite of rooms.\n\n";
                GameManager.PARTY[_selected_Character].gold -= 200;
                GameManager.PARTY[_selected_Character].weeksOld++;
                GameManager.PARTY[_selected_Character].hp += 49;
                if (GameManager.PARTY[_selected_Character].hp > GameManager.PARTY[_selected_Character].maxHP) GameManager.PARTY[_selected_Character].hp = GameManager.PARTY[_selected_Character].maxHP;
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += GameManager.PARTY[_selected_Character].name + "'s health is now " + GameManager.PARTY[_selected_Character].hp + "|" + GameManager.PARTY[_selected_Character].maxHP + "\n\n";
                _stayedinroom = true;
            }
            else
            {
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[_selected_Character].name + " cannot afford to stay in a suite.\nPerhaps another option would be more affordable?";
                _stayedinroom = false;
            }
        }
        //4 = royal suite = 10, costs 500
        if (n == 4)
        {
            if (GameManager.PARTY[_selected_Character].gold >= 500)
            {
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[_selected_Character].name + " stays 7 nights on a lavishly decorated suite of rooms.\n\n";
                GameManager.PARTY[_selected_Character].gold -= 500;
                GameManager.PARTY[_selected_Character].weeksOld++;
                GameManager.PARTY[_selected_Character].hp += 70;
                if (GameManager.PARTY[_selected_Character].hp > GameManager.PARTY[_selected_Character].maxHP) GameManager.PARTY[_selected_Character].hp = GameManager.PARTY[_selected_Character].maxHP;
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += GameManager.PARTY[_selected_Character].name + "'s health is now " + GameManager.PARTY[_selected_Character].hp + "|" + GameManager.PARTY[_selected_Character].maxHP + "\n\n";
                _stayedinroom = true;
            }
            else
            {
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[_selected_Character].name + " cannot afford to stay in such opulence.\nPerhaps another option would be more suited to your station?";
                _stayedinroom = false;
            }
        }

        if (_stayedinroom)
        {
            bool _tryLevelup = GameManager.PARTY[_selected_Character].LevelUp();

            if (!_tryLevelup) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += GameManager.PARTY[_selected_Character].name + " needs " + GameManager.PARTY[_selected_Character].xpNeededForLevelUp + " more experience to level up.";
            if (_tryLevelup)
            {
                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += GameManager.PARTY[_selected_Character].name + " gains a level!";

                if (GameManager.PARTY[_selected_Character].strengthChange < 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nLoses Strength.";
                if (GameManager.PARTY[_selected_Character].strengthChange > 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nGains Strength.";
                if (GameManager.PARTY[_selected_Character].iqChange < 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nLoses IQ.";
                if (GameManager.PARTY[_selected_Character].iqChange > 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nGains IQ.";
                if (GameManager.PARTY[_selected_Character].pietyChange < 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nLoses Piety.";
                if (GameManager.PARTY[_selected_Character].pietyChange > 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nGains Piety.";
                if (GameManager.PARTY[_selected_Character].agilityChange < 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nLoses Agility.";
                if (GameManager.PARTY[_selected_Character].agilityChange > 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nGains Agility.";
                if (GameManager.PARTY[_selected_Character].vitalityChange < 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nLoses Vitality.";
                if (GameManager.PARTY[_selected_Character].vitalityChange > 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nGains Vitality.";
                if (GameManager.PARTY[_selected_Character].luckChange < 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nLoses Luck.";
                if (GameManager.PARTY[_selected_Character].luckChange > 0) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\nGains Luck.";

                MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\n" + "Health goes up by " + GameManager.PARTY[_selected_Character].healthChange + " health.";

                if (GameManager.PARTY[_selected_Character].newSpells) MessagePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += "\n" + "Learns new spells.";

                partyPanel.GetComponent<PartyPanelController>().UpdatePartyPanel();
                SaveLoadModule.SaveGame();
            }
        }
    }
}
