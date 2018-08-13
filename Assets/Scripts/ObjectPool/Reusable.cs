using UnityEngine;
using System;

public sealed class Reusable : MonoBehaviour, IReuseable
{
    bool _isSpawned;

    internal SimplePool.Pool pool;

    public Action OnInitailze
    {
        get;
        set;
    }

    public Action OnHibernate
    {
        get;
        set;
    }

    public bool isSpawned
    {
        get
        {
            return _isSpawned;
        }
    }

    public void Initailize()
    {
        if (OnInitailze != null) OnInitailze();
        _isSpawned = true;
    }

    public void Hibernate()
    {
        if (OnHibernate != null) OnHibernate();
        _isSpawned = false;
    }
}