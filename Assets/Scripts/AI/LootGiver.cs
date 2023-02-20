using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootGiver : MonoBehaviour
{
    [SerializeField] private LootContainer _loot;
    [SerializeField] private Sprite[] _collectableSprites;
    private Inventory _playerInventory;
    private GameObject _lootSplashImage;

    private void Start()
    {
        GetComponent<Health>().OnDeath += Give;
        _playerInventory = FindObjectOfType<Inventory>();
        _lootSplashImage = Resources.Load<GameObject>("LootImage");
    }

    public void Give(GameObject dead)
    {
        int chance = Random.Range(0, 100);

        List<CollectableObject> loot = new List<CollectableObject>();
        if (chance < _loot.PossibilityOf3)
        {
            loot.AddRange(_loot.LootTypes[2]);
        }
        else if (chance < _loot.PossibilityOf2)
        {
            loot.AddRange(_loot.LootTypes[1]);
        }
        else if (chance < _loot.PossibilityOf1)
        {
            loot.AddRange(_loot.LootTypes[0]);
        }
        _playerInventory.AddObjects(loot);


        if (loot.Count > 0)
        {
            ShowSplashImage(loot[0].GetItemType(), loot.Count, dead.transform.position);
        }
    }

    private void ShowSplashImage(Collectables item, int number, Vector2 position)
    {
        GameObject splashImage = Instantiate(_lootSplashImage);
        splashImage.transform.position = position;
        splashImage.GetComponentInChildren<Image>().sprite = _collectableSprites[(int)item];
        splashImage.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();

        Destroy(splashImage, 1);
    }
}
