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

    void Start()
    {
        Debug.Log("Camera = " + mainCamera);
        Debug.Log("FadeImage = " + fadeImage);

        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    public void PlayDeathEffect()
    {

        Debug.Log("死亡演出開始");
        
        if (isDead) return;
        isDead = true;

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
{
    Debug.Log("DeathSequence 開始");

    float elapsed = 0f;

    Quaternion startRot = mainCamera.transform.rotation;
    Quaternion targetRot =
        startRot * Quaternion.Euler(0, 0, tiltAngle);

    // --- カメラ傾け ---
    while (elapsed < 1f)
    {
        elapsed += Time.unscaledDeltaTime;
        mainCamera.transform.rotation =
            Quaternion.Slerp(startRot, targetRot, elapsed);
        yield return null;
    }

    // ★★★ ここ！！！ ★★★
    Debug.Log("暗転開始（強制テスト）");

    fadeImage.color = new Color(0, 0, 0, 1); // ← 強制真っ黒

    yield return new WaitForSecondsRealtime(1f); // 1秒止める

    Debug.Log("ここが見えたら暗転は成功");

    // ↓↓↓ 以下はいったんコメントアウトでOK
    /*
    elapsed = 0f;
    Color c = fadeImage.color;

    while (elapsed < fadeDuration)
    {
        elapsed += Time.unscaledDeltaTime;
        c.a = Mathf.Lerp(0, 1, elapsed / fadeDuration);
        fadeImage.color = c;
        yield return null;
    }
    */

    SceneManager.LoadScene("GameOverScene");
}



}
