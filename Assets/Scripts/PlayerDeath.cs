using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDeath : MonoBehaviour
{
    public float gameOverDelay = 1.2f;

    public void Die()
    {
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // 操作不能にする
        if (TryGetComponent(out CharacterController cc))
            cc.enabled = false;

        if (TryGetComponent(out Rigidbody rb))
            rb.isKinematic = true;

        // カーソル表示
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.15f);
        Time.timeScale = 1f;

        Camera.main.transform.localRotation =
        Quaternion.Euler(10f, 0, 0);


        // ここで効果音・画面演出を入れてもOK
        Debug.Log("プレイヤー死亡演出中…");

        yield return new WaitForSeconds(gameOverDelay);

        SceneManager.LoadScene("GameOver");
    }
}
