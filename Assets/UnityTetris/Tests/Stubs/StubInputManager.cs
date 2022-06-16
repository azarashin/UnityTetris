using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTetris.Abstract;
using UnityTetris.Interface;

public class StubInputManager : IInputManager
{
    public string CallList { get; private set; } = "";

    private bool _returnIsMoveDown;
    private bool _returnIsMoveLeft;
    private bool _returnIsMoveRight;
    private bool _returnIsRotateLeft;
    private bool _returnIsRotateRight;

    public void ClearCallList()
    {
        CallList = "";
    }

    public void SetReturn(bool moveDown, bool moveLeft, bool moveRight, bool rotateLeft, bool rotateRight)
    {
        _returnIsMoveDown = moveDown;
        _returnIsMoveLeft = moveLeft;
        _returnIsMoveRight = moveRight;
        _returnIsRotateLeft = rotateLeft;
        _returnIsRotateRight = rotateRight; 
    }

    public void StartGame()
    {
        CallList += "StartGame\n";
    }

    public void UpdateState()
    {
        CallList += "UpdateState\n";
    }

    public void FinishGame()
    {
        CallList += "FinishGame\n";
    }

    public bool IsMoveDown()
    {
        CallList += "IsMoveDown\n"; 
        return _returnIsMoveDown; 
    }

    public bool IsMoveLeft()
    {
        CallList += "IsMoveLeft\n";
        return _returnIsMoveLeft;
    }

    public bool IsMoveRight()
    {
        CallList += "IsMoveRight\n";
        return _returnIsMoveRight;
    }

    public bool IsRotateLeft()
    {
        CallList += "IsRotateLeft\n";
        return _returnIsRotateLeft;
    }

    public bool IsRotateRight()
    {
        CallList += "IsRotateRight\n";
        return _returnIsRotateRight;
    }

}
