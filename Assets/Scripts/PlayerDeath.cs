using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    bool isDead = false;
    public float gameOverDelay = 1.2f;

    public void Die()
    {
        Debug.Log("PlayerDeath.Die() が呼ばれた");

        if (isDead) return;
        isDead = true;

        // 操作スクリプトだけ止める
        if (TryGetComponent(
            out StarterAssets.FirstPersonController fps))
        {
            fps.enabled = false;
        }

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
{
    Debug.Log("DeathSequence 開始");

    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    if (Camera.main != null)
        Camera.main.transform.localRotation = Quaternion.Euler(15f, 0, 0);

    Debug.Log("プレイヤー死亡演出中…");

    yield return new WaitForSeconds(gameOverDelay);

    Debug.Log("GameOver シーン読み込み直前");
    SceneManager.LoadScene("GameOver");
}

}
