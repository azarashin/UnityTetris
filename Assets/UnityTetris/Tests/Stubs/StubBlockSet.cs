using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;
using UnityTetris.Interface;

public class StubBlockSet : AbstractBlockSet
{
    public string CallList { get; private set; } = "";

    public int ReturnRotStep;
    public Vector2Int ReturnCenterPos;

    public void ClearCallList()
    {
        CallList = "";
    }


    public override void Setup(IPlayer owner, AbstractField field, IInputManager input, ISoundManager sound, int fallLevel)
    {
        CallList += $"Setup({fallLevel})\n";
    }

    public override int RotStep()
    {
        CallList += $"RotStep\n";
        return ReturnRotStep; 
    }

    public override Vector2Int CenterPos()
    {
        CallList += $"CenterPos\n";
        return ReturnCenterPos; 
    }
}
