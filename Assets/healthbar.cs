using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    [SerializeField]
    private DogStats dogStats;

    [SerializeField]
    private Image hpBar;

// Start is called before the first frame update
    void Start()
    {
        hpBar.fillAmount = dogStats.CurrentHealth / 10f;
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = dogStats.CurrentHealth / 10f;
    }
}
