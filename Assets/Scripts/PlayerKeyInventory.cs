using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; // ★追加：TextMeshProを使うために必要

public class PlayerKeyInventory : MonoBehaviour
{
    [Tooltip("現在所持している鍵の数")]
    public int keysCollected = 0;
    
    [Tooltip("脱出に必要な鍵の数")]
    public int requiredKeys = 3; 

    [Header("UI設定")]
    [Tooltip("鍵の数を表示するテキストUI (TextMeshPro)")]
    public TextMeshProUGUI keyText; // ★追加

    [Header("クリア演出")]
    public GameObject clearUI; // ここにさっき作った ClearCanvas を入れる

    void Start()
    {
        UpdateUI(); // ゲーム開始時にUIを初期化
    }

    /// <summary>
    /// 鍵を取得したときに呼ばれるメソッド
    /// </summary>
    public void AddKey()
    {
        keysCollected++;
        Debug.Log("鍵を取得しました。現在: " + keysCollected + "個");
        
        UpdateUI(); // ★取得したときにUIを更新
    }

    /// <summary>
    /// UIの表示を更新する
    /// </summary>
    void UpdateUI()
{
    if (keyText != null)
    {
        keyText.text = "Keys: " + keysCollected + " / " + requiredKeys;
        Debug.Log("UIを書き換えました: " + keyText.text); // ★これが出るか確認
    }
    else
    {
        Debug.LogError("keyTextが空っぽです！インスペクターで紐付けてください！"); // ★これが出たら紐付けミス
    }
}

    /// <summary>
    /// 脱出ドアから呼ばれ、脱出を試みる
    /// </summary>
    public void TryEscape(EscapeDoor door)
    {
        if (keysCollected >= requiredKeys)
        {
            Debug.Log("脱出成功！");
            door.OpenDoor(this);
        }
        else
        {
            Debug.Log("鍵が足りません。");
            // ここで「鍵が足りません」という文字を一時的にUIに出す処理も追加可能です
        }
    }
    
    public void EndGame()
    {
    Debug.Log("ゲームクリア！");

    // クリア画面を表示
    if (clearUI != null)
    {
        clearUI.SetActive(true);
    }

    // マウスカーソルを表示し、ゲームを止める
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    Time.timeScale = 0f; // ゲーム内の時間を止める
    }
}