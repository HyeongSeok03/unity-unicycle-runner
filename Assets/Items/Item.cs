using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] MeshRenderer mr;
    [SerializeField] Collider cd;
    [SerializeField] protected float effectDuration = 3f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] GameObject childObject;
    bool _active = false;

    void Start()
    {
        moveSpeed = LevelManager.GetObstacleSpeed();
    }

    void Update()
    {
        transform.Translate(Vector3.back * (moveSpeed * Time.deltaTime));
        if (transform.position.z < 0f && !_active)
        {
            var newColor = mr.material.color;
            newColor.a = 0.5f;

            mr.material.color = newColor;
            Destroy(gameObject, 1f);
        }
    }

    protected virtual void ApplyEffect(Unicycle player)
    {
        _active = true;
        SetInvisible();
        StartCoroutine(EffectDurationCoroutine(player));
    }
    
    protected virtual IEnumerator EffectDurationCoroutine(Unicycle player)
    {
        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        Unicycle player = other.GetComponent<Unicycle>();

        if (player != null)
        {
            ApplyEffect(player);
        }
    }

    private void SetInvisible()
    {
        mr.enabled = false;
        cd.enabled = false;
        if(childObject != null)
            childObject.SetActive(false);
    }

    void OnValidate()
    {
        if (mr == null)
            mr = GetComponent<MeshRenderer>();
        if (cd == null)
        {
            cd = GetComponent<Collider>();
            cd.isTrigger = true;
        }
    }
}
