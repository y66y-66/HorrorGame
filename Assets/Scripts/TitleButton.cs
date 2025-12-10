using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
