using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Spell
{
    public enum Source { none, divine, arcane, fire, cold, lightning }
    public enum Type { priest, mage }
    public enum Target { self, friend, enemy, group, foes, party }
    public string spellTitle;
    public string spellWord;
    public Source source;
    public Type type;
    public Target target;
    public int spellLevel;
    public string spellDescription;
    public bool combatSpell;
    public bool exploreSpell;

    public Spell (string title, string word, Source s, Type t, Target tar, int level, string desc, bool combat, bool explore)
    {
        spellTitle = title;
        spellWord = word;
        source = s;
        type = t;
        target = tar;
        spellLevel = level;
        spellDescription = desc;
        combatSpell = combat;
        exploreSpell = explore;
    }
}
