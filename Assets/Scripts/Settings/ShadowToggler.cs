using UnityEngine;
using UnityEngine.UI;

public class ShadowToggleUI : MonoBehaviour
{
    public Toggle toggle;
    public ShadowSettingsManager shadowManager;

    void Start()
    {
        // Aplicar valor inicial
        toggle.isOn = shadowManager.GetShadowsEnabled();

        // Suscribirse dinámicamente
        toggle.onValueChanged.AddListener(shadowManager.SetShadowsEnabled);
    }
}