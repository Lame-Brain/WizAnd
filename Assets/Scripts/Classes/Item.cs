using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item 
{     
    //Enums
    public enum Protection { None, Animal, Mythical, Giant, Undead, Demon, Dragon, Insect, Were, Fighter, Thief, Mage, Priest }
    public enum Type { Misc, Sword, Axe, Mace, Flail, Dagger, Staff, Key, Rod, Helm, Armor, Shield, Gloves, Ring, Amulet, Scroll, Potion, Figurine }
    public enum Alignment { Any, Good, Neutral, Evil }

    //ID
    public int ID;

    //Description
    public string itemName;
    public Type itemType;
    public string itemLore;

    //Shop Stuff
    public int shopStock;
    public int price;

    //Specials
    public Protection protection;
    public bool cursed;
    public string invoke;
    public string cast;
    public int heal;
    public Alignment alignment;

    //Class restrictions
    public string allowEquip;

    //Armor
    public int acBonus;

    //Weapons
    public int weaponAttackModifier;
    public int weaponDamageMin;
    public int weaponDamageMax;
    public int weaponSwingCount;
    public string[] weaponAttack;

    //Constructor for weapons
    public Item(int _n, string _name, Type _type, string _lore, int _stock, int _price, Protection _prot, bool _curse, string _invoke, string _cast, int _heal, Alignment _alig, string _restrict, int _ac, int _attk, int _min, int _max, int _swing)
    {
        ID = _n;
        itemName = _name;
        itemType = _type;
        itemLore = _lore;
        shopStock = _stock;
        price = _price;
        protection = _prot;
        cursed = _curse;
        invoke = _invoke;
        cast = _cast;
        heal = _heal;
        allowEquip = _restrict;
        alignment = _alig;
        acBonus = _ac;
        weaponAttackModifier = _attk;
        weaponDamageMin = _min;
        weaponDamageMax = _max;
        if(itemType == Type.Sword) weaponAttack = new string[4] { "slashes", "cuts", "stabs", "slices"};
        if(itemType == Type.Axe) weaponAttack = new string[3] { "chops", "slams", "cleaves" };
        if(itemType == Type.Dagger) weaponAttack = new string[3] { "stabs", "pierces", "shanks" };
        if(itemType == Type.Staff || itemType == Type.Mace || itemType == Type.Flail) weaponAttack = new string[6] { "bashes", "clobbers", "crushes", "smashes", "slams", "crunches into" };
        if (itemType == Type.Rod) weaponAttack = new string[1] { "bonks" };
    }

    //Constructor for Armor
    public Item(int _n, string _name, Type _type, string _lore, int _stock, int _price, Protection _prot, bool _curse, string _invoke, string _cast, int _heal, Alignment _alig, string _restrict, int _ac)
    {
        ID = _n;
        itemName = _name;
        itemType = _type;
        itemLore = _lore;
        shopStock = _stock;
        price = _price;
        protection = _prot;
        cursed = _curse;
        invoke = _invoke;
        cast = _cast;
        heal = _heal;
        allowEquip = _restrict;
        alignment = _alig;
        acBonus = _ac;
        weaponAttackModifier = 0;
        weaponDamageMin = 0;
        weaponDamageMax = 0;
        weaponAttack = new string[1] { "" };
    }

    //Constructor for Other
    public Item(int _n, string _name, Type _type, string _lore, int _stock, int _price, Protection _prot, bool _curse, string _invoke, string _cast, int _heal)
    {
        ID = _n;
        itemName = _name;
        itemType = _type;
        itemLore = _lore;
        shopStock = _stock;
        price = _price;
        protection = _prot;
        cursed = _curse;
        invoke = _invoke;
        cast = _cast;
        heal = _heal;
        alignment = Alignment.Any;
        allowEquip = "FMPTBSLN";
        acBonus = 0;
        weaponAttackModifier = 0;
        weaponDamageMin = 0;
        weaponDamageMax = 0;
        weaponAttack = new string[1] { "" };
    }
}
