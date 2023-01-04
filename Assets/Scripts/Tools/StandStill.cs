using UnityEngine;

public class StandStill : MonoBehaviour
{
    private Transform _transform;
    private Transform _parentTransform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _parentTransform = _transform.parent;
    }

    private void Update()
    {
        _transform.localRotation = Quaternion.Inverse(_parentTransform.localRotation);
    }
}
