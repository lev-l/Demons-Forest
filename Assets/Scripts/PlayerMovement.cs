using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimations), typeof(StepsSound))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed, _stealthSpeedCut;
    [SerializeField] private Space _moveSpace;
    private bool _coroutineOngoing;
    private bool _stealthMode;
    private Transform _transform;
    private StepsSound _stepsSound;
    private PlayerAnimations _animations;
    private PlayerObject _player;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");
        _coroutineOngoing = false;
        _stealthMode = false;
        _transform = GetComponent<Transform>();
        _stepsSound = GetComponent<StepsSound>();
        _animations = GetComponent<PlayerAnimations>();
    }

    private void Update()
    {
        Vector3 move = new Vector3();
        move.y = Input.GetAxis("Vertical") * _speed * Time.deltaTime;
        move.x = Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
        _animations.ChangeRunState(move);

        if(!_coroutineOngoing
            && move.sqrMagnitude > 0)
        {
            _coroutineOngoing = true;
            StartCoroutine(nameof(Noising));
        }
        else if(move.sqrMagnitude == 0)
        {
            _coroutineOngoing = false;
            StopAllCoroutines();
        }

        _transform.Translate(move, _moveSpace);
    }

    private IEnumerator Noising()
    {
        while (true)
        {
            if (!_stealthMode)
            {
                _stepsSound.Noise();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ChangeStealthMod()
    {
        _stealthMode = !_stealthMode;
        if (_stealthMode)
        {
            _speed /= _stealthSpeedCut;
        }
        else
        {
            _speed *= _stealthSpeedCut;
        }
        _player.StealthMode = _stealthMode;
    }
}
