using System;
using UnityEngine;

public class HomeUIManager : MonoBehaviour
{
    [SerializeField] private StartMenuUILogic startMenuPanelPrefab;
    [SerializeField] private PauseMenuUILogic pauseMenuPanelPrefab;
    private StartMenuUILogic _startMenuPanel;
    private PauseMenuUILogic _pauseMenuPanel;
    private float _timescale;

    private void Awake()
    {
        _startMenuPanel = Instantiate(startMenuPanelPrefab, transform);
        _pauseMenuPanel = Instantiate(pauseMenuPanelPrefab, transform);
    }

    private void Start()
    {
        _pauseMenuPanel.gameObject.SetActive(false);
        _pauseMenuPanel.LeavePauseMenu += OnLeavePauseMenu;
    }


    private void OnLeavePauseMenu(object sender, EventArgs e)
    {
        _startMenuPanel.gameObject.SetActive(true);
        _pauseMenuPanel.gameObject.SetActive(false);
    }

    private void ScenePauseMenu(object sender, EventArgs e)
    {
        //pause
        _timescale = Time.timeScale;
        Time.timeScale = 0;
        _startMenuPanel.gameObject.SetActive(true);
        _pauseMenuPanel.gameObject.SetActive(false);
        //unpause
        Time.timeScale = _timescale;
    }

}
