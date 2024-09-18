using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuUILogic : MonoBehaviour
{

    private const string VehicleSelectorName = "VehicleSelector";
    private const string LevelSelectorName = "LevelSelector";
    private const string StartButtonName = "StartButton";
    private const string EditCartButtonName = "EditCartButton";
    private const string QuitButtonName = "QuitButton";

    private UIDocument _startMenuUIDocument;

    private void OnEnable()
    {
        _startMenuUIDocument = GetComponent<UIDocument>();
        if (_startMenuUIDocument == null)
        {
            Debug.LogError("Main Menu UI Document is not found");
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


        /* _power = PersistentDataManager.Power;
         _steering = PersistentDataManager.Steering;

         _cartEditorDocument = GetComponent<UIDocument>();
         if (_cartEditorDocument == null)
         {
             Debug.LogError("No UIDocument found on CartEditor object! Disabling CartEditor script.");
             enabled = false;
             return;
         }


         SliderInt powerSlider = _cartEditorDocument.rootVisualElement.Q<SliderInt>(PowerSliderName);
         powerSlider.value = (int)_power;
         powerSlider.RegisterValueChangedCallback(evt => { _power = evt.newValue; });

         SliderInt steeringSlider = _cartEditorDocument.rootVisualElement.Q<SliderInt>(SteeringSliderName);
         steeringSlider.value = (int)_steering;
         steeringSlider.RegisterValueChangedCallback(evt => { _steering = evt.newValue; });

         _cartEditorDocument.rootVisualElement.Q<Button>(ConfirmButtonName).clicked += () =>
         {
             Debug.Log("Confirm button clicked!");
             Debug.Log("Power: " + _power + "/ Steering: " + _steering);
             PersistentDataManager.Power = _power;
             PersistentDataManager.Steering = _steering;
             OnLeaveEditorButtonPressed();
         };
         _cartEditorDocument.rootVisualElement.Q<Button>(BackButtonName).clicked += () =>
         {
             Debug.Log("Back button clicked!");
             OnLeaveEditorButtonPressed();
         };*/
    }

}
