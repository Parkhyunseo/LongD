using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Policy
{
    public class RotationPolicy
    {
        bool _isclockwise = true;

        int _speedPerTurn;

        public bool IsClockWise
        {
            get
            {
                return _isclockwise;
            }
            set
            {
                _isclockwise = value;
            }
        }

        public int SpeedPerTurn
        {
            get
            {
                return _speedPerTurn;
            }
            set
            {
                _speedPerTurn = value;
            }
        }

        public RotationPolicy() : this(1)
        {

        }

        public RotationPolicy(int speed)
        {
            _speedPerTurn = speed;
        }
    }

}
