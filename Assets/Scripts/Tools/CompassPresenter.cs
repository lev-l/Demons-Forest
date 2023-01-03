using UnityEngine;
using TMPro;

public class CompassPresenter : MonoBehaviour
{
    [SerializeField] private Transform _compass;
    private Transform _player;
    private GameObject _markPrefab;
    private CompassData _data;
    private RotateToTarget _rotation;

    private void Start()
    {
        _player = FindObjectOfType<PlayerMovement>().transform;
        _markPrefab = Resources.Load<GameObject>("Mark");
        _data = Resources.Load<CompassData>("CompassDataBase");
        _rotation = GetComponent<RotateToTarget>();
    }

    public void AddAMark(string name)
    {
        GameObject newMark = Instantiate(_markPrefab, _compass);
        newMark.GetComponentInChildren<TextMeshProUGUI>().text = name;
        _data.Marks.Add(newMark, _player.position);
        _data.MarksObjects.Add(newMark);
    }

    private void Update()
    {
        foreach(GameObject mark in _data.MarksObjects)
        {
            mark.transform.rotation = Quaternion.Euler(Vector3.forward*_rotation
                                                                        .Rotate(
                                                                                _player.position,
                                                                                _data.Marks[mark],
                                                                                -90)
                                                    );
        }
    }
}
