using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {//BuildIndex - 0 = Main Menu; 1 = Game;
        //TO DO: Antes de lançar o jogo, alterar código conforme instruções acima:
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        //SceneManager.LoadScene(0); // -> Álvaro: --; Explicação abaixo.

        //-> Álvaro
        GameController.Instance.StartGame();
        gameObject.SetActive(false);
        /**
        * O jogo é iniciado no GameController. A cena iniciada por padrão pela Unity é a UI, e logo em seguida o script GameController
        * dá load de forma aditiva na cena Principal, que contêm o jogo em si, o Player, Inputs, a câmera e etc... Portanto a cena Principal
        * sempre deve estar carregada.
        **/
    }
    
    public void QuitGame(){
        //Fechar a aplicação;
        Debug.Log("Game is now closed"); // Debug apenas para questões de testes
        Application.Quit();
    }
}
