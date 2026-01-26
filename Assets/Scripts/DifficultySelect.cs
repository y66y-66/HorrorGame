using UnityEngine;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;

    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    void Start()
    {
        SelectEasy(); // 初期値
    }

    public void SelectEasy()
    {
        ResetColors();
        easyButton.image.color = selectedColor;

        DifficultyManager.Instance.currentDifficulty =
            DifficultyManager.Difficulty.Easy;

        Debug.Log("難易度：EASY");
    }

    public void SelectNormal()
    {
        ResetColors();
        normalButton.image.color = selectedColor;

        DifficultyManager.Instance.currentDifficulty =
            DifficultyManager.Difficulty.Normal;

        Debug.Log("難易度：NORMAL");
    }

    public void SelectHard()
    {
        ResetColors();
        hardButton.image.color = selectedColor;

        DifficultyManager.Instance.currentDifficulty =
            DifficultyManager.Difficulty.Hard;

        Debug.Log("難易度：HARD");
    }

    void ResetColors()
    {
        easyButton.image.color = normalColor;
        normalButton.image.color = normalColor;
        hardButton.image.color = normalColor;
    }
}
