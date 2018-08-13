using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Population : MonoBehaviour
{
    [Header("Population")]
    [SerializeField]
    public int MaximumPopulation;

    [SerializeField]
    TextMesh _populationText;

    public int CurrentPopulation;

    public Action OnDie;

    protected virtual void Awake()
    {
        // TODO :: 게임이 시작되었을 때 분배
        Initialization();
        UpdatePopulationText();
    }

    public virtual bool UsePopulation(int cost)
    {
        if (CurrentPopulation <= 0) // TODO :: 사용할 수 없다는 Animation
            return false;

        CurrentPopulation -= cost;

        UpdatePopulationText();
        return true;
    }
 
    protected virtual void Initialization()
    {
        CurrentPopulation = MaximumPopulation;
    }

    private void UpdatePopulationText()
    {
        _populationText.text = $"{String.Format("{0:D3}",CurrentPopulation)}/{String.Format("{0:D3}", MaximumPopulation)}";
    }
}
