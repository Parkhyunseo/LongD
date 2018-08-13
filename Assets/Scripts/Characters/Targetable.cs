using System;
using UnityEngine;

/// <summary>
/// Card의 Target이 될 수 있는 추상 클래스.
/// </summary>
public abstract class Targetable : MonoBehaviour
{
    public abstract void Target(Action todo);
}
