using UnityEngine;

public class Shield : MonoBehaviour
{
    private int activeTimeSeconds;
    private int coolDownTimeSeconds;

    public int ActiveTimeSeconds { get => activeTimeSeconds; set => activeTimeSeconds = value; }
    public int CoolDownTimeSeconds { get => coolDownTimeSeconds; set => coolDownTimeSeconds = value; }

}
