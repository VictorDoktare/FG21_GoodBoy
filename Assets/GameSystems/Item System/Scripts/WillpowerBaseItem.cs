using UnityEngine;

[CreateAssetMenu (fileName = "New FoodItem", menuName = "Item/WillpowerItem")]
public class WillpowerBaseItem : BaseItem
{
    private void Awake()
    {
        type = ItemType.Willpower;
    }
}