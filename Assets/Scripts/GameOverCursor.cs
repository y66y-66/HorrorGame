using UnityEngine;

public class GameOverCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true; // カーソルを表示
        Cursor.lockState = CursorLockMode.None; // ロック解除
    }
}
