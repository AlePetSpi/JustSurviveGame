using System;
using UnityEngine;
using UnityEngine.UIElements;

public class OverlayUILogic : MonoBehaviour
{
    private const string PauseMenuButtonName = "PauseMenu";

    public event EventHandler PauseMenuButtonPressed;
    protected virtual void OnPauseMenuButtonPressed()
    {
        PauseMenuButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private UIDocument _overlayDocument;

    private void OnEnable()
    {
        _overlayDocument = GetComponent<UIDocument>();
        if (_overlayDocument == null)
        {
            Debug.LogError("No UIDocument found on OverlayManager object! Disabling OverlayManager script.");
            enabled = false;
            return;
        }
        _overlayDocument.rootVisualElement.Q<Button>(PauseMenuButtonName).clicked += () =>
        {
            Debug.Log("Setting button clicked!");
            OnPauseMenuButtonPressed();
        };
    }
}
