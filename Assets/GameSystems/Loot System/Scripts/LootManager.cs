using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LootManager : MonoBehaviour
{
    [Header("Food Loot")] [SerializeField] private LootCategory foodLoot;

    [Header("Warmth Loot")] [SerializeField]
    private LootCategory warmthLoot;

    [Header("Willpower Loot")] [SerializeField]
    private LootCategory willpowerLoot;

    [SerializeField] private List<GameObject> resourceDropPoints; //= new List<ResourceDropPoint>();

    private static LootManager instance;

    public static LootManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var obj in resourceDropPoints)
        {
            if (obj.active)
            {
                var dropPoint = obj.GetComponent<ResourceDropPoint>();
                SpawnDrop(dropPoint);
            }
        }
    }

    public void SpawnDrop(ResourceDropPoint dropPoint)
    {
        GameObject dropPrefab;
        switch (dropPoint.DropType)
        {
            case DropType.Food:
                dropPrefab = foodLoot.GetLootOfRarity(DropRarity.Common);
                //dropPrefab = foodLoot
                break;
            case DropType.Warmth:
                dropPrefab = warmthLoot.GetLootOfRarity(DropRarity.Common);
                break;
            case DropType.Willpower:
                dropPrefab = willpowerLoot.GetLootOfRarity(DropRarity.Common);
                break;
            default:
                dropPrefab = foodLoot.GetLootOfRarity(DropRarity.Common);
                break;
        }

        Instantiate(dropPrefab, dropPoint.transform.position, Quaternion.identity);
    }


    // public void AddDropPoint(ResourceDropPoint drop)
    // {
    //     resourceDropPoints.Add(drop);
    // }
}