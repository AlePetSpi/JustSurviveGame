using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuUILogic : MonoBehaviour
{
    private const string BackButtonName = "BackButton";
    private const string ConfirmButtonName = "StartMenuButton";

    public event EventHandler LeavePauseMenu;

    protected virtual void OnLeavePauseButtonPressed()
    {
        LeavePauseMenu?.Invoke(this, EventArgs.Empty);
    }

    private UIDocument _pauseMenuDocument;

    private void OnEnable()
    {
        _pauseMenuDocument = GetComponent<UIDocument>();
        if (_pauseMenuDocument == null)
        {
            Debug.LogError("No UIDocument found on CartEditor object! Disabling CartEditor script.");
            enabled = false;
            return;
        }


        _pauseMenuDocument.rootVisualElement.Q<Button>(ConfirmButtonName).clicked += () =>
        {
            Debug.Log("Start Menu button clicked!");
            OnLeavePauseButtonPressed();
        };

        _pauseMenuDocument.rootVisualElement.Q<Button>(BackButtonName).clicked += () =>
        {
            Debug.Log("Back button clicked!");
            OnLeavePauseButtonPressed();
        };
    }

}
