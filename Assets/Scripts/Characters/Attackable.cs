using UnityEngine;

/// <summary>
/// 공격 대상이 될 수 있는 인터페이스.
/// </summary>
public abstract class Attackable : MonoBehaviour
{
    public abstract void TakeDamage(int damage, GameObject Insticator);
}
