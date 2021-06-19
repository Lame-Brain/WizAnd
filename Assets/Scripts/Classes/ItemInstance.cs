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

    public ItemInstance(ItemInstance _i)
    {
        refID = _i.refID;
        name = _i.name;
        identified = _i.identified;
        curseActive = _i.curseActive;
    }
}
