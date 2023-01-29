using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompassPresenter : MonoBehaviour
{
    [SerializeField] private Transform _compass;
    [SerializeField] private InputField _input;
    [SerializeField] private MarkDelete _markDelete;
    private Transform _player;
    private GameObject _markPrefab;
    private CompassData _data;
    private GameObject _lastShownText;
    private GameObject _activeMark;
    private RotateToTarget _rotation;
    private bool _markActions;
    private bool _block;

    private void Start()
    {
        _player = FindObjectOfType<PlayerMovement>().transform;
        _markPrefab = Resources.Load<GameObject>("Mark");
        _data = Resources.Load<CompassData>("CompassDataBase");
        _rotation = GetComponent<RotateToTarget>();
        _markActions = false;
        _block = false;
    }

    public void AddAMark()
    {
        GameObject newMark = Instantiate(_markPrefab, _compass);
        string name = _input.text;
        _input.text = "";
        MouseTouched mouseInteraction = newMark.GetComponentInChildren<MouseTouched>();
        mouseInteraction.Text.text = name;
        newMark.GetComponentInChildren<Image>().color
            = new Color(Random.Range(10, 200)/255f, Random.Range(10, 200)/255f, Random.Range(10, 200)/255f);

        _data.Marks.Add(newMark, _player.position);
        _data.MarksObjects.Add(newMark);

        mouseInteraction.OnMouseEnter += Show;
        mouseInteraction.OnMouseEnter += SetReady;
        mouseInteraction.OnMouseExit += UnsetReady;
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

        if (_markActions
            && Input.GetKeyDown(KeyCode.Delete))
        {
            _markDelete.gameObject.SetActive(true);
            _markDelete.GetComponentInChildren<TextMeshProUGUI>(true).text
                = $"Are you sure you want to delete {_activeMark.GetComponentInChildren<TextMeshProUGUI>(true).text}";
            _block = true;
        }
    }

    public void Show(GameObject gObject)
    {
        if (_lastShownText)
        {
            _lastShownText.SetActive(false);
        }

        gObject.SetActive(true);
        _lastShownText = gObject;
    }

    public void SetReady(GameObject gObject)
    {
        if (!_block)
        {
            _markActions = true;
            _activeMark = gObject.transform.parent.parent.gameObject;
        }
    }

    public void UnsetReady()
    {
        if (!_block)
        {
            _markActions = false;
        }
    }

    public void Delete()
    {
        _markDelete.Delete(_activeMark, _data);
        _markActions = false;
        _block = false;
    }

    public void Unblock()
    {
        _block = false;
        _markActions = false;
    }
}
