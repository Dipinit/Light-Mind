using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class LevelSelection : MonoBehaviour
    {
        public void Select(string levelName)
        {
            PlayerPrefs.SetString("currentLevel", levelName);
            SceneManager.LoadScene("Scenes/Game");
        }
    }
}