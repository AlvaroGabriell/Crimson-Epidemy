using UnityEngine;

public class UIHelper : MonoBehaviour
{
    [SerializeField] private GameObject GameWinScreen, GameLostScreen;

    void Awake()
    {
        GameController.OnGameWon += OnGameWon;
        GameController.OnGameLost += OnGameLost;
    }

    void OnDestroy()
    {
        GameController.OnGameWon -= OnGameWon;
        GameController.OnGameLost -= OnGameLost;
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
}
