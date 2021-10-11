using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReservationList : MonoBehaviour
{
    [SerializeField]
    GameObject[] _items; 

    public void SetId(int id)
    {
        foreach(GameObject item in _items)
        {
            item.SetActive(false); 
        }
        if(id >= 0 && id < _items.Length)
        {
            _items[id].SetActive(true);
        }
    }
}
