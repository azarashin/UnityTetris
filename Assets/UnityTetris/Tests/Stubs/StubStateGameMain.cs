using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris;
using UnityTetris.Abstract;
using UnityTetris.Interface;

public class StubStateGameMain : AbstractStateGameMain
{
    public string CallList { get; private set; } = "";

    public void ClearCallList()
    {
        CallList = "";
    }

    public override void PlayerGameOver(IPlayer player)
    {
        CallList += $"PlayerGameOver({player.GetHashCode()})\n";
    }

    public override void Setup(IGameController parent, IPlayer[] players)
    {
        CallList += $"Setup()\n";
    }
}
