using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void IniciarApp()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

public class BotonBack : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("MenuPrincipal");
    }
}