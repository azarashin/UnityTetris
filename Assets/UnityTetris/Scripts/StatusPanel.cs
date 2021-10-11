using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityTetris.Interface;

namespace UnityTetris
{
    public class StatusPanel : MonoBehaviour, IStatusPanel
    {
        [SerializeField]
        private Text _scoreLabel;

        [SerializeField]
        private ReservationList[] _reservationList; 

        private int _currentScore;

        private void UpdateScoreLabel()
        {
            _scoreLabel.text = $"{_currentScore:00000000}";
        }

        public void ResetScore()
        {
            _currentScore = 0;
            UpdateScoreLabel(); 
        }

        public void AddScore(int numberOfLines)
        {
            if(numberOfLines < 0)
            {
                Debug.LogError($"Invalid number of lines {numberOfLines}");
            }
            _currentScore += numberOfLines * numberOfLines * 100;
            UpdateScoreLabel();
        }

        public void UpdateReservation(List<int> reservation)
        {
            for(int i=0;i<_reservationList.Length;i++)
            {
                if(i < reservation.Count())
                {
                    _reservationList[i].SetId(reservation[i]);
                } else
                {
                    _reservationList[i].SetId(-1);
                }
            }
        }
    }

}