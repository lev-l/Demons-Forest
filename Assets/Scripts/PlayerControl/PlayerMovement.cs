using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimations), typeof(StepsSound))]
public class PlayerMovement : Movement
{
    [SerializeField] private KeyCode _dodgeKey;
    [SerializeField] private float _speed, _stealthSpeedCut;
    [SerializeField] private Space _moveSpace;
    private bool _coroutineOngoing;
    private Transform _transform;
    private StepsSound _stepsSound;
    private Dodge _dodge;
    private PlayerAnimations _animations;
    private PlayerObject _player;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");
        _coroutineOngoing = false;
        _notBlocked = true;
        _transform = GetComponent<Transform>();
        _stepsSound = GetComponent<StepsSound>();
        _dodge = GetComponent<Dodge>();
        _animations = GetComponent<PlayerAnimations>();
    }

    private void Update()
    {
        if (_notBlocked)
        {
            Vector3 move = new Vector3();
            move.y = Input.GetAxis("Vertical") * _speed * Time.deltaTime;
            move.x = Input.GetAxis("Horizontal") * _speed * Time.deltaTime;
            _animations.ChangeRunState(move);

            if (!_coroutineOngoing
                && move.sqrMagnitude > 0)
            {
                _coroutineOngoing = true;
                StartCoroutine(nameof(Noising));
            }
            else if (move.sqrMagnitude == 0)
            {
                _coroutineOngoing = false;
                StopAllCoroutines();
            }

            if (Input.GetKeyDown(_dodgeKey))
            {
                _dodge.DoDodge(move);
                _stepsSound.Noise();
            }

            _transform.Translate(_player.StealthMode ? move / _stealthSpeedCut : move,
                                                                            _moveSpace);
        }
    }

    private IEnumerator Noising()
    {
        while (true)
        {
            if (!_player.StealthMode)
            {
                _stepsSound.Noise();
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

}
