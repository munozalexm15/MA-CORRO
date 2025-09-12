using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowSettingsManager : MonoBehaviour
{
    [Header("Referencia a la luz principal")]
    public Light mainLight;

    [Header("URP Asset")]
    public UniversalRenderPipelineAsset urpAsset;

    public enum ShadowQuality { Low = 1, Medium = 2, High = 4 }

    private const string KEY_ENABLED = "ShadowsEnabled";
    private const string KEY_DIST = "ShadowsDistance";
    private const string KEY_QUALITY = "ShadowsQuality";

    private void Start()
    {
        InitDefaults();
    }

    public void SetShadowsEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(KEY_ENABLED, enabled ? 1 : 0);
        PlayerPrefs.Save();

        if (mainLight != null)
            mainLight.shadows = enabled ? LightShadows.Soft : LightShadows.None;
    }

    public void SetShadowQualityFromSlider(float sliderValue)
    {
        // Redondear a 0,1,2 y mapear a enum
        int index = Mathf.RoundToInt(sliderValue);
        ShadowQuality quality = ShadowQuality.Medium;

        switch (index)
        {
            case 0: quality = ShadowQuality.Low; break;
            case 1: quality = ShadowQuality.Medium; break;
            case 2: quality = ShadowQuality.High; break;
        }

        PlayerPrefs.SetInt(KEY_QUALITY, (int)quality);
        PlayerPrefs.Save();

        if (urpAsset != null)
            urpAsset.shadowCascadeCount = (int)quality;
    }
    
    public ShadowQuality GetQualityIndex()
    {
        return (ShadowQuality)PlayerPrefs.GetInt(KEY_QUALITY, (int)ShadowQuality.Medium);
    }

    public bool GetShadowsEnabled()
    {
        return PlayerPrefs.GetInt(KEY_ENABLED, 1) == 1;
    }
    
    private void InitDefaults()
    {
        if (!PlayerPrefs.HasKey(KEY_QUALITY))
            PlayerPrefs.SetInt(KEY_QUALITY, (int)ShadowQuality.High); // Por ejemplo, poner Alto por defecto
        if (!PlayerPrefs.HasKey(KEY_ENABLED))
            PlayerPrefs.SetInt(KEY_ENABLED, 1); // Sombras activadas por defecto
        if (!PlayerPrefs.HasKey(KEY_DIST))
            PlayerPrefs.SetFloat(KEY_DIST, 50f);

        PlayerPrefs.Save();
    }
}