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
            Debug.Log("Start Button Pressed");
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

        currentVehicleInstance = Instantiate(vehiclePrefab[_tempVehicleIndex].gameObject);
    }

    private void FixedUpdate()
    {
        if (vehiclePrefab == null) return;
        int vehicleNr = _startMenuUIDocument.rootVisualElement.Q<DropdownField>(VehicleSelectorName).index;
        if (vehicleNr != _tempVehicleIndex)
        {
            Debug.Log($"vehicleNr: {vehicleNr}");
            Debug.Log($"currentVehicleInstance: {currentVehicleInstance}");

            if (currentVehicleInstance != null)
            {
                Destroy(currentVehicleInstance);
            }
            currentVehicleInstance = Instantiate(vehiclePrefab[vehicleNr].gameObject);
            PersistentDataManager.VehicleId = vehicleNr;
            _tempVehicleIndex = vehicleNr;
        }
    }

}
