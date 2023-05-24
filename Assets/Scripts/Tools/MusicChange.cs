using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicChange : MonoBehaviour
{
    [SerializeField] private AudioClip _stealthMusic;
    [SerializeField] private AudioClip _fightMusic;
    public AnimationCurve DecayRate;
    public AnimationCurve RisingRate;
    private AudioSource _audio;
    private AudioClip _currentTrack;
    private AudioClip _nextTrack;
    private PlayerObject _playerSO;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _playerSO = Resources.Load<PlayerObject>("Player");

        _playerSO.OnStealthChanged += ChangeStealth;
        _playerSO.OnZeroEnemies += StopFightMusic;
        Resources.Load<FightNoticeObject>("FightEvent").OnFightBegan += StartFightMusic;

        _currentTrack = _audio.clip;
        _nextTrack = _currentTrack;
    }

    public void StartNewMusic(AudioClip track)
    {
        _nextTrack = track;
        StopAllCoroutines();

        if(_currentTrack == null)
        {
            StartCoroutine(nameof(MusicStarting));
            return;
        }
        StartCoroutine(nameof(MusicDecaying));
    }

    private IEnumerator MusicDecaying()
    {
        float currentTime = 0;
        float targetTime = DecayRate.keys[DecayRate.length - 1].time;

        while (currentTime < targetTime)
        {
            float value = DecayRate.Evaluate(currentTime);
            _audio.volume = value;
            yield return null;

            currentTime += Time.deltaTime;
        }

        StartCoroutine(nameof(MusicStarting));
    }

    private IEnumerator MusicStarting()
    {
        float currentTime = 0;
        float targetTime = RisingRate.keys[DecayRate.length - 1].time;

        _audio.clip = _nextTrack;
        _currentTrack = _audio.clip;
        _audio.Play();

        while (currentTime < targetTime)
        {
            float value = RisingRate.Evaluate(currentTime);
            _audio.volume = value;
            yield return null;

            currentTime += Time.deltaTime;
        }
    }

    public void ChangeStealth(bool stealth)
    {
        AudioClip next = stealth ? _stealthMusic : null;

        if (next != _nextTrack
            && _nextTrack != _fightMusic)
        {
            StartNewMusic(next);
        }
    }

    public void StartFightMusic()
    {
        if (_fightMusic != _nextTrack)
            StartNewMusic(_fightMusic);
    }

    public void StopFightMusic()
    {
        if (null != _nextTrack)
            StartNewMusic(null);
    }
}
