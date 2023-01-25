using NoobDev;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoSingleton<SoundEffector>
{
    public AudioSource Sound;
    public AudioSource Music;
    public AudioClip winSound, loseSound;

    [Header("UI sounds")]
    public AudioClip clickSound;


    private void Start()
    {
        Debug.Log("MusicPref " + Yandexs.Instance.Data.MusicVolume);
        Debug.Log("SoundEffectsPref " + Yandexs.Instance.Data.SoundVolume);
        Music.volume = Yandexs.Instance.Data.MusicVolume;
        Sound.volume = Yandexs.Instance.Data.SoundVolume;
        _currentClip = 0;
        SoundEffector.Instance.Play();
    }

    public void SetSoundVolume(float value)
    {
        Sound.volume = value;
        Yandexs.Instance.Data.SoundVolume = value;
        Yandexs.Instance.SaveData ();
    }
    public void SetMusicVolume(float value)
    {
        Music.volume = value;
        Yandexs.Instance.Data.MusicVolume = value;
        Yandexs.Instance.SaveData ();
    }

    public void PlaywinSound()
    {
        Sound.PlayOneShot(winSound);
    }

    public void PlayloseSound()
    {
        Sound.PlayOneShot(loseSound);
    }

    public void PlayClick()
    {
        Sound.PlayOneShot(clickSound);
    }

    #region -MUSIC-
    public enum PlayM
    {
        single,
        loop,
        random
    }
    [SerializeField] private List<AudioClip> _audioClips = new List<AudioClip>();
    public PlayM PlayMode;
    private int _currentClip;
    public void Play()
    {
        if (_audioClips.Count < 1) return;
        if (_currentClip >= _audioClips.Count)
        {
            _currentClip = 0;
            if (PlayMode == PlayM.single) return;

            if (PlayMode == PlayM.random)
            {
                if (SetClip((int)Random.Range(0, _audioClips.Count)) == null) return;
            }
        }
        else
        {
            SetClip(_currentClip);
        }
        Music.Play();
    }
    public string SetClip(int id)
    {
        if (id >= _audioClips.Count) return null;

        _currentClip = id;
        Music.clip = _audioClips[_currentClip];
        return _audioClips[0].name;
    }
    #endregion
}
