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
		    if (!PlayerPrefs.HasKey("levelReached")) PlayerPrefs.SetInt("levelReached", 0);
              Debug.Log(PlayerPrefs.GetInt("levelReached"));
		    PlayerPrefs.SetString("currentLevel", String.Format("level{0}", PlayerPrefs.GetInt("levelReached")));
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
