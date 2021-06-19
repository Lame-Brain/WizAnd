using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerCharacter
{
    //Enums
    public enum Race { other, Human, Elf, Dwarf, Gnome, Hobbit }
    public enum Class { Fighter, Thief, Mage, Priest, Lord, Samurai, Ninja, Bishop }
    public enum Alignment { Good, Neutral, Evil }

    //Location
    public int dungeon_X, dungeon_Y, dungeon_Level;

    //descriptors
    public string name;
    public Race race;
    public Class job;
    public Alignment alignment;
    public int weeksOld; //52 weeks in a year

    //status
    public bool poisoned;
    public bool afraid;
    public bool asleep;
    public bool plyze;
    public bool stoned;
    public bool dead;
    public bool ashes;
    public bool lost;

    //awards
    public bool killedWerdna;
    public bool blessedByGnilda;
    public bool knighted;
    public bool starOfLlylgamyn;

    //stats
    public int strength;
    public int iq;
    public int piety;
    public int vitality;
    public int agility;
    public int luck;
    public int gold;
    public int ep;
    public int level;
    public int maxLevel;
    public int ac;
    public float hp;
    public float maxHP;

    // equipment
    public ItemInstance head;
    public ItemInstance body;
    public ItemInstance shield;
    public ItemInstance weapon;
    public ItemInstance jewelry;
    public ItemInstance[] bag = new ItemInstance[10];

    //spells
    public int[] mageSlots = new int[7];
    public int[] priestSlots = new int[7];
    public List<string> mageSpells = new List<string>();
    public List<string> priestSpells = new List<string>();

    //saving throws 
    public float saveVsDeath; //Saves vs poison, paralysis, and critical hits
    public float saveVsPetrify; //saves vs petrify in combat
    public float saveVsWand; //saves vs traps
    public float saveVsBreath;// saves vs breath attacks. succesful saves cuts damage in half
    public float saveVsSpell;// saves vs Montino spell effects in combat    

    //info for level up screen
    public bool newSpells;
    public int strengthChange;
    public int iqChange;
    public int pietyChange;
    public int vitalityChange;
    public int agilityChange;
    public int luckChange;
    public int healthChange;
    public int xpNeededForLevelUp;


    public void CalculateSpells()
    {
        System.Random _random = new System.Random();
        
        //calculate mage spell slots
        int _a = 0; int _b = 0;
        if (job == Class.Mage) { _a = 0; _b = 2; }
        if (job == Class.Bishop) { _a = 0; _b = 4; }
        if (job == Class.Samurai) { _a = 3; _b = 3; }
        for(int _mSplLvl = 0; _mSplLvl < 7; _mSplLvl++) mageSlots[_mSplLvl] = level - _a + _b - (_b * _mSplLvl);

        //calculate priest spell slots
        if (job == Class.Priest) { _a = 0; _b = 2; }
        if (job == Class.Bishop) { _a = 3; _b = 4; }
        if (job == Class.Lord) { _a = 3; _b = 2; }
        for (int _pSplLvl = 0; _pSplLvl < 7; _pSplLvl++) priestSlots[_pSplLvl] = level - _a + _b - (_b * _pSplLvl);
               
        //---SPELLS----
        //you always get the first spell in a level if you have slot
        if (mageSlots[0] > 0 && mageSpells.Count == 0) mageSpells.Add("Fire Bolt");
        if (mageSlots[1] > 0 && !mageSpells.Contains("Mage Armor")) mageSpells.Add("Mage Armor");
        if (mageSlots[2] > 0 && !mageSpells.Contains("Mage Armor")) mageSpells.Add("Mage Armor");
        if (mageSlots[3] > 0 && !mageSpells.Contains("Heat Wave")) mageSpells.Add("Heat Wave");
        if (mageSlots[4] > 0 && !mageSpells.Contains("Fear")) mageSpells.Add("Fear");
        if (mageSlots[5] > 0 && !mageSpells.Contains("Blizzard")) mageSpells.Add("Blizzard");
        if (mageSlots[6] > 0 && !mageSpells.Contains("Teleport")) mageSpells.Add("Teleport");

        if (priestSlots[0] > 0 && priestSpells.Count == 0) priestSpells.Add("Heal");
        if (priestSlots[1] > 0 && !priestSpells.Contains("Hold Monsters")) priestSpells.Add("Hold Monsters");
        if (priestSlots[2] > 0 && !priestSpells.Contains("Prayer")) priestSpells.Add("Prayer");
        if (priestSlots[3] > 0 && !priestSpells.Contains("Greater Heal")) priestSpells.Add("Greater Heal");
        if (priestSlots[4] > 0 && !priestSpells.Contains("Flame Tower")) priestSpells.Add("Flame Tower");
        if (priestSlots[5] > 0 && !priestSpells.Contains("Spirit Recall")) priestSpells.Add("Spirit Recall");
        if (priestSlots[6] > 0 && !priestSpells.Contains("Resurrection")) priestSpells.Add("Resurrectionr");

        //check for learned spells. chance to learn spells: check if random(0,50) is less than IQ or Piety
        foreach ( Spell sp in GameManager.LISTS.spellList) //cycle all spells
        {
            if(sp.type == Spell.Type.mage) //is it a mage spell?
            {                
                if(mageSlots[(sp.spellLevel - 1)] > 0 && !mageSpells.Contains(sp.spellTitle)) //is there a slot for this level of spell? AND is the spell not already learned?
                {
                    if(_random.Next(0,50) < iq) //Is the random roll less than IQ?
                    {
                        mageSpells.Add(sp.spellTitle); // add the spell
                        mageSlots[(sp.spellLevel - 1)]++; //add a spell slot
                    }
                }
            }

            if (sp.type == Spell.Type.priest) //is it a priest spell?
            {
                if (priestSlots[(sp.spellLevel - 1)] > 0 && !priestSpells.Contains(sp.spellTitle)) //is there a slot for this level of spell? AND is the spell not already learned?
                {
                    if (_random.Next(0, 50) < piety) //Is the random roll less than Piety?
                    {
                        priestSpells.Add(sp.spellTitle); // add the spell
                        priestSlots[(sp.spellLevel - 1)]++; //add a spell slot
                    }
                }
            }
        }

        //clamp spell slots to 9 for each spell level
        for (int _i = 1; _i < 7; _i++)
        {
            if (mageSlots[_i] > 9) mageSlots[_i] = 9;
            if (priestSlots[_i] > 9) priestSlots[_i] = 9;
        }
    }

    public void CalculateSaves()
    {
        int _deathBonus = 0; int _petrifyBonus = 0; int _wandBonus = 0; int _breathBonus = 0; int _spellBonus = 0;
        if (race == Race.Human) _deathBonus += 1;
        if (race == Race.Elf) _spellBonus += 2;
        if (race == Race.Dwarf) _breathBonus += 4;
        if (race == Race.Gnome) _petrifyBonus += 2;
        if (race == Race.Hobbit) { _deathBonus++; _petrifyBonus++; _wandBonus++; _breathBonus++; _spellBonus++; }
        if (job == Class.Fighter) { _deathBonus += 3; }
        if (job == Class.Thief) { _breathBonus += 3; }
        if (job == Class.Priest) { _petrifyBonus += 3; }
        if (job == Class.Mage) { _spellBonus += 3; }
        if (job == Class.Lord) { _deathBonus += 2; _petrifyBonus += 2; }
        if (job == Class.Ninja) { _deathBonus += 3; _breathBonus += 3; _petrifyBonus += 2; _wandBonus += 4; }
        if (job == Class.Bishop) { _petrifyBonus += 2; _wandBonus += 2; _spellBonus += 2; }
        if (job == Class.Samurai) { _deathBonus += 2; _spellBonus += 2; }

        saveVsBreath = ((level / 5) + (luck / 5) + _deathBonus) * 7;
        saveVsPetrify = ((level / 5) + (luck / 5) + _petrifyBonus) * 7;
        saveVsWand = ((level / 5) + (luck / 5) + _wandBonus) * 7;
        saveVsBreath = ((level / 5) + (luck / 5) + _breathBonus) * 7;
        saveVsSpell = ((level / 5) + (luck / 5) + _spellBonus) * 7;
    }

    public int StatChange(int s)
    {
        int _roll1 = GameManager.GAME.RandINT(1, 101);
        int _roll2 = GameManager.GAME.RandINT(1, 131);
        GameManager.GAME.DebugLog("random roll 1 = " + _roll1 + "   Target is 75");
        GameManager.GAME.DebugLog("random roll 2 = " + _roll2 + "   Target is " + (int)weeksOld / 52);
        if (_roll1 < 75)
        {
            if (_roll2 < (weeksOld / 52))  //chance to decrease stat
            {
                s--;
                if (s == 17 && GameManager.GAME.RandINT(1, 7) < 5) s++; //if stat is 18 and decreases, there is a 5/6 chance that it stays at 18
            }
            else //if the stat did not decrease, then it increases by 1;
            {
                s++;
                GameManager.GAME.DebugLog("STAT INCREASE");
            }
        }

        if (s > 18) s = 18; //clamp stats at 18

        return s;
    }

    public bool LevelUp()
    {
        System.Random _random = new System.Random();

        bool _leveledUp = false; xpNeededForLevelUp = 0;

        //Set E.P. Targets
        int _a = 0; int _b = 0; int _c = 0; int _d = 0; int _e = 0; int _f = 0; int _g = 0; int _h = 0; int _i = 0; int _j = 0; int _k = 0; int _l = 0; int _m = 0;
        if(job == Class.Fighter) { _a = 1000; _b = 1724; _c = 2972; _d = 5124; _e = 8834; _f = 15231; _g = 26260; _h = 45275; _i = 78060; _j = 134586; _k = 232044; _l = 400075; _m = 289709; }
        if(job == Class.Mage)    { _a = 1100; _b = 1896; _c = 3268; _d = 5634; _e = 9713; _f = 16746; _g = 28872; _h = 49779; _i = 85825; _j = 147974; _k = 255127; _l = 439874; _m = 318529; }
        if(job == Class.Priest)  { _a = 1050; _b = 1810; _c = 3120; _d = 5379; _e = 9274; _f = 15989; _g = 27567; _h = 47529; _i = 81946; _j = 141286; _k = 243596; _l = 419993; _m = 304132; }
        if(job == Class.Thief)   { _a = 900;  _b = 1551; _c = 2674; _d = 4610; _e = 7948; _f = 13703; _g = 23625; _h = 40732; _i = 70187; _j = 121081; _k = 208750; _l = 359931; _m = 260639; }
        if(job == Class.Bishop)  { _a = 1000; _b = 2105; _c = 3692; _d = 6477; _e = 11363; _f = 19935; _g = 34973; _h = 61136; _i = 107642; _j = 188845; _k = 331370; _l = 581240; _m = 438479; }
        if(job == Class.Samurai) { _a = 1250; _b = 2192; _c = 3845; _d = 6745; _e = 11833; _f = 20759; _g = 36419; _h = 63892; _i = 112091; _j = 196650; _k = 345000; _l = 605263; _m = 456601; }
        if (job == Class.Lord)   { _a = 1300; _b = 2280; _c = 4000; _d = 7017; _e = 12310; _f = 21596; _g = 37887; _h = 66468; _i = 116610; _j = 204578; _k = 358908; _l = 629663; _m = 475008; }
        if (job == Class.Ninja)  { _a = 1450; _b = 2543; _c = 4461; _d = 7826; _e = 13729; _f = 24085; _g = 42254; _h = 74129; _i = 130050; _j = 228157; _k = 400275; _l = 702236; _m = 529756; }
        int[] _xpNNL = new int[] { 0, _a, _b, _c, _d, _e, _f, _g, _h, _i, _j, _k, _l, _m };

        //determine if levelUp has occured.
        if(level < 13 && ep > _xpNNL[level]) _leveledUp = true;
        if (level > 12 && ep > (_xpNNL[12] + (_xpNNL[13] * (level - 12)))) _leveledUp = true;
        if (!_leveledUp) xpNeededForLevelUp = _xpNNL[level] - ep;

        if (_leveledUp)
        {
            //increase level
            level++;

            //stat changes
            int _newStrength = StatChange(strength);
            int _newIQ = StatChange(iq);
            int _newPiety = StatChange(piety);
            int _newAgility = StatChange(agility);
            int _newVitality = StatChange(vitality);
            int _newLuck = StatChange(luck);

            //set variable for display on level up screen
            strengthChange = _newStrength - strength;
            iqChange = _newIQ - iq;
            pietyChange = _newPiety - piety;
            agilityChange = _newAgility - agility;
            vitalityChange = _newVitality - vitality;
            luckChange = _newLuck - luck;

/*//DEBUG LINE
            GameManager.GAME.DebugLog("old strength " + strength);
            GameManager.GAME.DebugLog("new strength " + _newStrength);
            GameManager.GAME.DebugLog("Strength change " + strengthChange);
            GameManager.GAME.DebugLog("old iq " + iq);
            GameManager.GAME.DebugLog("new iq " + _newIQ);
            GameManager.GAME.DebugLog("iq change " + iqChange);
            GameManager.GAME.DebugLog("old piety " + piety);
            GameManager.GAME.DebugLog("new piety " + _newPiety);
            GameManager.GAME.DebugLog("piety change " + pietyChange);
            GameManager.GAME.DebugLog("old agility " + agility);
            GameManager.GAME.DebugLog("new agility " + _newAgility);
            GameManager.GAME.DebugLog("agility change " + agilityChange);
            GameManager.GAME.DebugLog("old vitality " + vitality);
            GameManager.GAME.DebugLog("new vitality " + _newVitality);
            GameManager.GAME.DebugLog("vitality change " + vitalityChange);
            GameManager.GAME.DebugLog("old luck " + luck);
            GameManager.GAME.DebugLog("new luck " + _newLuck);
            GameManager.GAME.DebugLog("luck change " + luckChange); 
*/

            //apply stat changes
            strength = _newStrength;
            iq = _newIQ;
            piety = _newPiety;
            agility = _newAgility;
            vitality = _newVitality;
            luck = _newLuck;

            //reroll health
            int _newMaxHP = 0; int _vitalityBonus = 0;

            if (vitality < 6) _vitalityBonus = -1;
            if (vitality < 4) _vitalityBonus = -2;
            if (vitality == 16) _vitalityBonus = 1;
            if (vitality == 17) _vitalityBonus = 2;
            if (vitality == 18) _vitalityBonus = 3;

            if (job == Class.Samurai) _newMaxHP = _random.Next(1, 8) + _vitalityBonus; //samurai get an extra hit dice
            for(int _lvl = 0; _lvl < level; _lvl++)
            {
                if (job == Class.Fighter) _newMaxHP += _random.Next(1, 10) + _vitalityBonus;
                if (job == Class.Thief) _newMaxHP += _random.Next(1, 6) + _vitalityBonus;
                if (job == Class.Priest) _newMaxHP += _random.Next(1, 8) + _vitalityBonus;
                if (job == Class.Mage) _newMaxHP += _random.Next(1, 4) + _vitalityBonus;
                if (job == Class.Lord) _newMaxHP += _random.Next(1, 10) + _vitalityBonus;
                if (job == Class.Samurai) _newMaxHP += _random.Next(1, 8) + _vitalityBonus;
                if (job == Class.Ninja) _newMaxHP += _random.Next(1, 6) + _vitalityBonus;
                if (job == Class.Bishop) _newMaxHP += _random.Next(1, 6) + _vitalityBonus;
            }
            if(_newMaxHP <= maxHP) _newMaxHP = (int)maxHP + 1; //clamp newMaxHp to always be at least 1 more than old maxHP
            healthChange = _newMaxHP - (int)maxHP; //set variable for display on level up screen
            maxHP = _newMaxHP; //set new max hp
            hp += healthChange;
            if (hp > maxHP) hp = maxHP;

            //calculate SpellSlots and Spells
            CalculateSpells();
        }

        return _leveledUp;
    }
}
