using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Interface;

public class StubStatusPanel : IStatusPanel
{
    public int ReturnScore;
    public int ReturnLevel;
    public int ReturnNext;
    public int ReturnNextMax;

    public string CallList { get; private set; } = "";

    public void ClearCallList()
    {
        CallList = "";
    }


    public int Score()
    {
        return ReturnScore; 
    }

    public int Level()
    {
        return ReturnLevel;
    }

    public (int, int) Next(int id)
    {
        return (ReturnNext, ReturnNextMax);
    }

    public void AddScore(int numberOfLines)
    {
        CallList += $"AddScore({numberOfLines})\n";
    }

    public void ResetScore()
    {
        CallList += $"ResetScore\n";
    }

    public void UpdateReservation(List<int> reservation)
    {
        CallList += $"UpdateReservation\n";
    }

    public void UpdateLevel(int level)
    {
        CallList += $"UpdateLevel({level})\n";
    }

}