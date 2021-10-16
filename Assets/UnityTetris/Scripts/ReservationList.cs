using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReservationList : MonoBehaviour
{
    [SerializeField]
    GameObject[] _items;

    private int _id; 

    public void SetId(int id)
    {
        _id = id; 
        foreach(GameObject item in _items)
        {
            item.SetActive(false); 
        }
        if(_id >= 0 && _id < _items.Length)
        {
            _items[_id].SetActive(true);
        }
    }

    public int GetId()
    {
        return _id;
    }

    public int GetIdMax()
    {
        return _items.Length; 
    }
}
