using UnityEngine;

[CreateAssetMenu (fileName = "New FoodItem", menuName = "Item/WarmthItem")]
public class WarmthBaseItem : BaseItem
{
    private void Awake()
    {
        type = ItemType.Warmth;
    }
}