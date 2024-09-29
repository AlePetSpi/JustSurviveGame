using UnityEngine;
using UnityEngine.UIElements;

public class OverlayUILogic : MonoBehaviour
{
    private const string PauseMenuButtonName = "PauseMenu";
    private const string HealthButtonName = "HealthProgress";
    private const string ShieldButtonName = "ShieldProgress";

    /*public event EventHandler PauseMenuButtonPressed;
    protected virtual void OnPauseMenuButtonPressed()
    {
        PauseMenuButtonPressed?.Invoke(this, EventArgs.Empty);
    }*/

    private UIDocument _overlayDocument;

    private void Awake()
    {
        _overlayDocument = GetComponent<UIDocument>();
        if (_overlayDocument == null)
        {
            Debug.LogError("No UIDocument found on OverlayManager object! Disabling OverlayManager script.");
            enabled = false;
            return;
        }
        //_overlayDocument?.rootVisualElement.Q<ProgressBar>(HealthButtonName).style.backgroundColor.value = #ffffff;
        PersistentDataManager.DataChangedEvent += (_, _) => UpdateStatsProperties();
        var healthProgress = _overlayDocument.rootVisualElement.Q<ProgressBar>(HealthButtonName);
        var shieldProgress = _overlayDocument.rootVisualElement.Q<ProgressBar>(HealthButtonName);
        healthProgress.style.unityBackgroundImageTintColor = new StyleColor(Color.red);
        shieldProgress.style.unityBackgroundImageTintColor = new StyleColor(Color.cyan);
    }

    private void UpdateStatsProperties()
    {
        if (_overlayDocument == null) return;
        _overlayDocument?.rootVisualElement.Q<ProgressBar>(HealthButtonName).SetValueWithoutNotify(PersistentDataManager.MaxHealth);
        _overlayDocument?.rootVisualElement.Q<ProgressBar>(ShieldButtonName).SetValueWithoutNotify(PersistentDataManager.MaxShield);
    }

    private void OnEnable()
    {
        if (_overlayDocument == null)
        {
            Debug.LogError("No UIDocument found on OverlayManager object! Disabling OverlayManager script.");
            enabled = false;
            return;
        }
        _overlayDocument.rootVisualElement.Q<Button>(PauseMenuButtonName).clicked += () =>
        {
            Debug.Log("Setting button clicked!");
            //OnPauseMenuButtonPressed();
        };

    }
}
