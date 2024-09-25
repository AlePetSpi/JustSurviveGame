using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuUILogic : MonoBehaviour
{
    [SerializeField] private Vehicle[] vehiclePrefab;
    private const string VehicleSelectorName = "VehicleSelector";
    private const string LevelSelectorName = "LevelSelector";
    private const string StartButtonName = "StartButton";
    private const string QuitButtonName = "QuitButton";

    private UIDocument _startMenuUIDocument;
    private int _tempVehicleIndex = 0;
    private GameObject currentVehicleInstance;
    private void OnEnable()
    {
        _startMenuUIDocument = GetComponent<UIDocument>();
        if (_startMenuUIDocument == null || vehiclePrefab == null)
        {
            Debug.LogError("UI Document is not found");
            enabled = false;
            return;
        }
        _startMenuUIDocument.rootVisualElement.Q<Button>(StartButtonName).clicked += () =>
        {
            int sceneNr = _startMenuUIDocument.rootVisualElement.Q<DropdownField>(LevelSelectorName).index + 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNr);
        };
        _startMenuUIDocument.rootVisualElement.Q<Button>(QuitButtonName).clicked += () =>
        {
#if !UNITY_EDITOR
            Application.Quit();
#elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        };

        InstantiateVehicle(_tempVehicleIndex);
    }

    private void FixedUpdate()
    {
        if (vehiclePrefab == null) return;
        int vehicleNr = _startMenuUIDocument.rootVisualElement.Q<DropdownField>(VehicleSelectorName).index;
        if (vehicleNr != _tempVehicleIndex)
        {
            if (currentVehicleInstance != null)
            {
                Destroy(currentVehicleInstance);
            }
            InstantiateVehicle(vehicleNr);
            _tempVehicleIndex = vehicleNr;
        }
    }

    private void InstantiateVehicle(int index)
    {
        currentVehicleInstance = Instantiate(vehiclePrefab[index].gameObject);
        Vehicle vehicle = currentVehicleInstance.GetComponent<Vehicle>();
        PersistentDataManager.VehicleId = vehicle.VehicleId;
        PersistentDataManager.Health = vehicle.MaxHealth;
        PersistentDataManager.Shield = vehicle.Shield;
        PersistentDataManager.Power = vehicle.Power;
        PersistentDataManager.Steering = vehicle.Steering;
        PlayerPrefs.Save();
    }

}
