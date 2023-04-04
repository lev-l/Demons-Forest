using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicChange : MonoBehaviour
{
    public AnimationCurve DecayRate;
    public AnimationCurve RisingRate;
    private AudioSource _audio;
    private AudioMixer _audioMixer;
    private AudioClip _currentTrack;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audioMixer = Resources.Load<AudioMixer>("AudioMixer");
    }

    public void StartNewMusic(AudioClip track)
    {
        _currentTrack = track;
        StartCoroutine(nameof(MusicDecaying));
    }

    private IEnumerator MusicDecaying()
    {
        float currentTime = 0;
        float targetTime = DecayRate.keys[DecayRate.length - 1].time;

        while (currentTime < targetTime)
        {
            float value = DecayRate.Evaluate(currentTime);
            _audioMixer.SetFloat("MusicVolume", value * 80f);
            yield return null;

            currentTime += Time.deltaTime;
        }

        StartCoroutine(MusicStarting(_currentTrack));
    }

    private IEnumerator MusicStarting(AudioClip newTrack)
    {
        float currentTime = 0;
        float targetTime = RisingRate.keys[DecayRate.length - 1].time;

        while (currentTime < targetTime)
        {
            float value = RisingRate.Evaluate(currentTime);
            _audioMixer.SetFloat("MusicVolume", value * 80f);
            yield return null;

            currentTime += Time.deltaTime;
        }
    }
}
