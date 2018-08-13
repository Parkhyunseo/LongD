using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Characters.Buildings;

namespace Modifiers
{
    /// <summary>
    /// 상태에 변화를 주는 메서드를 정의.
    /// </summary>
    public abstract class Modifier : MonoBehaviour
    {
        /// <summary>
        /// target 캐릭터에게 상태 변화를 적용합니다. 수치는 stat을 기반으로 계산됩니다.
        /// </summary>
        public abstract void Modify(Building building);
    }

}
