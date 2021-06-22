using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMemberToPartyController : MonoBehaviour
{
    public GameObject RosterPanel, PartyPanel, CharacterLine_PF;
    public List<int> DisplayRoster = new List<int>();
    public List<int> DisplayParty = new List<int>();

    //This is for Tavern Inspect
    public TMPro.TextMeshProUGUI[] TavernInspectPanelLineItem;
    public GameObject CharacterSheet_PF, Canvas_ref;
    private GameObject _characterSheet;


    private GameObject _go;
    private int _selected_Character;
    private string _focusWindow = "none";

    public void InitModule()
    {
        BuildRosterLists();
        UpdateScreen();
    }

    public void BuildRosterLists()
    {
        DisplayParty.Clear();
        for (int _i = 0; _i < 6; _i++)
        {
            DisplayParty.Add(-1);
            if (GameManager.PARTY.Count > _i) DisplayParty[_i] = GameManager.PARTY[_i];
        }

        DisplayRoster.Clear();
        for (int _i = 0; _i < GameManager.ROSTER.Count; _i++)
        {
/*             if (!GameManager.ROSTER[_i].plyze &&
                 !GameManager.ROSTER[_i].dead &&
                 !GameManager.ROSTER[_i].ashes &&
                 !GameManager.ROSTER[_i].stoned &&
                 !GameManager.ROSTER[_i].lost &&
                 !DisplayParty.Contains(_i)) */
                 DisplayRoster.Add(_i);
        }
    }
    public void UpdateScreen()
    {
        //Clear any children
        if (RosterPanel.transform.childCount > 0) foreach (Transform child in RosterPanel.transform) Destroy(child.gameObject);
        if (PartyPanel.transform.childCount > 0) foreach (Transform child in PartyPanel.transform) Destroy(child.gameObject);

        for (int _i = 0; _i < GameManager.ROSTER.Count; _i++)
        {
            _go = Instantiate(CharacterLine_PF, RosterPanel.transform);
            _go.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.ROSTER[DisplayRoster[_i]].name + " LVL" + GameManager.ROSTER[DisplayRoster[_i]].level + " " + GameManager.ROSTER[DisplayRoster[_i]].alignment.ToString() + " " + GameManager.ROSTER[DisplayRoster[_i]].job.ToString();
            if (GameManager.ROSTER[_i].plyze ||
                GameManager.ROSTER[_i].dead ||
                GameManager.ROSTER[_i].ashes ||
                GameManager.ROSTER[_i].stoned ||
                GameManager.ROSTER[_i].lost ||
                DisplayParty.Contains(_i)) _go.SetActive(false);

        }
        for (int _i = 0; _i < 6; _i++)
        {
            _go = Instantiate(CharacterLine_PF, PartyPanel.transform);
            _go.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            if(DisplayParty[_i] > -1) _go.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.ROSTER[DisplayRoster[_i]].name + " LVL" + GameManager.ROSTER[DisplayRoster[_i]].level + " " + GameManager.ROSTER[DisplayRoster[_i]].alignment.ToString() + " " + GameManager.ROSTER[DisplayRoster[_i]].job.ToString();
        }
    }

    public void AddOrRemoveFromParty()
    {
        if(_focusWindow == "RosterPanel")
        {
            int _temp = -1;
            for (int _i = 0; _i < 6; _i++) if (_temp == -1 && DisplayParty[_i] == -1) _temp = _i;
            if (_temp > -1) DisplayParty[_temp] = _selected_Character;
            UpdateScreen();
        }

        if (_focusWindow == "PartyPanel")
        {
            //DisplayRoster.Add(_selected_Character);
            //DisplayParty.Remove(_selected_Character);
            //UpdateScreen();
        }

        _focusWindow = "none";
        UpdateScreen();
    }

    public void CharacterLineClicked(int n, string fw)
    {
        //Debug.Log("I clicked in the " + fw + " window, on: line #" + n);

        _selected_Character = n;
        _focusWindow = fw;
        AddOrRemoveFromParty();
    }

    public void FinalizeParty()
    {
        GameManager.PARTY.Clear(); //Clear the main Party list in order to re-create it.

        for (int _p = 0; _p < 6; _p++) //Cycle through the temporary party list that was just created.
        {
            if(DisplayParty[_p] > -1) GameManager.PARTY.Add(DisplayParty[_p]);
        }
    }

    //this is for the tavern inspect panel
    public void InitializeTavernInspectPanel()
    {
        for(int _i = 0; _i < GameManager.PARTY.Count; _i++)
        {
            string _line_item = GameManager.ROSTER[GameManager.PARTY[_i]].name + " ";
            if (GameManager.ROSTER[GameManager.PARTY[_i]].alignment == PlayerCharacter.Alignment.Good) _line_item += "G-";
            if (GameManager.ROSTER[GameManager.PARTY[_i]].alignment == PlayerCharacter.Alignment.Neutral) _line_item += "N-";
            if (GameManager.ROSTER[GameManager.PARTY[_i]].alignment == PlayerCharacter.Alignment.Evil) _line_item += "E-";
            _line_item += GameManager.ROSTER[GameManager.PARTY[_i]].job.ToString().Substring(0, 3) + " LVL" + GameManager.ROSTER[GameManager.PARTY[_i]].level.ToString();
            TavernInspectPanelLineItem[_i].text = _line_item;
        }
    }

    public void Inspect_THIS_Character(int _selected)
    {
        int _selectedFromRoster = GameManager.PARTY[_selected];
        
        _characterSheet = Instantiate(CharacterSheet_PF, Canvas_ref.transform);
        _characterSheet.GetComponent<CharacterScreenController>().selected_character = _selectedFromRoster;
        _characterSheet.GetComponent<CharacterScreenController>().UpdateCharacterScreen();
    }
}
