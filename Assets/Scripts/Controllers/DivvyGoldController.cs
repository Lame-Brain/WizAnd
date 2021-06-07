using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivvyGoldController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI[] textLine;

    public void DoIt()
    {
        int _totalGP = 0;
        int _num = GameManager.PARTY.Count;
        for (int _i = 0; _i < _num; _i++) _totalGP += GameManager.PARTY[_i].gold;
        Debug.Log("Total GP " + _totalGP);
        int _bulkSplit = 0;
        if (_num > 0) _bulkSplit = (int)_totalGP / _num;
        Debug.Log("Bulk Split " + _bulkSplit);
        int _remainder = 0;
        if (_num > 0) _remainder = _totalGP % _num;
        Debug.Log("remainder " + _remainder);
        for (int _i = 0; _i < _num; _i++)
        {
            int _s = 0;
            if(_remainder > 0) { _s = 1; _remainder--; }
            GameManager.PARTY[_i].gold = _bulkSplit + _s;
            textLine[_i].text = GameManager.PARTY[_i].name + " received " + (_bulkSplit + _s) + " gp.";
            Debug.Log(_i + ") " + (_bulkSplit + _s));
        }
    }
}
