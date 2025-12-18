using UnityEngine;

public class UIHelper : MonoBehaviour
{
    [SerializeField] private GameObject GameWinScreen, GameLostScreen, PauseScreen;

    void Awake()
    {
        GameController.OnGameWon += OnGameWon;
        GameController.OnGameLost += OnGameLost;
        GameController.OnGamePaused += ShowPauseMenu;
        GameController.OnGameResumed += HidePauseMenu;
    }

    void OnDestroy()
    {
        GameController.OnGameWon -= OnGameWon;
        GameController.OnGameLost -= OnGameLost;
        GameController.OnGamePaused -= ShowPauseMenu;
        GameController.OnGameResumed -= ShowPauseMenu;
    }


    public void OnPlayAgainButton()
    {
        GameController.Instance.RestartGame(false);
    }

    public void OnMainMenuButton()
    {
        GameController.Instance.RestartGame(true);
    }

    private void OnGameWon()
    {
        GameWinScreen.SetActive(true);
    }
    private void OnGameLost()
    {
        GameLostScreen.SetActive(true);
    }

    public void ShowPauseMenu(bool PlayerPaused)
    {
        if(PlayerPaused) PauseScreen.SetActive(true);
    }
    public void HidePauseMenu(bool PlayerResumed)
    {
        if(PlayerResumed) PauseScreen.SetActive(false);
    }
}
