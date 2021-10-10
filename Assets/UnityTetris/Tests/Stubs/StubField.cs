using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTetris;
using UnityTetris.Abstract;
using UnityTetris.Interface;

public class StubField : AbstractField
{
    public string CallList { get; private set; } = "";

    public bool ReturnIsHit;
    public bool ReturnSetBlocks;
    public int ReturnWidth;
    public int ReturnHeight;

    public void ClearCallList()
    {
        CallList = ""; 
    }

    public override bool IsHit(Vector2Int[] blocks)
    {
        // 例：
        // blocks = new Vector2Int[] {new Vector2Int(1,2), new Vector2Int(3,4)}; の時、CallListに追加される文字列：
        // "IsHit((1,2),(3,4))\n"
        string parameter = string.Join(",", blocks.Select(s => $"({s.x},{s.y})"));
        CallList += $"IsHit({parameter})\n"; 
        return ReturnIsHit; 
    }

    public override void ResetField(ISoundManager sound, int width, int height, int borderLine)
    {
        // 例：
        // width=4, height=5, borderLine=7 の時、CallListに追加される文字列：
        // "ResetField(4,5,7)\n"
        CallList += $"ResetField({width},{height},{borderLine})\n";
    }

    public override bool SetBlocks(Block[] blocks)
    {
        string parameter = string.Join(",", blocks.Select(s => $"({s.Px},{s.Py})"));
        CallList += $"SetBlocks({parameter})\n";
        return ReturnSetBlocks; 
    }

    public override int Width()
    {
        CallList += "Width\n";
        return ReturnWidth; 
    }

    public override int Height()
    {
        CallList += "Height\n";
        return ReturnHeight;
    }

    public override void ReduceLines(IPlayer owner)
    {
        CallList += $"ReduceLines({owner.GetHashCode()})\n";
    }
}
