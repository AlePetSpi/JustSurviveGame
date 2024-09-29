using Assets.Scripts;
using System;
using UnityEngine;

public class SceneUIManager : MonoBehaviour
{
    [SerializeField] private PauseMenuUILogic pauseMenuPanelPrefab;
    [SerializeField] private EndMenuUILogic endMenuPanelPrefab;
    [SerializeField] private Player player;
    private PauseMenuUILogic _pauseMenuPanel;
    private EndMenuUILogic _endMenuPanel;
    private float _timeScale;

    private void Awake()
    {
        _pauseMenuPanel = Instantiate(pauseMenuPanelPrefab, transform);
        _pauseMenuPanel.gameObject.SetActive(false);
        _endMenuPanel = Instantiate(endMenuPanelPrefab, transform);
        _endMenuPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        _pauseMenuPanel.gameObject.SetActive(false);
        _pauseMenuPanel.LeavePauseMenu += OnLeavePauseMenu;
        if (player != null)
        {
            player.PauseMenuButtonPressed += OnPauseMenuButtonPressed;
        }
        else
        {
            Debug.LogError("Player reference is missing in SceneUIManager.");
        }
    }

    private void OnPauseMenuButtonPressed(object sender, EventArgs e)
    {
        Debug.Log("Pause activated Event");
        _timeScale = Time.timeScale;
        Time.timeScale = 0;
        _pauseMenuPanel.gameObject.SetActive(true);
    }

    private void OnLeavePauseMenu(object sender, EventArgs e)
    {
        Debug.Log("Pause canceled");
        Time.timeScale = _timeScale;
        _pauseMenuPanel.gameObject.SetActive(false);
    }

    public void EndScreen(EndScreenStatus endScreenStatus)
    {
        _pauseMenuPanel.gameObject.SetActive(false);
        _endMenuPanel.gameObject.SetActive(true);
        PersistentDataManager.EndTitleText = "GameOver :(";
    }
}
