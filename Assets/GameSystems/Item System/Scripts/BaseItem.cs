using UnityEngine;

public abstract class BaseItem : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    //public BaseStat stat;
    public Sprite itemIcon;
    [TextArea(5, 5)] public string itemDescription;
    [Header("Item Settings")]
    [SerializeField][Range(0,100)] protected int _restoreAmount;
    [SerializeField][Range(0,100)] protected int _tempRestoreAmount;
    
    public int RestoreValue => _restoreAmount;
    public int TempRestoreValue => _tempRestoreAmount;
}

public enum ItemType
{
    Food,
    Warmth,
    Willpower,
    SpecialItem
}
