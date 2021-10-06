using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Interface;

public class StubInputManager : IInputManager
{
    public string CallList { get; private set; }

    public bool ReturnIsMoveDown;
    public bool ReturnIsMoveLeft;
    public bool ReturnIsMoveRight;
    public bool ReturnIsRotateLeft;
    public bool ReturnIsRotateRight;

    public bool IsMoveDown()
    {
        CallList += "IsMoveDown\n"; 
        return ReturnIsMoveDown; 
    }

    public bool IsMoveLeft()
    {
        CallList += "IsMoveLeft\n";
        return ReturnIsMoveLeft;
    }

    public bool IsMoveRight()
    {
        CallList += "IsMoveRight\n";
        return ReturnIsMoveRight;
    }

    public bool IsRotateLeft()
    {
        CallList += "IsRotateLeft\n";
        return ReturnIsRotateLeft;
    }

    public bool IsRotateRight()
    {
        CallList += "IsRotateRight\n";
        return ReturnIsRotateRight;
    }
}
