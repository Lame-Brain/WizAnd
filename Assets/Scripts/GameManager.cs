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
    public static List<int> PARTY = new List<int>();

    void Awake()
    {
        GAME = this;
        LISTS = this.GetComponent<ListManager>();
        LISTS.LoadLists();

        DontDestroyOnLoad(this.gameObject);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Town");

        SaveLoadModule.LoadGame();

        //DEBUG PARTY
        //for(int p = 0; p < 6; p++) PARTY.Add(p);

        //UnityEngine.SceneManagement.SceneManager.LoadScene("Level 1");
    }


    void Start()
    {
    }

    void Update()
    {

    }

    public void DebugLog(string s)
    {
        Debug.Log(s);
    }

    public int RandINT(int min, int max)
    {
        return Random.Range(min, max);
    }

    public void UpdatePartyPanel()
    {
        GameObject _go = GameObject.FindGameObjectWithTag("PartyPanel");
        if (_go != null)
        {
            _go.GetComponent<PartyPanelController>().UpdatePartyPanel();
            Debug.Log("Updating Party Panel");
        }
        if (_go == null) Debug.Log("Unable to update Party Panel");
    }

    public void EnterMazeFromTown()
    {
        // 1. mark party members as OUT
        for (int _i = 0; _i < GameManager.PARTY.Count; _i++) GameManager.ROSTER[GameManager.PARTY[_i]].lost = true;

        // 2. transition to level 1
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 1");

        // 3. Set party location to appropriate spot
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        GameObject _fromAbove = null;
        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("Teleport")) if (_go.name == "FromAbove") { _player.transform.position = _go.transform.position; _player.transform.rotation = _go.transform.rotation; }
    }
}
