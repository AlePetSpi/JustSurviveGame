using Assets.Scripts;
using System;
using UnityEngine;

public class SceneUIManager : MonoBehaviour
{
    [SerializeField] private PauseMenuUILogic pauseMenuPanelPrefab;
    [SerializeField] private EndMenuUILogic endMenuPanelPrefab;
    [SerializeField] private Player player;
    [SerializeField] private FinishPoint finishPoint;
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
        _endMenuPanel.LeaveEndScreenMenu += OnLeaveEndScreenPressed;
        if (player != null)
        {
            player.PauseMenuButtonPressed += OnPauseMenuButtonPressed;
        }
        else
        {
            Debug.LogError("Player reference is missing in SceneUIManager.");
        }

        if (finishPoint != null)
        {
            finishPoint.FinishPassed += OnFinishedPassed;
        }
        else
        {
            Debug.LogError("FinishPoint reference is missing in SceneUIManager.");
        }
    }

    private void OnFinishedPassed(object sender, EventArgs e)
    {
        Debug.Log("Finished activated Event");
        EndScreen(EndScreenStatus.WIN_STATUS);
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

    private void OnLeaveEndScreenPressed(object sender, EventArgs e)
    {
        Debug.Log("EndScreen canceled");
        Time.timeScale = _timeScale;
        _endMenuPanel.gameObject.SetActive(false);
    }

    public void EndScreen(EndScreenStatus endScreenStatus)
    {
        _timeScale = Time.timeScale;
        Time.timeScale = 0;
        _pauseMenuPanel.gameObject.SetActive(false);
        _endMenuPanel.gameObject.SetActive(true);
        if (EndScreenStatus.WIN_STATUS.ToString().Equals(endScreenStatus.ToString()))
        {
            PersistentDataManager.EndTitleText = "You Win";
        }
        else
        {
            PersistentDataManager.EndTitleText = "GameOver :(";
        }
    }
}
