using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        // 即死 → GameOver シーンへ
        SceneManager.LoadScene("GameOver");
    }
}
