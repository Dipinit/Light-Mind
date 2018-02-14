using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}