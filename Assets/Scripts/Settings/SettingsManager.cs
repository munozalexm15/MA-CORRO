using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class SettingsManager
{
    private const string RESOLUTION_SCALE_KEY = "ResolutionScale";
    private static readonly float[] ResolutionScales = { 0.5f, 0.67f, 0.83f, 1.0f };

    public static int ResolutionScaleIndex
    {
        get => PlayerPrefs.GetInt(RESOLUTION_SCALE_KEY, 2); // 2 = Recomendada
        set
        {
            PlayerPrefs.SetInt(RESOLUTION_SCALE_KEY, value);
            PlayerPrefs.Save();
            ApplyResolutionScale(value);
        }
    }

    public static void ApplyResolutionScale(int index)
    {
        var urp = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        if (urp != null)
            urp.renderScale = ResolutionScales[index];
    }

    // Puedes añadir más ajustes aquí:
    // public static float Volume, public static int Quality, etc.
}