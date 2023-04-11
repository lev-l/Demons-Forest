using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicChange : MonoBehaviour
{
    [SerializeField] private AudioClip _stealthMusic;
    [SerializeField] private AudioClip _ambient;
    [SerializeField] private AudioClip _fightMusic;
    public AnimationCurve DecayRate;
    public AnimationCurve RisingRate;
    private AudioSource _audio;
    private AudioMixer _audioMixer;
    private AudioClip _currentTrack;
    private PlayerObject _playerSO;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audioMixer = Resources.Load<AudioMixer>("AudioMixer");
        _playerSO = Resources.Load<PlayerObject>("Player");

        _playerSO.OnStealthChanged += ChangeStealth;
        _playerSO.OnZeroEnemies += StopFightMusic;
        Resources.Load<FightNoticeObject>("FightEvent").OnFightBegan += StartFightMusic;
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

        _audio.clip = _currentTrack;
        _audio.Play();

        while (currentTime < targetTime)
        {
            float value = RisingRate.Evaluate(currentTime);
            _audioMixer.SetFloat("MusicVolume", value * 80f);
            yield return null;

            currentTime += Time.deltaTime;
        }
    }

    public void ChangeStealth(bool stealth)
    {
        AudioClip next = stealth ? _stealthMusic : _ambient;

        if (next != _currentTrack)
        {
            StartNewMusic(next);
        }
    }

    public void StartFightMusic()
    {
        if (_fightMusic != _currentTrack)
            StartNewMusic(_fightMusic);
    }

    public void StopFightMusic()
    {
        if (_ambient != _currentTrack)
            StartNewMusic(_ambient);
    }
}
