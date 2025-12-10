using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    // ゲームへ戻る
    public void OnRetry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // タイトルに戻る
    public void OnTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
