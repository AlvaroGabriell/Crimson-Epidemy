using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Utils
{
    // ----- Others -----
    private static List<GameObject> groups = new();

    // ----- UI -----
    private static GameObject MainMenu;

    // ----- Player -----
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

    public static GameObject GetMainMenu()
    {
        if (MainMenu.IsUnityNull()) MainMenu = GameObject.Find("MenuPrincipal");
        return MainMenu;
    }

    public static bool TryGetGroupByName(string name, out GameObject group)
    {
        group = null;
        foreach (var g in groups)
        {
            if(g.name == name)
            {
                if (g.IsUnityNull())
                {
                    groups.Remove(g);
                } else
                {
                    group = g;
                    return true;
                }
            }
        }

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Group");

        foreach (GameObject obj in objs)
        {
            if (obj.name == name)
            {
                groups.Add(obj);
                group = obj;
                return true;
            }
        }

        return false;
    }
}
