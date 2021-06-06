using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MonsterStack
{
    public enum Class { None, Animal, Mythical, Giant, Undead, Demon, Dragon, Insect, Were, Fighter, Thief, Mage, Priest }

    public int ID;
    public string unknownName, unknownNames, name, names;
    public int minGroupSize, maxGroupSize;
    public int hpDice, hpSides, hpBonus;
    public int iconIndex;
    public Class monsterClass;
    public int ac;
    public List<int> attackDice = new List<int>();
    public List<int> attackSides = new List<int>();
    public List<int> attackBonus = new List<int>();

    //Specials
    public int breath;
    public int drain;
    public int heal;

    //reward tables
    public int reward1, reward2;

    //
    public int mageLevel, priestLevel, spellResist;

    //Elemental Resitances
    public bool physical;
    public bool arcane;
    public bool fire;
    public bool cold;
    public bool lightning;

    //Abilities
    public bool Sleep;
    public bool Run;
    public bool Critical;
    public bool Paralyze;
    public bool Poison;
    public bool Call;
    public bool Stone;

    //E.P. Award per monster in stack
    public int epAward;

    //Buddy
    public int buddyIndex;
    public string buddyName;
    public int buddyOdds;


    public MonsterStack(int _id, string _un, string _uns, string _nam, string _nams, int _minGrp, int _maxGrp, int _hpDice, int _hpSides, int _hpBonus, Class _class, int _AC, int _breath, int _drain, int _heal, int _ep, int _reward1, int _reward2, string _resist, string _abil, int _mageLvl, int _prstLvl, int _spellResist)
    {
        ID = _id; unknownName = _un; unknownNames = _uns; name = _nam; names = _nams;
        minGroupSize = _minGrp; maxGroupSize = _maxGrp;
        hpDice = _hpDice; hpSides = _hpSides; hpBonus = _hpBonus;
        monsterClass = _class;
        ac = _AC;
        breath = _breath; drain = _drain; heal = _heal;
        epAward = _ep;
        reward1 = _reward1;
        reward2 = _reward2;

        physical = false; arcane = false; fire = false; cold = false; lightning = false;
        if (_resist.Contains("physical")) physical = true;
        if (_resist.Contains("arcane")) arcane = true;
        if (_resist.Contains("fire")) fire = true;
        if (_resist.Contains("cold")) cold = true;
        if (_resist.Contains("lightning")) lightning = true;

        Sleep = false; Run = false; Critical = false; Paralyze = false; Poison = false; Call = false; Stone = false;
        if (_abil.Contains("sleep")) Sleep = true;
        if (_abil.Contains("run")) Run = true;
        if (_abil.Contains("crit")) Critical = true;
        if (_abil.Contains("para")) Paralyze = true;
        if (_abil.Contains("pois")) Poison = true;
        if (_abil.Contains("call")) Call = true;
        if (_abil.Contains("stone")) Stone = true;

        mageLevel = _mageLvl;
        priestLevel = _prstLvl;
        spellResist = _spellResist;
    }

    public void SetAttacks(int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i += 3)
        {
            attackDice.Add(numbers[i]);
            attackSides.Add(numbers[i+1]);
            attackBonus.Add(numbers[i+2]);
        }
    }

    public void SetBuddy(int _index, string _name, int _chance)
    {
        buddyIndex = _index;
        buddyName = _name;
        buddyOdds = _chance;
    }
}
