using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndMenuUILogic : MonoBehaviour
{

    private const string EndTitleLableName = "EndTitle";
    private const string NextButtonName = "NextButton";
    private const string RestartButton = "RestartButton";
    private const string StartMenuButtonName = "StartMenuButton";

    private UIDocument _endMenuUIDocument;

    public event EventHandler LeaveEndScreenMenu;

    protected virtual void OnLeaveEndScreenButtonPressed()
    {
        LeaveEndScreenMenu?.Invoke(this, EventArgs.Empty);
    }

    void OnEnable()
    {
        _endMenuUIDocument = GetComponent<UIDocument>();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (_endMenuUIDocument == null)
        {
            Debug.LogError("Start Menu UI Document is not found");
            enabled = false;
            return;
        }
        _endMenuUIDocument.rootVisualElement.Q<Label>(EndTitleLableName).text = PersistentDataManager.EndTitleText;
        _endMenuUIDocument.rootVisualElement.Q<Button>(RestartButton).clicked += () =>
        {
            Debug.Log("RestartButton Button Pressed");
            OnLeaveEndScreenButtonPressed();
            SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
        };
        _endMenuUIDocument.rootVisualElement.Q<Button>(NextButtonName).SetEnabled(false);
        Debug.Log($"Count Scenes: {SceneManager.sceneCountInBuildSettings}");
        if (SceneManager.sceneCountInBuildSettings > currentSceneIndex + 1)
        {
            _endMenuUIDocument.rootVisualElement.Q<Button>(NextButtonName).SetEnabled(true);
            _endMenuUIDocument.rootVisualElement.Q<Button>(NextButtonName).clicked += () =>
            {
                Debug.Log($"Next Button Pressed next Scene index: {currentSceneIndex + 1}");
                OnLeaveEndScreenButtonPressed();
                SceneManager.LoadScene(currentSceneIndex + 1);
            };
        }

        _endMenuUIDocument.rootVisualElement.Q<Button>(StartMenuButtonName).clicked += () =>
        {
            Debug.Log("Start Menu Button Pressed");
            OnLeaveEndScreenButtonPressed();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        };

    }
}
