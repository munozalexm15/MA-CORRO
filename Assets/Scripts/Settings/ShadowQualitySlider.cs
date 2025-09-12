using UnityEngine;
using UnityEngine.UI;
using TMPro; // Si usas TextMeshPro

public class ShadowQualitySliderUI : MonoBehaviour
{
    [Header("Referencias")]
    public Slider qualitySlider;
    public ShadowSettingsManager shadowManager;

    [Header("Referencia al texto que muestra el valor")]
    public TMP_Text qualityLabel; // o Text si no usas TMP

    private void Start()
    {
        if (qualitySlider == null || shadowManager == null)
        {
            Debug.LogWarning("Faltan referencias asignadas");
            return;
        }

        // Aplicar valor inicial del PlayerPrefs al slider
        int savedQuality = (int)shadowManager.GetQualityIndex(); // 1=Low,2=Medium,4=High
        switch (savedQuality)
        {
            case 1:
                qualitySlider.value = 0;
                savedQuality = 0;
                break;
            case 2:
                qualitySlider.value = 1;
                savedQuality = 1;
                break;
            case 4:
                qualitySlider.value = 2;
                savedQuality = 2;
                break;
        }
        
        UpdateLabel(savedQuality);

        // Suscribirse al evento
        qualitySlider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        int index = Mathf.RoundToInt(value);
        shadowManager.SetShadowQualityFromSlider(index);
        UpdateLabel(index);
    }

    private void UpdateLabel(int index)
    {
        if (qualityLabel == null) return;
        Debug.Log(index);
        switch (index)
        {
            case 0: qualityLabel.text = "Low"; break;
            case 1: qualityLabel.text = "Medium"; break;
            case 2: qualityLabel.text = "High"; break;
        }
    }
}