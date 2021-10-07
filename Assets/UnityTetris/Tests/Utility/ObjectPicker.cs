using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPicker<T> where T : UnityEngine.Object
{
    private T[] _before; 

    public ObjectPicker()
    {
        _before = new T[0]; 
    }

    public T[] Pick()
    {

        T[] after = GameObject.FindObjectsOfType<T>();
        var diff = after.Where(s => !_before.Contains(s)).ToArray();
        _before = after;
        return diff; 
    }
}
