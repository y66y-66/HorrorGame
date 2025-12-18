using UnityEngine;
using UnityEngine.SceneManagement; // シーン切り替えに必要

public class SceneLoader : MonoBehaviour
{
    public void LoadTitle()
    {
        // ゲームを止めていた時間を元に戻す（重要！）
        Time.timeScale = 1f;
        
        // 「TitleScene」という名前のシーンを読み込む
        // ※実際のタイトル画面のシーン名に合わせて変更してください
        SceneManager.LoadScene("TitleScene");
    }
}