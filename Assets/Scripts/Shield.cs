using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Characters.Planets;

[RequireComponent(typeof(Health))]
public class Shield : Attackable {

    Planet _owner;
    Health _health;
    
    void Awake()
    {
        _health = GetComponent<Health>();
        _health.OnDie = () =>
        {
            Debug.Log("Shield Current Health : " + _health.CurrentHealth);
            Destroy(gameObject);
            //Managers.AnimationManager.Instance.
        };
    }

    public void Setting(int maxHP, Planet owner)
    {
        _health.MaximumHealth = maxHP;
        _health.CurrentHealth = maxHP;
        _owner = owner;
    }

    public override void TakeDamage(int damage, GameObject Insticator)
    {
        _health.TakeDamage(damage);
    }
}
