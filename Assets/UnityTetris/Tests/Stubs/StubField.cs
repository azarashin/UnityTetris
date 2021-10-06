using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris;
using UnityTetris.Interface;

public class StubField : IField
{
    public string CallList { get; private set; }

    public bool ReturnIsHit;
    public Transform ReturnRefTransform;
    public bool ReturnSetBlocks;
    public int ReturnWidth; 

    public bool IsHit(Vector2Int[] blocks)
    {
        CallList += "IsHit\n"; 
        return ReturnIsHit; 
    }

    public Transform RefTransform()
    {
        CallList += "RefTransform\n";
        return ReturnRefTransform; 
    }

    public void ResetField(int width, int height, int borderLine)
    {
        CallList += "ResetField\n";
    }

    public bool SetBlocks(Block[] blocks)
    {
        CallList += "SetBlocks\n";
        return ReturnSetBlocks; 
    }

    public int Width()
    {
        CallList += "Width\n";
        return ReturnWidth; 
    }
}
