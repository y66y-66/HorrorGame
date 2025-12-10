using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonController : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("SampleScene"); // ←ゲーム本編のシーン名に変更
    }
}
