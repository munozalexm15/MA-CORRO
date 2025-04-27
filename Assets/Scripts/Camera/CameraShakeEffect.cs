using UnityEngine;

public class CameraShakeEffect : MonoBehaviour
{
    public static CameraShakeEffect Instance;
    public Vector3 originalPos;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.2f;

    public bool canShake = false;

    void Awake()
    {
        Instance = this;
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0 && canShake)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime;
        }
        else if (shakeDuration < 0 && canShake)
        {
            shakeDuration = 0f;
            transform.localPosition = originalPos;
            canShake = false;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
