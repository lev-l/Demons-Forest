using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HotkeyShowing : MonoBehaviour
{
    private GameObject _hotkeyView;
    private Collider2D _collider;
    private List<Collider2D> _overlapResults;
    private ContactFilter2D _filter;
    private GameObject _hotkey;

    private void Start()
    {
        _hotkeyView = Resources.Load<GameObject>("Hotkey");

        _collider = GetComponent<Collider2D>();
        _overlapResults = new List<Collider2D>();
        _filter = new ContactFilter2D();

        _filter.useTriggers = false;
        _filter.useLayerMask = true;
        _filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    private void FixedUpdate()
    {
        int resultsNum = _collider.OverlapCollider(_filter, _overlapResults);

        if(resultsNum > 0
            && _hotkey == null)
        {
            Transform newHotkeyTransform = Instantiate(_hotkeyView, gameObject.transform).transform;
            _hotkey = newHotkeyTransform.gameObject;

            newHotkeyTransform.position += new Vector3(0.6f, 0.6f);
            newHotkeyTransform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else if(resultsNum == 0)
        {
            Destroy(_hotkey);
            _hotkey = null;
        }
    }
}
