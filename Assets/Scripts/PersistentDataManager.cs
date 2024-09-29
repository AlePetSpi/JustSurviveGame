using System;
using UnityEngine;

public static class PersistentDataManager
{
    private const string VehicleIdKey = "VehicleId";
    private const string MaxHealthKey = "MaxHealth";
    private const string CurrentHealthKey = "CurrentHealth";
    private const string MaxShieldKey = "MaxShield";
    private const string CurrentShieldKey = "CurrentShield";
    private const string PowerKey = "Power";
    private const string SteeringKey = "Steering";
    private const string EndTitleLable = "EndTitle";

    public static event EventHandler DataChangedEvent;
    private static void OnDataChanged()
    {
        DataChangedEvent?.Invoke(null, EventArgs.Empty);
    }

    public static int VehicleId
    {
        get => PlayerPrefs.GetInt(VehicleIdKey, 0);
        set
        {
            PlayerPrefs.SetInt(VehicleIdKey, value);
            OnDataChanged();
        }
    }

    public static int MaxHealth
    {
        get => PlayerPrefs.GetInt(MaxHealthKey, 100);
        set
        {
            PlayerPrefs.SetInt(MaxHealthKey, value);
            OnDataChanged();
        }
    }

    public static int MaxShield
    {
        get => PlayerPrefs.GetInt(MaxShieldKey, 20);
        set
        {
            PlayerPrefs.SetInt(MaxShieldKey, value);
            OnDataChanged();
        }
    }

    public static int CurrentHealth
    {
        get => PlayerPrefs.GetInt(CurrentHealthKey, 100);
        set
        {
            PlayerPrefs.SetInt(CurrentHealthKey, value);
            OnDataChanged();
        }
    }

    public static int CurrentShield
    {
        get => PlayerPrefs.GetInt(CurrentShieldKey, 20);
        set
        {
            PlayerPrefs.SetInt(CurrentShieldKey, value);
            OnDataChanged();
        }
    }

    public static int Power
    {
        get => PlayerPrefs.GetInt(PowerKey, 150);
        set
        {
            PlayerPrefs.SetInt(PowerKey, value);
            OnDataChanged();
        }
    }

    public static int Steering
    {
        get => PlayerPrefs.GetInt(SteeringKey, 20);
        set
        {
            PlayerPrefs.SetInt(SteeringKey, value);
            OnDataChanged();
        }
    }

    public static string EndTitleText
    {
        get => PlayerPrefs.GetString(EndTitleLable, "Test");
        set
        {
            PlayerPrefs.SetString(EndTitleLable, value);
            OnDataChanged();
        }

    }

}
