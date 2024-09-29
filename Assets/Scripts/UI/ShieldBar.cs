using UnityEngine;
using UnityEngine.UI;

public class ShieldBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetMaxShield(int maxShield)
    {
        slider.maxValue = maxShield;
        slider.value = maxShield;
    }

    public void SetShield(int shield)
    {
        slider.value = shield;
    }
}
