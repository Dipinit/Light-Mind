using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class LevelItem : MonoBehaviour
    {
        public string Filename;
        public bool IsInteractable;
        public bool IsCustom;

        public void Select()
        {
            Debug.LogWarning(string.Format("Is interactable: {0}", IsInteractable));
            if (!IsInteractable)
            {
                return;
            }

            PlayerPrefs.SetString("currentLevel", Filename);
            PlayerPrefs.SetInt("currentLevelIsCustom", IsCustom ? 1 : 0);
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