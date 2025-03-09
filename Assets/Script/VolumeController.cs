using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        if (GlobalAudioManager.Instance != null)
        {
            volumeSlider.value = GlobalAudioManager.Instance.globalVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }

    public void OnVolumeChanged(float value)
    {
        if (GlobalAudioManager.Instance != null)
            GlobalAudioManager.Instance.SetVolume(value);
    }
}
