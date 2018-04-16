﻿using UnityEngine;
using UnityEngine.SceneManagement;
namespace Menu
{
    public class MainMenu : MonoBehaviour
    {

  		public void PlayTDGame()
  		{
  			Application.LoadLevel("Scenes/GameTD3D");
  		}

        public void ReturnToMenu()
        {
            Application.LoadLevel("Scenes/MenuScene");
        }

        public void LevelEditor()
        {
            Application.LoadLevel("Scenes/EditorTD");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
