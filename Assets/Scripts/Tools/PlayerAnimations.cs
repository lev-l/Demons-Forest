using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public AnimationCurve StealthChanging;
    private Animator _animator;
    private AudioSource _stepsSound;
    private bool _wasMoving = false;
    private bool _wasStealth = false;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _stepsSound = GetComponent<AudioSource>();

        Resources.Load<PlayerObject>("Player").OnStealthChanged += StartStealthChanging;
    }

    public void ChangeRunState(Vector2 move)
    {
        bool isMoving = move.sqrMagnitude > 0;
        bool change = isMoving != _wasMoving || _isStealth != _wasStealth;
        _wasMoving = isMoving;
        _wasStealth = _isStealth;

        _animator.SetBool("Run", isMoving);

        if (change && isMoving
            && !_isStealth)
        {
            _stepsSound.Play();
        }
        else if (change)
        {
            _stepsSound.Pause();
        }
    }

    public void PrepareAttackAnimation()
    {
        _animator.SetTrigger("PrepareAttack");
    }

    public void PrepareStabAnimation()
    {
        _animator.SetTrigger("PrepareStab");
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }

    public void PlayStabAnimation()
    {
        _animator.SetTrigger("Stab");
    }

    public void SetAngleToCamera(float value)
    {
        _animator.SetFloat("AngleToCamera", value);
    }

    public void StartStealthChanging(bool stealth)
    {
        StartCoroutine(ChangeStealth(stealth));
    }

    private IEnumerator ChangeStealth(bool stealth)
    {
        int changeDirection = stealth ? -1 : 1;
        float timeToChange = StealthChanging.keys[StealthChanging.length - 1].time;

        for(float time = 0; time < timeToChange; time += Time.deltaTime)
        {
            _animator.SetFloat("Stealth", StealthChanging.Evaluate(time) * changeDirection);

            yield return null;
        }
    }

    private bool _isStealth => _animator.GetFloat("Stealth") >= 0 ? false : true;
}
