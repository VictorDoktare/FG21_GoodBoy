using UnityEngine;

[CreateAssetMenu(menuName = "Level Manager/Level Data", fileName = "New LevelData")]
public class GameData : ScriptableObject
{
    [Header("Stats")]
    public HungerStat hunger;
    public WarmthStat warmth;
    public WillpowerStat willpower;
    [Header("Inventory")]
    public Inventory inventory;

    private float _savedHunger;
    private float _savedWarmth;
    private float _savedTempWarmth;
    private float _savedWillpower;
    
    public void ResetStatData()
    {
        Debug.Log("Reset Stat Data");
        // hunger.ResetStat();
        // warmth.ResetStat();
        // willpower.ResetStat();

        hunger.SetValue(_savedHunger);
        warmth.SetValue(_savedWarmth);
        warmth.SetTempValue(_savedTempWarmth);
        willpower.SetValue(_savedWillpower);
    }

    public void ResetInventoryData()
    {
        Debug.Log("Reset Inventory Data");
        inventory.ClearInventory();
    }

    public void SaveStats()
    {
        Debug.Log("Saving Current Stat Data");
        _savedHunger = hunger.Value;
        _savedWarmth = warmth.Value;
        _savedTempWarmth = warmth.TempValue;
        _savedWillpower = willpower.Value;
    }
}
