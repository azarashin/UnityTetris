using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris;
using UnityTetris.Abstract;

public class StubStateGameMain : AbstractStateGameMain
{
    public string CallList { get; private set; } = "";

    public void ClearCallList()
    {
        CallList = "";
    }

    public override void PlayerGameOver(Player player)
    {
        CallList += $"PlayerGameOver({player.name})";
    }

    public override void Setup(GameController parent, Player[] players)
    {
        CallList += $"Setup()";
    }
}
