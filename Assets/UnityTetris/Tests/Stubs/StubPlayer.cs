using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris;
using UnityTetris.Abstract;
using UnityTetris.Interface;

public class StubPlayer : IPlayer
{
    public string CallList { get; private set; } = ""; 
    public bool RetIsAlive;

    public void ClearCallList()
    {
        CallList = "";
    }

    public void Dead()
    {
        CallList += "Dead\n";
    }

    public bool IsAlive()
    {
        CallList += "IsAlive\n";
        return RetIsAlive; 
    }

    public void PullNextBlock()
    {
        CallList += "PullNextBlock\n";
    }

    public void Setup(AbstractField fieldPrefab, AbstractBlockSet[] blockSetPrefabOptions, ISoundManager sound, LevelController levelController)
    {
        CallList += "Setup\n";
    }

    public void StartGame(AbstractStateGameMain parent)
    {
        CallList += "StartGame\n";
    }

    public void BlockSetHasBeenPlaced()
    {
        CallList += "BlockSetHasBeenPlaced\n";
    }
}
