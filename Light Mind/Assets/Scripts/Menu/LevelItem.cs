using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class LevelItem : MonoBehaviour
    {
        public string Filename;
        public bool IsInteractable;

        public void Select()
        {
            Debug.LogWarning(string.Format("Is interactable: {0}", IsInteractable));
            if (!IsInteractable)
            {
                return;
            }

            PlayerPrefs.SetString("currentLevel", Filename);
            SceneManager.LoadScene("Scenes/Game");
        }

        public void SetName(string levelName)
        {
            GetComponentInChildren<Text>().text = levelName;
        }

        public void SetFilename(string filename)
        {
            Filename = filename;
        }

        public void SetInteractable(bool interactable)
        {
            var button = GetComponentInChildren<Button>();
            IsInteractable = interactable;
            button.interactable = IsInteractable;
        }
    }
}