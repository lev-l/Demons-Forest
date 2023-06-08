using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimations), typeof(StepsSound))]
public class PlayerMovement : Movement
{
    public int AttackButton;
    [SerializeField] private KeyCode _dodgeKey;
    [SerializeField] private KeyCode _stealthKey;
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
        NotBlocked = true;
        _transform = GetComponent<Transform>();
        _stepsSound = GetComponent<StepsSound>();
        _dodge = GetComponent<Dodge>();
        _animations = GetComponent<PlayerAnimations>();

        _dodge.OnSound += _stepsSound.Noise;
    }

    private void Update()
    {
        if (NotBlocked)
        {
            Vector3 move = new Vector3();
            move.x = Input.GetAxis("Horizontal");
            move.y = Input.GetAxis("Vertical");
            move = move.normalized * _speed * Time.deltaTime;

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
                _dodge.DoDodge(new Vector3(Input.GetAxisRaw("Horizontal"),
                                            Input.GetAxisRaw("Vertical")));
            }
            if (Input.GetKeyDown(_stealthKey))
            {
                _player.ChangeStealthMode();
            }

            _transform.Translate(_player.StealthMode ? move / _stealthSpeedCut : move,
                                                                            _moveSpace);
            _animations.SetAngleToCamera(Vector2.SignedAngle(move, _transform.up) / 180f);
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
