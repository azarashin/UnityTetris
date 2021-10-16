using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

namespace UnityTetris.AI
{
    public class MLInputAgent : Agent
    {

        [SerializeField]
        Player _player;

        [SerializeField]
        UnityKeyInputManager _heulisticInput; 

        private const int _maxLevel = 10;

        private int _currentScore;

        public override void Initialize()
        {
            //Debug.Log($"TrainingPlayer.Initialize");
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            // 幅 + 高さ + 4 + ブロックの種類 * 3 + 幅 * 高さ
            string obs = ""; 
            (int, int)[] player_obs = _player.Observe();
            foreach ((int oneHot, int max) in player_obs)
            {
                sensor.AddOneHotObservation(oneHot, max);
                obs += $"{oneHot}/{max} ";
            }
            float level = (float)_player.StatusPanel.Level() / _maxLevel;
            (int next0, int next_max0) = _player.StatusPanel.Next(0);
            (int next1, int next_max1) = _player.StatusPanel.Next(1);
            sensor.AddOneHotObservation(next0, next_max0);
            obs += $"{next0}/{next_max0} ";
            sensor.AddOneHotObservation(next1, next_max1);
            obs += $"{next1}/{next_max1} ";
            if (level < 0.0f)
            {
                level = 0.0f;
            }
            if (level > 1.0f)
            {
                level = 1.0f;
            }
            int[,] map = _player.Field.GetFieldMap();
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    sensor.AddOneHotObservation(map[x, y], 2);
                    obs += $"{map[x, y]}/{2} ";
                }
            }
            sensor.AddObservation(level);
            obs += $"{level}";
//            Debug.Log("obs:"+obs);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            if (!_player.IsAlive())
            {
                EndEpisode();
            }
            GetComponent<MLInput>().UpdateActions(actions.DiscreteActions.Array);

            int score = _player.StatusPanel.Score();
            int delta = score - _currentScore;
            _currentScore = score;
            float fdelta = delta / 2000.0f;
            if (fdelta > 1.0f)
            {
                fdelta = 1.0f;
            }
            if (fdelta > 0.0f)
            {
                AddReward(fdelta);
            }
            AddReward(0.0001f);
        }

        public override void OnEpisodeBegin()
        {
            _currentScore = 0;
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            actionsOut.DiscreteActions.Array[0] = 0;

            if (_heulisticInput.IsMoveLeft())
            {
                // left move
                actionsOut.DiscreteActions.Array[0] = 1; 
            }

            if(_heulisticInput.IsMoveRight())
            {
                // right move
                actionsOut.DiscreteActions.Array[0] = 2;
            }

            if (_heulisticInput.IsMoveDown())
            {
                // down
                actionsOut.DiscreteActions.Array[0] = 3;
            }

            if (_heulisticInput.IsRotateLeft())
            {
                // rotate left
                actionsOut.DiscreteActions.Array[0] = 4;
            }

            if (_heulisticInput.IsRotateRight())
            {
                // rotate right
                actionsOut.DiscreteActions.Array[0] = 5;
            }

        }



    }
}
