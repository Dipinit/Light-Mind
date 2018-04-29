using UnityEngine;

namespace TD
{
    public class EndPoint : MonoBehaviour
    {
        private AudioSource[] _audioSources;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(string.Format("End triggered by {0}", other.gameObject));
            
            if (!other.CompareTag("enemy")) return;
            
            Destroy(other.gameObject);
            _audioSources = GetComponents<AudioSource>();
            _audioSources[0].Play();
            GameManager.Instance.TdManager.DecreaseLives();
        }
    }
}