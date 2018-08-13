using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subcomponent<T> : MonoBehaviour where T : Component
{
    [SerializeField]
    protected T _super;

    public T super
    {
        get
        {
            return _super;
        }
    }

    protected virtual void Awake()
    {
        if (_super == null)
        {
            _super = GetComponent<T>();

            if (_super == null)
                Debug.LogWarning("Could not find a super component!");
        }
    }
}