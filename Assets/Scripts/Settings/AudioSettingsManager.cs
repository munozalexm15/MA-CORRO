using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro; // Solo si usas TextMeshPro

public class AudioSettingsManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Text musicText;  // Texto de porcentaje de música
    [SerializeField] private TMP_Text sfxText;    // Texto de porcentaje de SFX

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    private const string KEY_MUSIC = "volume_music";
    private const string KEY_SFX = "volume_sfx";

    private void Start()
    {
        float musicVol = PlayerPrefs.GetFloat(KEY_MUSIC, 1f);
        float sfxVol = PlayerPrefs.GetFloat(KEY_SFX, 1f);

        musicSlider.value = musicVol;
        sfxSlider.value = sfxVol;

        ApplyVolumes();

        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxChanged);
    }

    private void OnMusicChanged(float value)
    {
        PlayerPrefs.SetFloat(KEY_MUSIC, value);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    private void OnSfxChanged(float value)
    {
        PlayerPrefs.SetFloat(KEY_SFX, value);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        float musicDb = Mathf.Log10(Mathf.Max(musicSlider.value, 0.0001f)) * 20f;
        float sfxDb = Mathf.Log10(Mathf.Max(sfxSlider.value, 0.0001f)) * 20f;

        audioMixer.SetFloat("MusicVol", musicDb);
        audioMixer.SetFloat("SFXVol", sfxDb);

        // Actualizar textos (0–100%)
        musicText.text = Mathf.RoundToInt(musicSlider.value * 100f) + "";
        sfxText.text = Mathf.RoundToInt(sfxSlider.value * 100f) + "";
    }
}