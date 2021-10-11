using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris.Interface
{
    public interface IStatusPanel
    {
        void ResetScore();
        void AddScore(int numberOfLines);
        void UpdateReservation(List<int> reservation); 
    }
}