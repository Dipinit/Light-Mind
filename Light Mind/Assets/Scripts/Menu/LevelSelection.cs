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
            /*
        Scene scene = SceneManager.GetSceneByName("Game");
        foreach (var gameObject in scene.GetRootGameObjects())
        {
            GameManager gameManager = gameObject.GetComponent<GameManager>();
            if (gameManager != null)
            {
                gameManager.LoadLevel(levelName);
                break;
            }
        }
        */
        }
    }
}