using UnityEngine;
using System.Collections;

public class AutoDespawn : MonoBehaviour
{
    [SerializeField]
    Reusable _reusable;
    [SerializeField]
    float _duration;

    protected void Awake()
    {
        if (_reusable == null)
            _reusable = GetComponent<Reusable>();
    }

    private void OnEnable()
    {
        StartCoroutine(CDespawn());
    }

    IEnumerator CDespawn()
    {
        yield return new WaitForSeconds(_duration);
        SimplePool.Despawn(_reusable);
    }
}