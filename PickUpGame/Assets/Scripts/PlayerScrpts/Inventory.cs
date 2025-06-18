using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 1;
    [SerializeField]private List<Pickupable> items = new List<Pickupable>();

    public bool AddItem(Pickupable item)
    {
        if (items.Count >= maxSlots)
        {
            Debug.Log("Inventory full");
            return false;
        }
        items.Add(item);
        item.gameObject.SetActive(false);
        return true;
    }

    public Pickupable RetrieveItem()
    {
        if (items.Count == 0)
        {
            Debug.Log("Inventory empty");
            return null;
        }
        Pickupable item = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);
        item.gameObject.SetActive(true);
        return item;
    }
}
