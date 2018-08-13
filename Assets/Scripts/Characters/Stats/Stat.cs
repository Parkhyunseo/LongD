using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Stats
{
    /// <summary>
    /// 무기
    /// 1.공격력
    /// 2.방어력
    /// 건물
    /// 1.체력
    /// 2.방어력
    /// 쉴드
    /// 1.체력
    /// 2.방어력
    /// 3.N번 막을 수 있음
    /// 
    /// </summary>
    /// 
    [Serializable]
    public class Stat
    {
        [SerializeField]
        StatBase _base = new StatBase();
        
        public Stat()
        {
            _base.Initialize();
        }
    }

}
