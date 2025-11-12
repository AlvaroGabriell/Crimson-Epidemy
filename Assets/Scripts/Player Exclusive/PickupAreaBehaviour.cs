using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PickupAreaBehaviour : MonoBehaviour
{
    public CircleCollider2D pickupArea;
    public AttributesSystem playerAttributes;
    private float lastPickupRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickupArea = GetComponent<CircleCollider2D>();
        playerAttributes = gameObject.GetComponentInParent<AttributesSystem>();

        lastPickupRange = playerAttributes.pickupRange.FinalValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPickupRange != playerAttributes.pickupRange.FinalValue)
        {
            UpdatePickupRange();
        }
    }

    private void UpdatePickupRange()
    {
        pickupArea.radius = playerAttributes.pickupRange.FinalValue;
        lastPickupRange = playerAttributes.pickupRange.FinalValue;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Collectable")) return;

        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb == null) return;

        Vector2 direction = transform.position - collision.transform.position;
        float distance = direction.magnitude;
        direction.Normalize();

        float attractionStrength = Mathf.Clamp01(1 - (distance / pickupArea.radius));
        float speed = Mathf.Lerp(0f, 8f, attractionStrength); // 8 = max velocity

        rb.linearVelocity = direction * speed;
    }
}