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
        if (_startMenuUIDocument == null)
        {
            Debug.LogError("Start Menu UI Document is not found");
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

        Instantiate(vehiclePrefab[_tempVehicleIndex].gameObject);
    }

    private void FixedUpdate()
    {
        int vehicleNr = _startMenuUIDocument.rootVisualElement.Q<DropdownField>(VehicleSelectorName).index;
        if (vehicleNr != _tempVehicleIndex)
        {
            Debug.Log($"vehicleNr: {vehicleNr}");
            Destroy(currentVehicleInstance);
            Instantiate(vehiclePrefab[vehicleNr].gameObject);
            currentVehicleInstance = vehiclePrefab[vehicleNr].gameObject;
            _tempVehicleIndex = vehicleNr;
        }
    }

}
