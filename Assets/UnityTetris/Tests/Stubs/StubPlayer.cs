using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris;
using UnityTetris.Interface;

public class StubPlayer : IPlayer
{
    public string CallList { get; private set; }
    public bool RetIsAlive;

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

    public void Setup(BlockSet[] blockSetOptions, ISoundManager sound)
    {
        CallList += "Setup\n";
    }

    public void StartGame(StateGameMain parent)
    {
        CallList += "StartGame\n";
    }
}
