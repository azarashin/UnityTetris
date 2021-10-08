using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Interface;

public class StubGameController : IGameController
{
    public string CallList { get; private set; } = "";

    public void ClearCallList()
    {
        CallList = "";
    }

    public void BootGame()
    {
        CallList += "BootGame\n"; 
    }

    public void FinishGame()
    {
        CallList += "FinishGame\n";
    }

    public void RunGame()
    {
        CallList += "RunGame\n";
    }

}
