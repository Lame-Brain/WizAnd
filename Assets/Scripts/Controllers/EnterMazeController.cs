using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterMazeController : MonoBehaviour
{
    public void EnterTheMaze()
    {
        if(GameManager.PARTY.Count > 0)
            GameManager.GAME.EnterMazeFromTown();
    }

    public void ExitGame()
    {

    }
}
