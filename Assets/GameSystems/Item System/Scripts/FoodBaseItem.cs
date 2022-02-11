using UnityEngine;

[CreateAssetMenu (fileName = "New FoodItem", menuName = "Item/FoodItem")]
public class FoodBaseItem : BaseItem
{
        private void Awake()
        {
                type = ItemType.Food;
        }
}