using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] MeshRenderer mr;
    [SerializeField] Collider cd;
    [SerializeField] protected float effectDuration = 3f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] GameObject childObject;
    
    [SerializeField] private bool instant = false;
    private bool _active = false;

    private void Start()
    {
        moveSpeed = LevelManager.GetObstacleSpeed();
    }

    private void Update()
    {
        transform.Translate(Vector3.back * (moveSpeed * Time.deltaTime));
        if (transform.position.z < -10f && !_active)
        {
            var newColor = mr!.material.color;
            newColor.a = 0.5f;

            mr.material.color = newColor;
            Destroy(gameObject);
        }
    }

    public virtual void ApplyEffect(Unicycle player)
    {
        StartCoroutine(EffectDurationCoroutine(player));
    }
    
    protected virtual IEnumerator EffectDurationCoroutine(Unicycle player)
    {
        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Unicycle>();
            
            if (instant)
            {
                ApplyEffect(player);
                Destroy(gameObject);
                return;
            }
            
            player.SetActiveItem(this);
            _active = true;
            SetInvisible();
        }
    }

    private void SetInvisible()
    {
        mr!.enabled = false;
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
