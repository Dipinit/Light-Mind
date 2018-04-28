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

        /// <summary>
        /// Continue to the next level.
        /// </summary>
  		public void Play()
  		{
		    PlayerPrefs.SetString("currentLevel", String.Format("level{0}", PlayerPrefs.GetInt("levelReached", 0)));
  			SceneManager.LoadScene("Scenes/Game");
  		}

        /// <summary>
        /// Continue to next level.
        /// </summary>
  		public void NextLevel()
  		{
            var nextLevel = Int32.Parse(PlayerPrefs.GetString ("currentLevel").Substring (5)) + 1;
            PlayerPrefs.SetString("currentLevel", String.Format("level{0}", nextLevel));
  			SceneManager.LoadScene("Scenes/Game");
  		}

        /// <summary>
        /// Play the level again.
        /// </summary>
        public void ReplayLevel()
  		{
  			SceneManager.LoadScene("Scenes/Game");
  		}

        /// <summary>
        /// Back to the main menu.
        /// </summary>
        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Scenes/Menu/MainMenu");
        }

        /// <summary>
        /// Load the level editor.
        /// </summary>
        public void LevelEditor()
        {
            SceneManager.LoadScene("Scenes/Editor");
        }

        /// <summary>
        /// Quit the game :'(
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }

        public void OnClick()
        {
            source.PlayOneShot(click);
        }

        /// <summary>
        /// Get back to the level.
        /// </summary>
        public void Resume()
        {
            GameManager.Instance.PauseLevel ();
        }
    }
}
