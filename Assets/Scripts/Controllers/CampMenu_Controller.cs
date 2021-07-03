using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampMenu_Controller : MonoBehaviour
{
    public TMPro.TextMeshProUGUI PartyMember1_slot, PartyMember2_slot, PartyMember3_slot, PartyMember4_slot, PartyMember5_slot, PartyMember6_slot;
    public GameObject CharacterSheet_PF, Canvas_Ref;

    private void Start()
    {
        UpdatePartyList();
    }

    public void UpdatePartyList()
    {
        PartyMember1_slot.text = "Empty Slot";
        PartyMember2_slot.text = "Empty Slot";
        PartyMember3_slot.text = "Empty Slot";
        PartyMember4_slot.text = "Empty Slot";
        PartyMember5_slot.text = "Empty Slot";
        PartyMember6_slot.text = "Empty Slot";
        if (GameManager.PARTY.Count > 0) PartyMember1_slot.text = GameManager.ROSTER[GameManager.PARTY[0]].name;
        if (GameManager.PARTY.Count > 1) PartyMember2_slot.text = GameManager.ROSTER[GameManager.PARTY[1]].name;
        if (GameManager.PARTY.Count > 2) PartyMember3_slot.text = GameManager.ROSTER[GameManager.PARTY[2]].name;
        if (GameManager.PARTY.Count > 3) PartyMember4_slot.text = GameManager.ROSTER[GameManager.PARTY[3]].name;
        if (GameManager.PARTY.Count > 4) PartyMember5_slot.text = GameManager.ROSTER[GameManager.PARTY[4]].name;
        if (GameManager.PARTY.Count > 5) PartyMember6_slot.text = GameManager.ROSTER[GameManager.PARTY[5]].name;
    }

    public void MoveSlotUp(int _index)
    {
        if (_index < GameManager.PARTY.Count)
        {
            int _tempValueAtIndex = GameManager.PARTY[_index];
            int _storeValue = GameManager.PARTY[_index - 1];
            GameManager.PARTY[_index - 1] = _tempValueAtIndex;
            GameManager.PARTY[_index] = _storeValue;
            UpdatePartyList();
        }
    }

    public void MoveSlotDown(int _index)
    {
        if(_index < GameManager.PARTY.Count - 1)
        {
            int _tempValueAtIndex = GameManager.PARTY[_index];
            int _storeValue = GameManager.PARTY[_index + 1];
            GameManager.PARTY[_index + 1] = _tempValueAtIndex;
            GameManager.PARTY[_index] = _storeValue;
            UpdatePartyList();
        }
    }

    public void InspectCharacter(int _selected)
    {
        if(_selected < GameManager.PARTY.Count)
        {
            GameObject _go = Instantiate(CharacterSheet_PF, Canvas_Ref.transform);
            _go.GetComponent<CharacterScreenController>().selected_character = GameManager.PARTY[_selected];
            _go.GetComponent<CharacterScreenController>().UpdateCharacterScreen();
        }

    }

    public void QuitAndSave()
    {

    }
}
