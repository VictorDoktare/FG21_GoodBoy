using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Transform itemSlots;

    private UIInventorySlot[] _slots;

    private void Start()
    {
        _slots = itemSlots.GetComponentsInChildren<UIInventorySlot>();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < inventory.inventorySlots.Length)
            {
                if (inventory.inventorySlots[i].amount == 0)
                {
                    _slots[i].ClearItemFromSlot(i);
                }
                else
                {
                    _slots[i].AddItemToSlot(inventory.inventorySlots[i].baseItem);
                }
            }
        }
    }
}
