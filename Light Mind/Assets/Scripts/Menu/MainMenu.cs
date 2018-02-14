﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Scenes/MenuScene");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
} 