using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Interface;

public class StubStatusPanel : IStatusPanel
{
    public string CallList { get; private set; } = "";

    public void ClearCallList()
    {
        CallList = "";
    }

    public void AddScore(int numberOfLines)
    {
        CallList += $"AddScore({numberOfLines})\n";
    }

    public void ResetScore()
    {
        CallList += $"ResetScore\n";
    }
}