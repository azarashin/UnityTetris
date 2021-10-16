using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTetris.Interface
{
    public interface IStatusPanel
    {
        int Score();
        int Level();
        (int, int) Next(int id);

        void ResetScore();
        void AddScore(int numberOfLines);
        void UpdateReservation(List<int> reservation);
        void UpdateLevel(int level); 
    }
}