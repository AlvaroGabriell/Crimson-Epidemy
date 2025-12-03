using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
    private int enemyCount = 0;
    public bool isPaused = false, gameStarted = false;

    // ----- Generic -----

    void Awake()
    {
        // Inicializa o singleton
        if (Instance == null)
        {
            Instance = this;
            if(!IsSceneLoaded("Principal")) SceneManager.LoadScene("Principal", LoadSceneMode.Additive);
            DontDestroyOnLoad(gameObject); // Mantém o objeto entre as cenas
        }
        else
        {
            Destroy(gameObject); // Garante que apenas uma instância exista
        }
    }

    void Start()
    {
        FirstStart();



        EnemyBehaviour.OnEnemyDeath += OnEnemyDeath;
        EnemyBehaviour.OnEnemySpawn += OnEnemySpawned;
    }

    public void FirstStart()
    {
        gameStarted = false;
        Utils.GetPlayer().GetComponent<PlayerInput>().actions.FindActionMap("Player").Disable();
        Utils.GetPlayer().GetComponent<PlayerRangedAttack>().canShoot = false;
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

    public void PauseGame()
    {
        Time.timeScale = 0f; // Pausa o jogo
        isPaused = true; // Atualiza o estado de pausa
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Retoma o jogo
        isPaused = false; // Atualiza o estado de pausa
    }

    public void StartGame()
    {
        Utils.GetPlayer().GetComponent<PlayerRangedAttack>().canShoot = true;
        Utils.GetPlayer().GetComponent<PlayerInput>().actions.FindActionMap("Player").Enable();
    }

    public void RestartGame()
    {
        // TODO: Lógica de Restart.
    }

    // ----- Enemy Methods -----
    public void OnEnemySpawned(EnemyBehaviour enemy)
    {
        enemyCount++;
    }
    public void OnEnemyDeath(EnemyBehaviour enemy)
    {
        enemyCount--;
    }

    public void SetEnemyCount(int enemyCount)
    {
        this.enemyCount = enemyCount;
    }
    public int GetEnemyCount()
    {
        return enemyCount;
    }
}
