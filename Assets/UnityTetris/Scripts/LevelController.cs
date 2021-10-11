using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController
{
    public const int MinimumFallLevel = 2;
    public const int NumberOfPhasesToChangeFallLevel = 20;

    private int _fallLevel;
    private int _initialFallLevel;
    private int _step; 

    public LevelController(int fallLevel)
    {
        _initialFallLevel = _fallLevel = fallLevel;
        _step = 0; 
    }

    public int CurrentDisplayLevel()
    {
        return _initialFallLevel - _fallLevel + 1;
    }

    public int CurrentFallLevel()
    {
        return _fallLevel; 
    }

    public void NextBlockHasBeenPulled()
    {
        if(_fallLevel <= MinimumFallLevel)
        {
            return; 
        }
        _step++; 
        if(_step >= NumberOfPhasesToChangeFallLevel)
        {
            _step = 0;
            _fallLevel--; 
        }
    }

}
