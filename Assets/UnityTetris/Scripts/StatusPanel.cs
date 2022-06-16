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
        private Text _levelLabel;

        [SerializeField]
        private ReservationList[] _reservationList; 

        private int _currentScore;
        private int _currentLevel;

        public int Score()
        {
            return _currentScore; 
        }

        public int Level()
        {
            return _currentLevel; 
        }

        public (int, int) Next(int id)
        {
            if (id < 0 || id >= _reservationList.Length)
            {
                return (-1, -1);
            }
            return (_reservationList[id].GetId(), _reservationList[id].GetIdMax());
        }

        private void UpdateScoreLabel()
        {
            _scoreLabel.text = $"{_currentScore:00000000}";
            _levelLabel.text = $"{_currentLevel:00}";
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
            _currentScore += _currentLevel * numberOfLines * numberOfLines * 100;
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

        public void UpdateLevel(int level)
        {
            _currentLevel = level;
            UpdateScoreLabel(); 
        }
    }

}