using UnityEngine;
using UnityEngine.UI;

public class CompassPresenter : MonoBehaviour
{
    [SerializeField] private Transform _compass;
    [SerializeField] private InputField _input;
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

    public void AddAMark()
    {
        GameObject newMark = Instantiate(_markPrefab, _compass);
        string name = _input.text;
        _input.text = "";
        newMark.GetComponentInChildren<MouseTouched>().Text.text = name;
        newMark.GetComponentInChildren<Image>().color
            = new Color(Random.Range(10, 200), Random.Range(10, 200), Random.Range(10, 200));

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
