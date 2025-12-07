using System.Collections;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] public bool isActive = false;
    [SerializeField] protected bool isOnce = false;
    [SerializeField] protected float updateInterval = 0.1f;

    private Unicycle _player;
    
    protected virtual void Enter(Unicycle player) {}
    protected virtual void Apply(Unicycle player) {}
    protected virtual void Exit(Unicycle player) {}
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = true;
            _player = other.GetComponent<Unicycle>();
            Enter(_player);
            if (!isOnce)
                StartCoroutine(AreaEffectCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = false;
            StopCoroutine(AreaEffectCoroutine());
            Exit(_player);
        }
    }
    
    private IEnumerator AreaEffectCoroutine()
    {
        while (isActive)
        {
            Apply(_player);
            yield return new WaitForSeconds(updateInterval);
        }
    }
}
