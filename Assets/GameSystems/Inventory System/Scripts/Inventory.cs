using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private GameEvent _onItemChanged;
    [SerializeField] private HungerStat hungerStat;
    [SerializeField] private WarmthStat warmthStat;
    [SerializeField] private WillpowerStat willStat;
    public InventorySlot[] inventorySlots = new InventorySlot[3];

    public void AddItem(BaseItem baseItem, int amount)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].baseItem == null)
            {
                inventorySlots[i].baseItem = baseItem;
                inventorySlots[i].amount = amount;
                _onItemChanged.Raise();
                break;
            }
        }
    }

    public void RemoveItem(int index)
    {
        if (inventorySlots[index].amount == 0)
            return;
        
        inventorySlots[index].baseItem = null;
        inventorySlots[index].amount = 0;
        _onItemChanged.Raise();
    }
    
    public void UnloadItems()
    {
        bool foundItem = false;
        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].amount == 0)
            {
                continue;
            }
            
            var restoreValue = inventorySlots[i].baseItem.RestoreValue;
            var restoreTempValue = inventorySlots[i].baseItem.TempRestoreValue;

            foundItem = true;
            
            switch (inventorySlots[i].baseItem.type)
            {
                case ItemType.Food:
                    hungerStat.AddValue(restoreValue);
                    RemoveItem(i);
                    continue;
                case ItemType.Warmth:
                    warmthStat.AddValue(restoreValue);
                    warmthStat.AddTempValue(restoreTempValue);
                    RemoveItem(i);
                    continue;
                case ItemType.Willpower:
                    willStat.AddValue(restoreValue);
                    RemoveItem(i);
                    continue;
                case ItemType.SpecialItem:
                    willStat.AddValue(restoreValue);
                    RemoveItem(i);
                    continue;
            }
        }

        if (foundItem)
        {
            DialogueManager.instance.DisplayRandomSentence();
        }
        
    }

    public void ClearInventory()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].baseItem != null)
            {
                RemoveItem(i);
            }
        }
        _onItemChanged.Raise();
    }

    private void OnValidate()
    {
        _onItemChanged.Raise();
    }
}

[System.Serializable]
public class InventorySlot
{
    public BaseItem baseItem;
    public int amount;

    public InventorySlot(BaseItem baseItem, int amount)
    {
        this.baseItem = baseItem;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}