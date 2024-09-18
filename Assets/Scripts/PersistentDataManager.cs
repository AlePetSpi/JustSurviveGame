using System;
using UnityEngine;

public static class PersistentDataManager
{
    private const string HealthKey = "Health";
    private const string ShildKey = "Shild";

    public static event EventHandler DataChangedEvent;
    private static void OnDataChanged()
    {
        DataChangedEvent?.Invoke(null, EventArgs.Empty);
    }

    public static float Health
    {
        get => PlayerPrefs.GetFloat(HealthKey, 100);
        set
        {
            PlayerPrefs.SetFloat(HealthKey, value);
            OnDataChanged();
        }
    }

    public static float Shild
    {
        get => PlayerPrefs.GetFloat(ShildKey, 20);
        set
        {
            PlayerPrefs.SetFloat(ShildKey, value);
            OnDataChanged();
        }

    }

}
