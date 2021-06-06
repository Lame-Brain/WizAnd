﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMemberToPartyController : MonoBehaviour
{
    public GameObject RosterPanel, PartyPanel, CharacterLine_PF;
    public List<PlayerCharacter> DisplayRoster = new List<PlayerCharacter>();
    public List<PlayerCharacter> DisplayParty = new List<PlayerCharacter>();

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
            if (!GameManager.ROSTER[_i].plyze && 
                !GameManager.ROSTER[_i].dead && 
                !GameManager.ROSTER[_i].ashes && 
                !GameManager.ROSTER[_i].stoned && 
                !GameManager.ROSTER[_i].lost) DisplayRoster.Add(GameManager.ROSTER[_i]);
    }
    public void UpdateScreen()
    {
        //Clear any children
        if (RosterPanel.transform.childCount > 0) foreach (Transform child in RosterPanel.transform) Destroy(child.gameObject);
        if (PartyPanel.transform.childCount > 0) foreach (Transform child in PartyPanel.transform) Destroy(child.gameObject);

        for (int _i = 0; _i < DisplayRoster.Count; _i++)
        {
            if(!DisplayParty.Contains(DisplayRoster[_i]))
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

}
