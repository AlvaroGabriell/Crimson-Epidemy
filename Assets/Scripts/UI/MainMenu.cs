using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {//BuildIndex - 0 = Main Menu; 1 = Game;
        //TO DO: Antes de lançar o jogo, alterar código conforme instruções acima:
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(0);
    }
    
    public void QuitGame(){
        //Fechar a aplicação;
        //Debug.Log("Game is now closed"); -- Debug apenas para questões de testes
        Application.Quit();
    }
}
