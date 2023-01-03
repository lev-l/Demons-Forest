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
        //try Quaternion.Inverse instead
        _transform.localRotation = Quaternion.Euler(-_parentTransform.localRotation.eulerAngles);
    }
}
