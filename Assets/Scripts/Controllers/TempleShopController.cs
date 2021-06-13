using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleShopController : MonoBehaviour
{
    public GameObject whoShops_pnl, whoShopsContent_pnl, services_pnl, servicesContent_pnl, TempleMessage_pnl, TempleConfirm_pnl, shopLineItem_PF;

    private int _selectedCharacter, _index;
    private float _randomRoll;
    private PlayerCharacter _tempToon;
    private List<GameObject> TempleServiceLineItem = new List<GameObject>();
    private List<int> _paitent = new List<int>();
    private List<string> _service = new List<string>();
    private List<int> _cost = new List<int>();
    private GameObject _go;

    public void BuildPartyList()
    {
        whoShopsContent_pnl.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "";
        whoShopsContent_pnl.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "";
        whoShopsContent_pnl.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = "";
        whoShopsContent_pnl.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = "";
        whoShopsContent_pnl.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = "";
        whoShopsContent_pnl.transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = "";

        if (GameManager.PARTY.Count > 0) whoShopsContent_pnl.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[0].name + " (" + GameManager.PARTY[0].alignment.ToString().Substring(0, 1) + "-" + GameManager.PARTY[0].job.ToString().Substring(0, 3) + ") has " + GameManager.PARTY[0].gold + "gp";
        if (GameManager.PARTY.Count > 1) whoShopsContent_pnl.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[1].name + " (" + GameManager.PARTY[1].alignment.ToString().Substring(0, 1) + "-" + GameManager.PARTY[1].job.ToString().Substring(0, 3) + ") has " + GameManager.PARTY[1].gold + "gp";
        if (GameManager.PARTY.Count > 2) whoShopsContent_pnl.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[2].name + " (" + GameManager.PARTY[2].alignment.ToString().Substring(0, 1) + "-" + GameManager.PARTY[2].job.ToString().Substring(0, 3) + ") has " + GameManager.PARTY[2].gold + "gp";
        if (GameManager.PARTY.Count > 3) whoShopsContent_pnl.transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[3].name + " (" + GameManager.PARTY[3].alignment.ToString().Substring(0, 1) + "-" + GameManager.PARTY[3].job.ToString().Substring(0, 3) + ") has " + GameManager.PARTY[3].gold + "gp";
        if (GameManager.PARTY.Count > 4) whoShopsContent_pnl.transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[4].name + " (" + GameManager.PARTY[4].alignment.ToString().Substring(0, 1) + "-" + GameManager.PARTY[4].job.ToString().Substring(0, 3) + ") has " + GameManager.PARTY[4].gold + "gp";
        if (GameManager.PARTY.Count > 5) whoShopsContent_pnl.transform.GetChild(5).GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.PARTY[5].name + " (" + GameManager.PARTY[5].alignment.ToString().Substring(0, 1) + "-" + GameManager.PARTY[5].job.ToString().Substring(0, 3) + ") has " + GameManager.PARTY[5].gold + "gp";
    }

    public void PayTithe()
    {
        GameManager.PARTY[_selectedCharacter].gold -= ((int)GameManager.PARTY[_selectedCharacter].gold / 10);
        TempleMessage_pnl.SetActive(true);
        //UpdateSave();
        TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text = "Bless you child, for your donation! May AEOS' favor walk with you always.";
    }

    public void Resurrect(int n)
    {
        TempleMessage_pnl.SetActive(true);
        if (GameManager.PARTY[_selectedCharacter].gold < _cost[n]) //FAIL, not enough gp
        {
            TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text = "Alas child, you are unable to donate sufficent funds to honor AEOS for this boon. Return when you have more coin.";
        }
        if (GameManager.PARTY[_selectedCharacter].gold < _cost[n]) //Have enough money, take a chance
        {
            TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text = "CHANT! MURMER! INVOKE!\n\n";
            _randomRoll = Random.Range(1f, 100f);
            if (_randomRoll <= (_tempToon.vitality * 3) + 40) //Succeed check
            {
                TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text += "Rejoice! " + _tempToon.name + " has been restored to life!";
                GameManager.ROSTER[n].hp = 1; //restore health to 1
                GameManager.ROSTER[n].ashes = false; GameManager.ROSTER[n].dead = false; //restore to life
                GameManager.ROSTER[n].weeksOld += Random.Range(1, 53); //Age between 1 and 52 weeks
                GameManager.PARTY[_selectedCharacter].gold -= _cost[n];
                UpdateSave();
            }
            else //Fail Check
            {
                TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text += "Curses! " + _tempToon.name + " has been reduced to ashes!";
                GameManager.ROSTER[n].ashes = true;
                GameManager.ROSTER[n].weeksOld += Random.Range(1, 53); //Age between 1 and 52 weeks
                GameManager.PARTY[_selectedCharacter].gold -= _cost[n];
                UpdateSave();
            }
        }
    }

    public void ResFromAsh(int n)
    {
        TempleMessage_pnl.SetActive(true);
        if (GameManager.PARTY[_selectedCharacter].gold < _cost[n]) //FAIL, not enough gp
        {
            TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text = "Alas child, you are unable to donate sufficent funds to honor AEOS for this boon. Return when you have more coin.";
        }
        if (GameManager.PARTY[_selectedCharacter].gold < _cost[n]) //Have enough money, take a chance
        {
            TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text = "CHANT! MURMER! INVOKE!\n\n";
            _randomRoll = Random.Range(1f, 100f);            
            if(_randomRoll <= (_tempToon.vitality * 3) + 40) //Succeed check
            {
                TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text += "Rejoice! " + _tempToon.name + " has been restored from Ashes!";
                GameManager.ROSTER[n].hp = GameManager.ROSTER[n].maxHP; //restore health
                GameManager.ROSTER[n].ashes = false; GameManager.ROSTER[n].dead = false; //restore to life
                GameManager.ROSTER[n].weeksOld += Random.Range(1, 53); //Age between 1 and 52 weeks
                GameManager.PARTY[_selectedCharacter].gold -= _cost[n];
                UpdateSave();
            }
            else //Fail Check
            {
                TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text += "Curses! " + _tempToon.name + " has been utterly destroyed!";
                GameManager.ROSTER.Remove(_tempToon);
                GameManager.ROSTER[n].weeksOld += Random.Range(1, 53); //Age between 1 and 52 weeks
                GameManager.PARTY[_selectedCharacter].gold -= _cost[n];
                UpdateSave();
            }
        }
    }

    public void Restore(int n)
    {
        TempleMessage_pnl.SetActive(true);
        if (GameManager.PARTY[_selectedCharacter].gold < _cost[n]) //FAIL, not enough gp
        {
            TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text = "Alas child, you are unable to donate sufficent funds to honor AEOS for this boon. Return when you have more coin.";
        }
        else
        { 
            TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text = "CHANT! MURMER! INVOKE!\n\n";
            TempleMessage_pnl.transform.Find("Message Text").GetComponent<TMPro.TextMeshProUGUI>().text += _tempToon.name + " has been restored!";
            GameManager.ROSTER[n].plyze = false;
            GameManager.ROSTER[n].stoned = false;
            GameManager.PARTY[_selectedCharacter].gold -= _cost[n];
            UpdateSave();
        }
    }

    public void BuildServiceList()
    {
        //init the variables and lists and such
        _paitent.Clear(); _service.Clear(); _cost.Clear();
        for (int _i = 0; _i < GameManager.ROSTER.Count; _i++) if (GameManager.ROSTER[_i] == GameManager.PARTY[_selectedCharacter]) _paitent.Add(_i);
        _service.Add("Tithe"); _cost.Add((int)GameManager.PARTY[_selectedCharacter].gold / 10);        

        //build the lists
        for (int _i = 0; _i < GameManager.ROSTER.Count; _i++) if (GameManager.ROSTER[_i].ashes) { _paitent.Add(_i); _service.Add("Restore from Ash"); _cost.Add(500 * GameManager.ROSTER[_i].level); }
        for (int _i = 0; _i < GameManager.ROSTER.Count; _i++) if (GameManager.ROSTER[_i].dead && !GameManager.ROSTER[_i].ashes) { _paitent.Add(_i); _service.Add("Resurrect"); _cost.Add(250 * GameManager.ROSTER[_i].level); }

        for (int _i = 0; _i < GameManager.ROSTER.Count; _i++) if (GameManager.ROSTER[_i].stoned) { _paitent.Add(_i); _service.Add("Restore"); _cost.Add(200 * GameManager.ROSTER[_i].level); }
        for (int _i = 0; _i < GameManager.ROSTER.Count; _i++) if (GameManager.ROSTER[_i].plyze && !GameManager.ROSTER[_i].stoned) { _paitent.Add(_i); _service.Add("Restore"); _cost.Add(100 * GameManager.ROSTER[_i].level); }

        //clear services panel content
        if (servicesContent_pnl.transform.childCount > 0) foreach (Transform child in servicesContent_pnl.transform) Destroy(child.gameObject);

        for(int _i = 0; _i < _paitent.Count; _i++)
        {
            _go = Instantiate(shopLineItem_PF, servicesContent_pnl.transform);
            _go.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>().text = _service[_i] + " (" + GameManager.ROSTER[_paitent[_i]] + ")";
            _go.transform.Find("ItemPrice").GetComponent<TMPro.TextMeshProUGUI>().text = ((int)GameManager.PARTY[_selectedCharacter].gold / 10) + "gp";
        }
    }

    public void ChooseServiceLine(int n)
    {
        _tempToon = GameManager.ROSTER[n];
        TempleConfirm_pnl.SetActive(true);
        TempleConfirm_pnl.GetComponent<TMPro.TextMeshProUGUI>().text = "You wish to perform the service of " + _service[n] + " on " + GameManager.ROSTER[_paitent[n]] + " for " + _cost + "gp?";
        _index = n;
    }
    
    public void YES()
    {
        int n = _index;
        if (_service[n] == "Tithe") PayTithe();
        if (_service[n] == "Restore from Ash") ResFromAsh(n);
        if (_service[n] == "Resurrect") Resurrect(n);
        if (_service[n] == "Restore") Restore(n);
        services_pnl.SetActive(false);
        whoShops_pnl.SetActive(true);
    }

    public void CharacterSelected(int n)
    {
        _selectedCharacter = n;
        whoShops_pnl.SetActive(false);
        services_pnl.SetActive(true);
        BuildServiceList();
    }

    public void UpdateSave()
    {
        SaveLoadModule.SaveGame();
    }
}
