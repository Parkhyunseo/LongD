using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Characters.Players;
using Managers;

namespace Tasks
{
    public class TaskScheduler : Singletons.Singleton<TaskScheduler>
    {
        // TODO :: Task 추가, AnimationTask추가, TurnEndcallback
        Queue<Task> _taskQueue;

        Queue<Task> _interceptTaskQueue;
        Queue<Task> _trapQueue;

        Action<Playable> _turnOverCallback; 
        GameManager _gm;

        protected override void Awake()
        {
            base.Awake();
            _taskQueue = new Queue<Task>();
            _interceptTaskQueue = new Queue<Task>();
            _trapQueue = new Queue<Task>();
        }

        public void Play()
        { 
            // TODO :: 함정카드와 요격탄 처리
            while(_taskQueue.Count > 0)
            {
                Task task = _taskQueue.Dequeue();
                task.Initialize();
            }

        }

        public void AddTask(Task task)
        {
            // TODO :: Queue에 한계를 둘 경우 여기서 Filtering
            _taskQueue.Enqueue(task);
        }

        public void Intercept()
        {
            if (_interceptTaskQueue.Count > 0)
            {
                Task task = _interceptTaskQueue.Dequeue();

            }
        }

        public void InvokeTrap()
        {
            if (_interceptTaskQueue.Count > 0)
            {
                Task task = _interceptTaskQueue.Dequeue();
            }
        }

        public bool IsUseable()
        {
            return _taskQueue.Count > 0 ? true : false;
        }
    }

}
