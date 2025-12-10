using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void OnRetryButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
