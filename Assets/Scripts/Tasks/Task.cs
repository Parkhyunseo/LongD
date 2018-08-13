using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Characters.Players;
using Types;

namespace Tasks
{
    public class Task : ITask
    {
        // 만약 애니메이터를 안쓴다면
        Action _activeAnimation;

        Playable _user;
        Action _behavior;

        public Playable user
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        public Action behavior
        {
            get
            {
                return _behavior;
            }
            set
            {
                _behavior = value;
            }
        }

        public Task()
        {

        }

        public Task(Action action)
        {
            _behavior = action;
        }

        public Task(Action action, Playable user)
        {
            _behavior = action;
            _user = user;
        }

        public void Initialize()
        {
            _behavior.Invoke();
        }
    }
}
