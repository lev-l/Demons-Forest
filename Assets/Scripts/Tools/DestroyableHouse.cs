using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DestroyableHouse : MonoBehaviour
{
    public string Hesh;
    [SerializeField] private Sprite _destroyedSprite;
    [SerializeField] private GameObject _chest;

    private void Awake()
    {
        Hesh = gameObject.name;
    }

    public void DestroyThis()
    {
        GetComponent<SpriteRenderer>().sprite = _destroyedSprite;
        Instantiate(_chest, transform.position, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)));
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<ShadowCaster2D>());
        FindObjectOfType<VillageBossLocationSaver>().AddDestroyed(Hesh);
    }
}
