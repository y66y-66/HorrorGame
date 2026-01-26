using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDeathEffect : MonoBehaviour
{
    public Camera mainCamera;
    public Image fadeImage;
    public float tiltAngle = 25f;
    public float fadeDuration = 1.0f;

    private bool isDead = false;

    public void PlayDeathEffect()
    {
        if (isDead) return;
        isDead = true;

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        float timer = 0f;
        Quaternion startRot = mainCamera.transform.rotation;
        Quaternion targetRot =
            startRot * Quaternion.Euler(0, 0, tiltAngle);

        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            // カメラを傾ける
            mainCamera.transform.rotation =
                Quaternion.Slerp(startRot, targetRot, t);

            // 暗転
            fadeImage.color =
                Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        // 最後に GAMEOVER へ
        SceneManager.LoadScene("GameOver");
    }
}
