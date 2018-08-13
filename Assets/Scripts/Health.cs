using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO :: Health, HealthCount
public class Health : MonoBehaviour {

    [Header("Health")]
    [SerializeField]
    public int MaximumHealth;

    [SerializeField]
    public TextMesh HealthText;

    public int CurrentHealth;

    public GameObject DeathEffect;
    public bool CollisionsOffOnDeath = true;
    public Action OnDie;
	
    protected virtual void Awake()
    {
        Initialization();
    }

    protected virtual void Initialization()
    {
        CurrentHealth = MaximumHealth;
    }

    public virtual void TakeDamage(int damage, GameObject instigator=null)
    {
        if (CurrentHealth <= 0)
            return;

        CurrentHealth -= damage;
        
        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Kill();
        }

        UpdateHealthText();
    }

    public virtual void Kill(bool notify=true)
    {
        CurrentHealth = 0;

        if (notify && OnDie != null)
            OnDie();

        if (CurrentHealth > 0)
            return;

        if (!gameObject.activeSelf)
            return;

        if(DeathEffect != null)
        {
            var instantiatedEffect = Instantiate(DeathEffect, transform.position, transform.rotation);
            instantiatedEffect.transform.localScale = transform.localScale;
        }
    }

    public virtual int GetHealth(int health)
    {
        int healed = health;

        CurrentHealth += health;

        if(CurrentHealth > MaximumHealth)
        {
            healed -= CurrentHealth - MaximumHealth;
            CurrentHealth = MaximumHealth;
        }

        UpdateHealthText();

        return healed;
    }

    protected virtual void UpdateHealthText()
    {
        if(HealthText != null)
            HealthText.text = $"{String.Format("{0:D2}", CurrentHealth)}";
    }
}
