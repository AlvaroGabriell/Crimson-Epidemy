using Unity.VisualScripting;
using UnityEngine;

public class Utils : MonoBehaviour
{
    private static GameObject player;

    /** <summary> 
    * Função pra pegar o jogador independente da cena, caso exista.
    * Também atualiza a referência caso o jogador seja recriado.
    * </summary> **/
    public static GameObject GetPlayer()
    {
        if (player.IsUnityNull()) player = GameObject.FindGameObjectWithTag("Player");
        return player;
    }
}
