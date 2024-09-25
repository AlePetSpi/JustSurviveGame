using System;
using UnityEngine;

public static class PersistentDataManager
{
    private const string VehicleIdKey = "VehicleId";
    private const string HealthKey = "Health";
    private const string ShieldKey = "Shield";
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

    public static int Health
    {
        get => PlayerPrefs.GetInt(HealthKey, 100);
        set
        {
            PlayerPrefs.SetInt(HealthKey, value);
            OnDataChanged();
        }
    }

    public static int Shield
    {
        get => PlayerPrefs.GetInt(ShieldKey, 20);
        set
        {
            PlayerPrefs.SetInt(ShieldKey, value);
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
