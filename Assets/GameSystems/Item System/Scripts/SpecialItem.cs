using UnityEngine;

[CreateAssetMenu (fileName = "New SpecialItem", menuName = "Item/SpecialItem")]
public class SpecialItem : BaseItem
{
    private void Awake()
    {
        type = ItemType.SpecialItem;
    }
}
