using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRosterInspectScreenController : MonoBehaviour
{
    public GameObject scrollviewPanel, characterLine_pf, InspectMenuPanel, CharacterScreenPanel;    

    private GameObject _go;    
    public int _selected_Character;

    private void Start() 
    {
        ///DEBUG -- REMOVE
        //InitRosterInspectWindow();
    }

    public void InitRosterInspectWindow(string list2Show)
    {
        //Clear any children
        if (scrollviewPanel.transform.childCount > 0) foreach (Transform child in scrollviewPanel.transform) Destroy(child.gameObject);

        //make new children
        if (list2Show == "Roster")
        {
            for (int _i = 0; _i < GameManager.ROSTER.Count; _i++)
            {
                _go = Instantiate(characterLine_pf, scrollviewPanel.transform);
                _go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GameManager.ROSTER[_i].name + " LVL" + GameManager.ROSTER[_i].level + " " + GameManager.ROSTER[_i].alignment.ToString() + " " + GameManager.ROSTER[_i].job.ToString();
            }
        }
        if (list2Show == "Party")
        {
            for (int _i = 0; _i < GameManager.PARTY.Count; _i++)
            {
                _go = Instantiate(characterLine_pf, scrollviewPanel.transform);
                _go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = GameManager.ROSTER[GameManager.PARTY[_i]].name + " LVL" + GameManager.ROSTER[GameManager.PARTY[_i]].level + " " + GameManager.ROSTER[GameManager.PARTY[_i]].alignment.ToString() + " " + GameManager.ROSTER[GameManager.PARTY[_i]].job.ToString();
            }
        }
    }

    public void CharacterLineClicked(int n)
    {
        _selected_Character = n;
        InspectMenuPanel.SetActive(true);
        InspectMenuPanel.transform.Find("Menu Title").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.ROSTER[n].name + " LVL" + GameManager.ROSTER[n].level + " " + GameManager.ROSTER[n].alignment.ToString() + " " + GameManager.ROSTER[n].job.ToString();
    }

    public void InspectCharacter()
    {
        _go = Instantiate(CharacterScreenPanel, transform.parent);
        _go.SetActive(true);
//        _go.GetComponent<CharacterScreenController>().InitCharacterScreen();
        _go.GetComponent<CharacterScreenController>().selected_character = _selected_Character;
        _go.GetComponent<CharacterScreenController>().UpdateCharacterScreen();
    }

    public void DeleteCharacter()
    {
        Destroy(scrollviewPanel.transform.GetChild(_selected_Character).gameObject);
        GameManager.ROSTER.RemoveAt(_selected_Character);
        SaveLoadModule.SaveGame();
    }
}
