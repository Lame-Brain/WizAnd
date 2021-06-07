using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ItemInstance
{
    public int refID;
    public string name;
    public bool identified;
    public bool curseActive;

    public ItemInstance(Item _i)
    {
        refID = _i.ID;
        name = _i.itemName;
        identified = false;
        curseActive = false;
    }
}
