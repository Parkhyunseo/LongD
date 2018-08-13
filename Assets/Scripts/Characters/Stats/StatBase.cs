using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Stats
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class StatBase : ISerializationCallbackReceiver
    {
        [SerializeField]
        protected StatForSerialize _serializeField;
        readonly public StatData stat = new StatData();


        public void Initialize()
        {
            stat.SetAll(0);
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            Initialize();

            // NOTE : 스탯 변경 직후에 Index Out of Range 예외 발생하나 껐다키면 괜찮아져서 그냥 둠.
            _serializeField.Serialize();

            for (int i = 0; i < stat.Array.Length; i++)
            {
                if (!string.IsNullOrEmpty(_serializeField.Array[i]))
                {
                    int value;
                    if (int.TryParse(_serializeField.Array[i], out value))
                    {
                        stat.Array[i] = value;
                    }else
                    {
                        Debug.LogError("StatBase Serialize Error");
                    }
                }
            }
        }
    }

}
