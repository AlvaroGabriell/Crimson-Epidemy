using UnityEngine;
using UnityEngine.UI;


public class TabController : MonoBehaviour
{
    public Image[] tabImages; //array para as abas
    public GameObject[] pages; //array para as paginas
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabNo)
    { //Ativar aquela aba+pagina de acordo com o índicie de cada uma. E desativar as outras enquanto uma está aberta
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            
        }
        pages[tabNo].SetActive(true);
        
    }
}
