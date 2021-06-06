//https://strategywiki.org/wiki/Wizardry:_Proving_Grounds_of_the_Mad_Overlord/Table_of_Contents
//https://datadrivengamer.blogspot.com/2019/08/the-bestiary-of-wizardry.html?showComment=1622503904421#c5782529334212861682
//https://datadrivengamer.blogspot.com/2019/08/the-not-so-basic-mechanics-of-wizardry.html?m=1
//http://www.zimlab.com/wizardry/walk/w123calc.htm
//http://www.wizardryarchives.com/downloads/archivesmanual.pdf
//https://lparchive.org/Wizardry-Proving-Grounds-of-the-Mad-Overlord/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager GAME;
    public static ListManager LISTS;

    public static List<PlayerCharacter> ROSTER = new List<PlayerCharacter>();
    public static List<PlayerCharacter> PARTY = new List<PlayerCharacter>();

    void Awake()
    {
        GAME = this;
        LISTS = this.GetComponent<ListManager>();
        SaveLoadModule.LoadGame();

        for (int _index = 0; _index < ROSTER.Count; _index++) Debug.Log(_index + ". " + ROSTER[_index].name);
    }
    // Start is called before the first frame update
    void Start()
    {
//        int total = 148 + 123 + 98 + 60 + 7 + 150;
//        int bulk = (int)total / 6;
//        int remainder = total % 6;
//        Debug.Log(total + " divided among 6: " + bulk + " with " + remainder + " left over...");
//        for (int _i = 0; _i < 6; _i++)
//        {
//            int amnt = 0;
//            if (remainder > 0) amnt = 1;
//            remainder--;
//            Debug.Log("PC " + _i + "gets " + (bulk + amnt) + " gp");
//        }
        
        //        List<string> list1 = new List<string>();
        //        List<string> list2 = new List<string>();
        //        list1.Add("Stuff 1"); list1.Add("Stuff A"); list1.Add("Stuff Alpha");
        //list2.Add("Floof"); list2.Add("Fluff");
        //list2 = list1;        
        //        for (int d = 0; d < list1.Count; d++) list2.Add(list1[d]);
        //        list2.RemoveAt(0);
        //        for (int d = 0; d < list1.Count; d++) Debug.Log("List 1: " + list1[d]);
        //        for (int d = 0; d < list2.Count; d++) Debug.Log("List 2: " + list2[d]);
        //        Debug.Log(list1[0] == list2[0]);

        //        string _test = "FAsT";
        //        Debug.Log("The word is: " + _test);
        //        Debug.Log("Do it have an S? " + _test.Contains("S"));
        //        Debug.Log("Do it have an s? " + _test.Contains("s"));
        //        Debug.Log("Do it have an F? " + _test.Contains("F"));
        //        Debug.Log("Do it have an f? " + _test.Contains("f"));
        //        Debug.Log(LISTS.monsterList[0].names + " do this damage = " + LISTS.monsterList[0].attackMin[0] + " to " + LISTS.monsterList[0].attackMax[0] + " + " + LISTS.monsterList[0].attackBonus[0]);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
