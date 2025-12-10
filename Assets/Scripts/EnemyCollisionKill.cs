using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCollisionKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Player に触れたら
        if (other.CompareTag("Player"))
        {
            // GameOver シーンへ移動
            SceneManager.LoadScene("GameOver");
        }
    }
}
