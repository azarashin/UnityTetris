using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void ClearCallList()
    {
        CallList = ""; 
    }

    public bool IsHit(Vector2Int[] blocks)
    {
        // 例：
        // blocks = new Vector2Int[] {new Vector2Int(1,2), new Vector2Int(3,4)}; の時、CallListに追加される文字列：
        // "IsHit((1,2),(3,4))\n"
        string parameter = string.Join(",", blocks.Select(s => $"({s.x},{s.y})"));
        CallList += $"IsHit({parameter})\n"; 
        return ReturnIsHit; 
    }

    public Transform RefTransform()
    {
        CallList += "RefTransform\n";
        return ReturnRefTransform; 
    }

    public void ResetField(int width, int height, int borderLine)
    {
        // 例：
        // width=4, height=5, borderLine=2 の時、CallListに追加される文字列：
        // "ResetField(4,5,2)\n"
        CallList += $"ResetField({width},{height},{borderLine})\n";
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
