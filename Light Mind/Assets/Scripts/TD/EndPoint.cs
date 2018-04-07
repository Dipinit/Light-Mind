using UnityEngine;

namespace TD
{
    public class EndPoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(string.Format("End triggered by {0}", other.gameObject));
            
            if (!other.CompareTag("enemy")) return;
            
            Destroy(other.gameObject);
            GameManager.Instance.TdManager.DecreaseLives();
        }
    }
}