using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyPanelController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI NameColumn, ClassColumn, AC_Column, HitsColumn, StatusColumn;

    private void Start()
    {
        UpdatePartyPanel();
    }

    public void UpdatePartyPanel()
    {
        NameColumn.text = "CHARACTER NAME"; ClassColumn.text = "CLASS"; AC_Column.text = "A.C."; HitsColumn.text = "HITs"; StatusColumn.text = "STATUS";
        for(int _i = 0; _i < GameManager.PARTY.Count; _i++)
        {
            NameColumn.text += "\n" + GameManager.ROSTER[GameManager.PARTY[_i]].name;
            ClassColumn.text += "\n" + GameManager.ROSTER[GameManager.PARTY[_i]].alignment.ToString()[0] + "-" + GameManager.ROSTER[GameManager.PARTY[_i]].job.ToString().Substring(0, 3);
            AC_Column.text += "\n" + GameManager.ROSTER[GameManager.PARTY[_i]].ac.ToString();
            HitsColumn.text += "\n" + GameManager.ROSTER[GameManager.PARTY[_i]].hp.ToString();

            string _status = GameManager.ROSTER[GameManager.PARTY[_i]].maxHP.ToString();
            if (GameManager.ROSTER[GameManager.PARTY[_i]].plyze) _status = "Paralyzed";
            if (GameManager.ROSTER[GameManager.PARTY[_i]].poisoned) _status = "Poisoned";
            if (GameManager.ROSTER[GameManager.PARTY[_i]].stoned) _status = "Stone";
            if (GameManager.ROSTER[GameManager.PARTY[_i]].dead) _status = "Dead";
            if (GameManager.ROSTER[GameManager.PARTY[_i]].ashes) _status = "Ashes";
            StatusColumn.text += "\n" + _status;
        }

    }
}
