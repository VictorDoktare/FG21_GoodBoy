using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private void OnTriggerEnter(Collider other)
    {
        ItemPickup(other);
        UnloadItems(other);
        
    }
    
    private void ItemPickup(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            var pickupItem = other.GetComponent<Item>();
        
            if (pickupItem && pickupItem.IsDroped == false)
            {
                for (int i = 0; i < _inventory.inventorySlots.Length; i++)
                {
                    if (_inventory.inventorySlots[i].amount == 0)
                    {
                        _inventory.AddItem(pickupItem.item, 1);
                        Destroy(other.gameObject);
                        break;
                    }
                }
            }
        }
    }

    private void UnloadItems(Collider other)
    {
        if (other.CompareTag("Hobo"))
        {
            _inventory.UnloadItems();
        }
    }
    
    public void OnApplicationQuit()
    {
        _inventory.ClearInventory();
    }
}
