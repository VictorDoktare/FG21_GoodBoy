using UnityEngine;

[CreateAssetMenu(menuName = "Loot/Loot Category")]
public class LootCategory : ScriptableObject
{
    [SerializeField]
    private LootPool CommonPool;

    [SerializeField]
    private LootPool RarePool;
    
    [SerializeField]
    private LootPool LegendaryPool;

    public GameObject GetLootOfRarity(DropRarity dropRarity)
    {
        switch (dropRarity)
        {
            case DropRarity.Common:
                return CommonPool.GetRandomDrop();
            
            case DropRarity.Rare:
                return RarePool.GetRandomDrop();
            
            case DropRarity.Legendary:
                return LegendaryPool.GetRandomDrop();
            
            default:
                #if UNITY_EDITOR
                Debug.LogWarning("Drop rarity defaulted to common.");
                #endif
                return CommonPool.GetRandomDrop();
        }
    }
}
