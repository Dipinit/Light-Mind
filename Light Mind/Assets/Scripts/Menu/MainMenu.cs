using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
		public void PlayClassicGame()
		{
			SceneManager.LoadScene(1);
		}

		public void PlayTDGame()
		{
			SceneManager.LoadScene("Scenes/GameTD");
		}

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Scenes/MenuScene");
        }

        public void LevelEditor()
        {
            SceneManager.LoadScene("Scenes/JulesEditor");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
} 