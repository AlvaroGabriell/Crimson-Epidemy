using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
    public bool isPaused = false, gameStarted = false;

    public int enemyCount = 0, killedEnemies = 0;

    public float RoundTime { get; private set; } = 600f;
    public static readonly float MaxRoundTime = 600f;
    public bool IsTimerRunning { get; private set; } = false;

    public static event Action OnGameStarted;
    public static event Action OnGameWon;
    public static event Action OnGameLost;
    public static event Action OnTimerFinished;



    // ----- Generic -----
    void Awake()
    {
        // Inicializa o singleton
        if (Instance == null)
        {
            Instance = this;
            if(!IsSceneLoaded("Game")) SceneManager.LoadScene("Game", LoadSceneMode.Additive);
            DontDestroyOnLoad(gameObject); // Mantém o objeto entre as cenas
        }
        else
        {
            Destroy(gameObject); // Garante que apenas uma instância exista
        }
    }

    void Start()
    {
        BootGame();

        EnemyBehaviour.OnEnemyDeath += OnEnemyDeath;
        EnemyBehaviour.OnEnemySpawn += OnEnemySpawned;

        PlayerController.OnPlayerDeath += OnPlayerDeath;
    }

    void OnDestroy()
    {
        EnemyBehaviour.OnEnemyDeath -= OnEnemyDeath;
        EnemyBehaviour.OnEnemySpawn -= OnEnemySpawned;
    }

    void Update()
    {
        if(IsTimerRunning && !isPaused) RoundTime -= Time.deltaTime;

        if(RoundTime <= 0f) FinishTime();
    }

    public bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
            {
                return true;
            }
        }
        return false;
    }



    // ----- Game management -----
    public void BootGame()
    {
        gameStarted = false; IsTimerRunning = false;
        Utils.GetPlayer().GetComponent<PlayerInput>().actions.FindActionMap("Player").Disable();
        Utils.GetPlayer().GetComponent<PlayerRangedAttackController>().canShoot = false;
        Utils.GetMainMenu().GetComponent<MainMenu>().OpenMainMenu();

        MusicManager.Instance.PlayMusic(MusicManager.Instance.musicLibrary.GetMusicByName("msc_ce_menu"));
    }

    public void StartGame()
    {
        Utils.GetPlayer().GetComponent<PlayerRangedAttackController>().canShoot = true;
        Utils.GetPlayer().GetComponent<PlayerInput>().actions.FindActionMap("Player").Enable();
        StartTimer();
        gameStarted = true;
        OnGameStarted?.Invoke();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        PauseTimer();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        ResumeTimer();
    }

    public void RestartGame()
    {
        if(isPaused) ResumeGame();
        enemyCount = 0;
        PauseTimer();
        ResetTimer();
        killedEnemies = 0;
        MusicManager.Instance.currentMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        SFXManager.Instance.StopAllRunningSFX();

        StartCoroutine(ReloadPrincipalScene());

        Debug.Log("Game Restarted!");
    }

    IEnumerator ReloadPrincipalScene()
    {
        yield return SceneManager.UnloadSceneAsync("Game");

        yield return SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        BootGame();
    }



    // ----- Enemy Management -----
    public void OnEnemySpawned(EnemyBehaviour enemy)
    {
        enemyCount++;
    }
    public void OnEnemyDeath(EnemyBehaviour enemy, DamageSource source)
    {
        enemyCount--;
        killedEnemies++;
    }



    // ----- Enemy Management -----
    private void OnPlayerDeath()
    {
        PauseTimer();
        OnGameLost?.Invoke();
    }



    // ----- Timing -----
    public void StartTimer()
    {
        RoundTime = 600f;
        IsTimerRunning = true;
    }

    public void PauseTimer()
    {
        IsTimerRunning = false;
    }
    public void ResumeTimer()
    {
        IsTimerRunning = true;
    }

    public void ResetTimer()
    {
        RoundTime = 600;
    }
    public void FinishTime()
    {
        PauseTimer();
        RoundTime = 0f;
        OnTimerFinished?.Invoke();
    }
}
