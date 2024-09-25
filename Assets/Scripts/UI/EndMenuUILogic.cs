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
            SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
        };
        _endMenuUIDocument.rootVisualElement.Q<Button>(NextButtonName).clicked += () =>
        {
            Debug.Log($"Next Button Pressed next Scene index: {currentSceneIndex + 1}");
            //UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        };
        _endMenuUIDocument.rootVisualElement.Q<Button>(StartMenuButtonName).clicked += () =>
        {
            Debug.Log("Start Button Pressed");
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        };

    }
}
