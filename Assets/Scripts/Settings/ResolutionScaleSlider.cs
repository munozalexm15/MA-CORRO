using UnityEngine;
using UnityEngine.UI;

public class ResolutionScaleSlider : MonoBehaviour
{
    public Slider slider;
    public TMPro.TextMeshProUGUI label;

    private readonly string[] names = { "Potato", "Low", "High", "Native" };

    void Start()
    {
        slider.minValue = 0;
        slider.maxValue = 3;
        slider.wholeNumbers = true;

        // Leer valor guardado y aplicarlo visualmente
        int current = SettingsManager.ResolutionScaleIndex;
        slider.value = current;
        UpdateLabel(current);

        // Cuando el jugador mueve el slider
        slider.onValueChanged.AddListener(OnSliderChanged);

        // Asegurarse de que el valor esté aplicado al entrar al menú
        SettingsManager.ApplyResolutionScale(current);
    }

    void OnSliderChanged(float val)
    {
        int index = Mathf.RoundToInt(val);

        SettingsManager.ResolutionScaleIndex = index; // guarda y aplica
        UpdateLabel(index);
    }

    void UpdateLabel(int i)
    {
        if (label != null)
            label.text = names[i];
    }
}