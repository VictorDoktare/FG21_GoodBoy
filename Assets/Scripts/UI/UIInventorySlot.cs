using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private KeyCode inputKey;
    [SerializeField] private Image icon;
    
    private BaseItem item;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(inputKey))
        {
            switch (inputKey)
            {
                case KeyCode.Alpha1:
                    DropItem(0);
                    ClearItemFromSlot(0);
                    break;
                case KeyCode.Alpha2:
                    DropItem(1);
                    ClearItemFromSlot(1);
                    break;
                case KeyCode.Alpha3:
                    DropItem(2);
                    ClearItemFromSlot(2);
                    break;
            }
        }
    }

    public void AddItemToSlot(BaseItem newItem)
    {
        if (newItem == null)
        {
            return;
        }
        item = newItem;
        icon.sprite = item.itemIcon;
        icon.enabled = true;
    }

    public void DropItem(int index)
    {
        if (inventory.inventorySlots[index].amount != 0)
        {
            var playerPos = GameObject.FindWithTag("Player").transform;
            List<Vector3> dropPositions = new List<Vector3>();
            dropPositions.Add(playerPos.position + playerPos.forward*0.25f - playerPos.up * 0.5f);
            dropPositions.Add(playerPos.position + playerPos.right*0.25f - playerPos.up * 0.5f);
            dropPositions.Add(playerPos.position - playerPos.right*0.25f - playerPos.up * 0.5f);
            
            var itemObj = Instantiate(inventory.inventorySlots[index].baseItem.prefab,dropPositions[index], Quaternion.identity);
            itemObj.GetComponent<Item>().IsDroped = true;
            GameManager.Instance.DroppedItems.Add(itemObj);
        }
    }

    public void ClearItemFromSlot(int index)
    {
        inventory.RemoveItem(index);
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
