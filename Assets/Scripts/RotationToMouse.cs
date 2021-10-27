using System.Collections;
using UnityEngine;

public class RotationToMouse : RotateToTarget
{
    private Camera _camera;
    private Transform _transform;

    private void Start()
    {
        _camera = Camera.main;
        _transform = GetComponent<Transform>();
    }

    private void Update()
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
