using UnityEngine.SceneManagement;
using UnityEngine;

public class Button_Func_Script : MonoBehaviour
{
    [SerializeField]
    private GameObject Layer_Main_Menu;

    public void Button_Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Button_Start()
    {
        Layer_Main_Menu.SetActive(false);
    }
}
