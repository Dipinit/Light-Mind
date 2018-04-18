using System;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public AudioSource source;
        public AudioClip click;
        public AudioClip hover;

  		public void Play()
  		{
		    PlayerPrefs.SetString("currentLevel", String.Format("level{0}", PlayerPrefs.GetInt("levelReached", 0)));
  			SceneManager.LoadScene("Scenes/Game");
  		}
  		public void NextLevel()
  		{
            var nextLevel = Int32.Parse(PlayerPrefs.GetString ("currentLevel").Substring (5)) + 1;
            PlayerPrefs.SetString("currentLevel", String.Format("level{0}", nextLevel));
  			SceneManager.LoadScene("Scenes/Game");
  		}

  		public void ReplayLevel()
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

        public void OnClick()
        {
            source.PlayOneShot(click);
        }
    }
}
