using UnityEngine;
using UnityEngine.UI;

public class SliderSounds : MonoBehaviour
{
    [SerializeField] private Slider _sounds;
    [SerializeField] private Slider _music;
    private void Start()
    {
        if (_sounds) _sounds.value = SoundEffector.Instance.Sound.volume;
        if (_music) _music.value = SoundEffector.Instance.Music.volume;
    }

    public void SetSoundVolume(Slider slider)
    {
        SoundEffector.Instance.SetSoundVolume(slider.value);
    }
    public void SetMusicVolume(Slider slider)
    {
        SoundEffector.Instance.SetMusicVolume(slider.value);
    }
}
