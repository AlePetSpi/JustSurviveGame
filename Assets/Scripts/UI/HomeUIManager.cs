using System;
using UnityEngine;

public class HomeUIManager : MonoBehaviour
{
    [SerializeField] private StartMenuUILogic startMenuPanelPrefab;
    [SerializeField] private PauseMenuUILogic pauseMenuPanelPrefab;
    private StartMenuUILogic _startMenuPanel;
    private PauseMenuUILogic _settingPanel;
    private float _timescale;

    private void Awake()
    {
        _startMenuPanel = Instantiate(startMenuPanelPrefab, transform);
        _settingPanel = Instantiate(pauseMenuPanelPrefab, transform);
    }

    private void Start()
    {
        _settingPanel.gameObject.SetActive(false);
        _settingPanel.LeavePauseMenu += OnLeaveEditorMenu;
    }


    private void OnLeaveEditorMenu(object sender, EventArgs e)
    {
        _startMenuPanel.gameObject.SetActive(true);
        _settingPanel.gameObject.SetActive(false);
    }

    private void ScenePauseMenu(object sender, EventArgs e)
    {
        //pause
        _timescale = Time.timeScale;
        Time.timeScale = 0;
        _startMenuPanel.gameObject.SetActive(true);
        _settingPanel.gameObject.SetActive(false);
        //unpause
        Time.timeScale = _timescale;
    }

}
