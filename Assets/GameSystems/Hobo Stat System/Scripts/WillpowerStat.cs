using UnityEngine;

[CreateAssetMenu(fileName = "New HoboStat", menuName = "StatSystem/HoboStat/WillpowerStat")]
public class WillpowerStat : BaseStat
{
    private void Awake()
    {
        type = StatType.Willpower;
    }
}
