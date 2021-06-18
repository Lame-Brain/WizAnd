using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMemberToPartyController : MonoBehaviour
{
    public GameObject RosterPanel, PartyPanel, CharacterLine_PF;
    public List<PlayerCharacter> DisplayRoster = new List<PlayerCharacter>();
    public List<PlayerCharacter> DisplayParty = new List<PlayerCharacter>();

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
        for (int _i = 0; _i < GameManager.PARTY.Count; _i++) DisplayParty.Add(GameManager.PARTY[_i]);

        DisplayRoster.Clear();
        for (int _i = 0; _i < GameManager.ROSTER.Count; _i++)
        {
             if (!GameManager.ROSTER[_i].plyze &&
                 !GameManager.ROSTER[_i].dead &&
                 !GameManager.ROSTER[_i].ashes &&
                 !GameManager.ROSTER[_i].stoned &&
                 !GameManager.ROSTER[_i].lost &&
                 !DisplayParty.Contains(GameManager.ROSTER[_i]))
                 DisplayRoster.Add(GameManager.ROSTER[_i]);
        }
    }
    public void UpdateScreen()
    {
        //Clear any children
        if (RosterPanel.transform.childCount > 0) foreach (Transform child in RosterPanel.transform) Destroy(child.gameObject);
        if (PartyPanel.transform.childCount > 0) foreach (Transform child in PartyPanel.transform) Destroy(child.gameObject);

        for (int _i = 0; _i < DisplayRoster.Count; _i++)
        {
            _go = Instantiate(CharacterLine_PF, RosterPanel.transform);
            _go.GetComponent<TMPro.TextMeshProUGUI>().text = DisplayRoster[_i].name + " LVL" + DisplayRoster[_i].level + " " + DisplayRoster[_i].alignment.ToString() + " " + DisplayRoster[_i].job.ToString();
        }
        for (int _i = 0; _i < DisplayParty.Count; _i++)
        {
                _go = Instantiate(CharacterLine_PF, PartyPanel.transform);
                _go.GetComponent<TMPro.TextMeshProUGUI>().text = DisplayParty[_i].name + " LVL" + DisplayParty[_i].level + " " + DisplayParty[_i].alignment.ToString() + " " + DisplayParty[_i].job.ToString();
        }
    }

    public void AddOrRemoveFromParty()
    {
        if(_focusWindow == "RosterPanel")
        {
            if (DisplayParty.Count < 6)
            {
                DisplayParty.Add(DisplayRoster[_selected_Character]);
                DisplayRoster.RemoveAt(_selected_Character);
            }
        }

        if (_focusWindow == "PartyPanel")
        {
            DisplayRoster.Add(DisplayParty[_selected_Character]);
            DisplayParty.RemoveAt(_selected_Character);
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

        for (int _p = 0; _p < DisplayParty.Count; _p++) //Cycle through the temporary party list that was just created.
        {
            for (int _r = 0; _r < GameManager.ROSTER.Count; _r++) //Cycle through the Character Roster...
            {
                if (DisplayParty[_p] == GameManager.ROSTER[_r]) //..And look for same character...
                {
                    GameManager.PARTY.Add(DisplayParty[_p]); //...to add them to the party list
                }
            }
        }
    }

    //this is for the tavern inspect panel
    public void InitializeTavernInspectPanel()
    {
        for(int _i = 0; _i < GameManager.PARTY.Count; _i++)
        {
            string _line_item = GameManager.PARTY[_i].name + " ";
            if (GameManager.PARTY[_i].alignment == PlayerCharacter.Alignment.Good) _line_item += "G-";
            if (GameManager.PARTY[_i].alignment == PlayerCharacter.Alignment.Neutral) _line_item += "N-";
            if (GameManager.PARTY[_i].alignment == PlayerCharacter.Alignment.Evil) _line_item += "E-";
            _line_item += GameManager.PARTY[_i].job.ToString().Substring(0, 3) + " LVL" + GameManager.PARTY[_i].level.ToString();
            TavernInspectPanelLineItem[_i].text = _line_item;
        }
    }

    public void Inspect_THIS_Character(int _selected)
    {
        int _selectedFromRoster = 0;
        if (_selected < GameManager.PARTY.Count)
        {            
            for (int _i = 0; _i < GameManager.ROSTER.Count; _i++)
                if (GameManager.PARTY[_selected] == GameManager.ROSTER[_i])
                    _selectedFromRoster = _i;
            _characterSheet = Instantiate(CharacterSheet_PF, Canvas_ref.transform);
            _characterSheet.GetComponent<CharacterScreenController>().selected_character = _selectedFromRoster;
            _characterSheet.GetComponent<CharacterScreenController>().UpdateCharacterScreen();
        }
    }
}
