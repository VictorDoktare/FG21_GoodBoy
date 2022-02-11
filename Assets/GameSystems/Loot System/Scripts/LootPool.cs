using UnityEngine;
public enum DropRarity
{
    Common,
    Rare,
    Legendary
}

[CreateAssetMenu(menuName = "Loot/Loot Pool")]
public class LootPool : ScriptableObject
{
    [SerializeField]
    private BaseItem[] poolItems;

    public GameObject GetRandomDrop()
    {
        if (poolItems.Length == 0) Debug.LogError("Pool is empty when trying to get random drop!", this);
        return poolItems[Random.Range(0, poolItems.Length)].prefab;
    }
}
