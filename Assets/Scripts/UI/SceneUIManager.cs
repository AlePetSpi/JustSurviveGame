using System;
using UnityEngine;

public class SceneUIManager : MonoBehaviour
{
    [SerializeField] private OverlayUILogic overlayUILogic;
    [SerializeField] private PauseMenuUILogic pauseMenuPanelPrefab;
    private OverlayUILogic _overlayPanel;
    private PauseMenuUILogic _pauseMenuPanel;
    private float _timeScale;

    private void Awake()
    {
        _overlayPanel = Instantiate(overlayUILogic, transform);
        _pauseMenuPanel = Instantiate(pauseMenuPanelPrefab, transform);
    }

    private void Start()
    {
        _pauseMenuPanel.gameObject.SetActive(false);
        _overlayPanel.PauseMenuButtonPressed += OnPauseMenuButtonPressed;
        _pauseMenuPanel.LeavePauseMenu += OnLeavePauseMenu;
    }

    private void OnPauseMenuButtonPressed(object sender, EventArgs e)
    {
        _timeScale = Time.timeScale;
        Time.timeScale = 0;
        _overlayPanel.gameObject.SetActive(false);
        _pauseMenuPanel.gameObject.SetActive(true);
    }

    private void OnLeavePauseMenu(object sender, EventArgs e)
    {
        Time.timeScale = _timeScale;
        _overlayPanel.gameObject.SetActive(true);
        _pauseMenuPanel.gameObject.SetActive(false);
    }

}
