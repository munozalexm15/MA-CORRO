using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class MapDarkener : MonoBehaviour
{
    public Volume volume; // Arrástralo en el Inspector
    public float fadeDuration = 0.5f; // Tiempo que tarda en oscurecer o aclarar
    public float holdDuration = 1f; // Tiempo que se queda oscuro
    public float targetExposure = -1.5f; // Qué tan oscuro se pone

    private ColorAdjustments colorAdjustments;

    public ObjectPool objPoolManager;

    private void Start()
    {
        if (volume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.active = true;
            colorAdjustments.postExposure.value = 0f;
        }
    }

    public void TriggerDarkEffect()
    {
        StopAllCoroutines(); // Por si se activa varias veces seguidas
        StartCoroutine(DarkEffectSequence());
    }

    private IEnumerator DarkEffectSequence()
    {
        objPoolManager.StopMap();
        yield return StartCoroutine(FadeExposure(0f, targetExposure, fadeDuration));
        

        yield return new WaitForSeconds(holdDuration);

        objPoolManager.ContinueMap();
        yield return StartCoroutine(FadeExposure(targetExposure, 0f, fadeDuration));
    }

    private IEnumerator FadeExposure(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            colorAdjustments.postExposure.value = Mathf.Lerp(from, to, t);
            yield return null;
        }

        colorAdjustments.postExposure.value = to;
    }
}
