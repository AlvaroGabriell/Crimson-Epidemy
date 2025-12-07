using UnityEngine;

public class CollectableBehaviour : MonoBehaviour
{
    public CollectableType collectableType;
    public float value = 0f;

    [Header("Pulse Effect Settings")]
    public float pulseSpeed = 2f;
    public float pulseIntensity = 0.1f;
    private Vector3 baseScale;
    private float pulseOffset;

    void Start()
    {
        float randomScale = Random.Range(0.9f, 1.1f);
        transform.localScale = transform.localScale * randomScale;
        baseScale = transform.localScale;
        
        pulseOffset = Random.value * Mathf.PI * 2;
    }

    void Update()
    {
        float pulse = 1 + Mathf.Sin(Time.time * pulseSpeed + pulseOffset) * pulseIntensity;
        transform.localScale = baseScale * pulse;
    }

    public void SetValue(float value)
    {
        this.value = value;
    }
    public float GetValue()
    {
        return value;
    }
    public void SetCollectableType(CollectableType type)
    {
        collectableType = type;
    }
    public CollectableType GetCollectableType()
    {
        return collectableType;
    }
}

public enum CollectableType
{
    Xp,
    Health
}