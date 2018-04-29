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

        /// <summary>
        /// Set the name of the level.
        /// </summary>
        /// <param name="levelName"></param>
        public void SetName(string levelName)
        {
            GetComponentInChildren<Text>().text = levelName;
        }

        /// <summary>
        /// Set the filename of the level.
        /// </summary>
        /// <param name="filename"></param>
        public void SetFilename(string filename)
        {
            Filename = filename;
        }

        /// <summary>
        /// Set the level as loadable.
        /// </summary>
        /// <param name="interactable"></param>
        public void SetInteractable(bool interactable)
        {
            var button = GetComponentInChildren<Button>();
            IsInteractable = interactable;
            button.interactable = IsInteractable;
        }
    }
}