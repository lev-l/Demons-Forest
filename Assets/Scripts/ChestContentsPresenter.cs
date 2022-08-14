using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestContentsPresenter : MonoBehaviour
{
    [SerializeField] private GameObject _itemsCanvas;
    [SerializeField] private Sprite _healBottleSprite,
                                    _throwingKnifeSprite,
                                    _staticTorchSprite;
    private GameObject _itemViewPrefab;
    private Transform _itemsPanel;
    private List<GameObject> _itemsViews;

    public void SetCanvas(Canvas canvas)
    {
        _itemsCanvas = canvas.gameObject;
        _itemsPanel = _itemsCanvas.transform.GetChild(0);
        _itemsCanvas.SetActive(false);
    }

    private void Start()
    {
        if(_itemsCanvas == null)
        {
            throw new System.Exception("Canvas wasnot defined");
        }
        _itemViewPrefab = Resources.Load<GameObject>("ItemView");
        _itemsViews = new List<GameObject>(3);
    }

    public void ShowItems(List<CollectableObject> items)
    {
        //foreach instead of if
        foreach(CollectableObject item in items)
        {
            Dictionary<Collectables, int> sortedItems = SortTypeToNumber(items);

            ShowItem(_healBottleSprite, sortedItems[Collectables.HealBottle], new Vector2(0, 60));
            ShowItem(_throwingKnifeSprite, sortedItems[Collectables.ThrowingKnife], new Vector2(0, 0));
            ShowItem(_staticTorchSprite, sortedItems[Collectables.StaticTorch], new Vector2(0, -60));
            _itemsCanvas.SetActive(true);
            StartCoroutine(nameof(HideView));

            break;
        }
    }

    private void ShowItem(Sprite itemSprite,
                                int number,
                                Vector2 position)
    {
        if (number > 0)
        {
            GameObject itemView = Instantiate(_itemViewPrefab, _itemsPanel);
            itemView.GetComponent<Image>().sprite = itemSprite;
            itemView.GetComponentInChildren<Text>().text = number.ToString();
            itemView.transform.localPosition = position;

            _itemsViews.Add(itemView);
        }
    }

    private Dictionary<Collectables, int> SortTypeToNumber(List<CollectableObject> items)
    {
        Dictionary<Collectables, int> typeToNumber = new Dictionary<Collectables, int>();
        typeToNumber.Add(Collectables.HealBottle, 0);
        typeToNumber.Add(Collectables.ThrowingKnife, 0);
        typeToNumber.Add(Collectables.StaticTorch, 0);

        foreach(CollectableObject item in items)
        {
            typeToNumber[item.GetItemType()]++;
        }

        return typeToNumber;
    }

    private IEnumerator HideView()
    {
        yield return new WaitForSeconds(1);
        _itemsCanvas.SetActive(false);

        // not deleting. find why
        foreach(GameObject item in _itemsViews)
        {
            Destroy(item);
        }
    }
}
