using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class LogManager : MonoBehaviour
    {
        public int maxQueueSize;
        Queue<Log> _logSpriteQueue;
        Vector3 nextPosition;
        float _padding;

        private void Awake()
        {
            _logSpriteQueue = new Queue<Log>(maxQueueSize);
        }
        
        // 위에서 아래로 쌓이다가

        void AddLog(Log log)
        {
            if(_logSpriteQueue.Count >= maxQueueSize)
            {
                _logSpriteQueue.Dequeue();
                // TODO :: Log 위로 올라가는 Animation
            }
            _logSpriteQueue.Enqueue(log);
        }

        public void PresentLog(GameObject log)
        {
            AddLog(log.GetComponent<Log>());
            var obj = Instantiate(log);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = nextPosition;
        }

        // View를 어떻게 할 것인가

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
