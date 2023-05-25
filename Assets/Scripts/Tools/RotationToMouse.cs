using System.Collections;
using UnityEngine;

public class RotationToMouse : RotateToTarget
{
    private Camera _camera;
    private Transform _transform;
    private PlayerObject _player;

    private void Start()
    {
        _camera = Camera.main;
        _transform = GetComponent<Transform>();
        _player = Resources.Load<PlayerObject>("Player");
    }

    private void Update()
    {
        if (_player.FreeRotation)
        {
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

            _transform.rotation = Quaternion.AngleAxis(Rotate(
                                                            position: _transform.position,
                                                            target: mousePosition,
                                                            -90
                                                            ),
                                                        Vector3.forward
            );
        }
    }
}
