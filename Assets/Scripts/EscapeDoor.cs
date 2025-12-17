using UnityEngine;

public class EscapeDoor : MonoBehaviour
{
    private bool isOpened = false;
    private Animator doorAnimator;

    void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// Eキーを押したときにプレイヤーから呼ばれる入り口
    /// </summary>
    public void Interact(PlayerKeyInventory playerInventory)
    {
        if (isOpened) return;

        // 鍵の数を判定
        if (playerInventory.keysCollected >= playerInventory.requiredKeys)
        {
            OpenDoor(playerInventory);
        }
        else
        {
            int missing = playerInventory.requiredKeys - playerInventory.keysCollected;
            Debug.Log($"鍵が足りません。あと {missing} 個必要です。");
        }
    }

    /// <summary>
    /// 実際にドアを開ける処理（publicにする必要があります）
    /// </summary>
    public void OpenDoor(PlayerKeyInventory playerInventory)
    {
        if (isOpened) return;
        isOpened = true;

        // 1. アニメーション再生
        if (doorAnimator != null)
        {
            // doorAnimator.SetTrigger("Open"); 
        }

        // 2. 当たり判定を消して通れるようにする
        Collider doorCollider = GetComponent<Collider>();
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        // 3. 演出としてドア自体を消す（アニメーションがない場合）
        // gameObject.SetActive(false);

        Debug.Log("脱出成功！ドアが開きました。");

        // 4. クリア処理を呼び出す
        if (playerInventory != null)
        {
            playerInventory.EndGame();
        }
    }
}