using System.Collections;
using UnityEngine;

public class PulseText : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PulseLoop(transform));
    }

    public static IEnumerator PulseLoop(Transform target, float duration = 0.7f, float scale = 1.1f)
{
    Vector3 originalScale = target.localScale;
    Vector3 biggerScale = originalScale * scale;

    while (true)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Sin(t / duration * Mathf.PI); // curva suave
            target.localScale = Vector3.Lerp(originalScale, biggerScale, lerp);
            yield return null;
        }
    }
}
}
