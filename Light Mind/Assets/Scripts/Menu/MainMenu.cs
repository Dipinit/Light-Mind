using UnityEngine;
using UnityEngine.SceneManagement;
namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
  		public void Play()
  		{
  			SceneManager.LoadScene("Scenes/Game");
  		}

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Scenes/Menu/MainMenu");
        }

        public void LevelEditor()
        {
            SceneManager.LoadScene("Scenes/Editor");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
