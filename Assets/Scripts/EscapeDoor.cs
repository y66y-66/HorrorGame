using UnityEngine;

public class EscapeDoor : MonoBehaviour
{
    private bool isOpened = false;
    private Animator doorAnimator; 

    void Start()
    {
        // ドアにAnimatorが設定されている場合
        doorAnimator = GetComponent<Animator>(); 
    }

    /// <summary>
    /// プレイヤーからのインタラクトを処理
    /// </summary>
    public void Interact(PlayerKeyInventory playerInventory)
    {
        if (isOpened) return; 

        // プレイヤーのインベントリを使って脱出を試みる
        playerInventory.TryEscape(this);
    }
    
    /// <summary>
    /// 鍵が揃っていた場合に、PlayerKeyInventoryから呼ばれる
    /// </summary>
    public void OpenDoor(PlayerKeyInventory playerInventory)
    {
        isOpened = true;
        
        // ドアが開くアニメーションを再生
        if (doorAnimator != null)
        {
            // 例: AnimatorのOpenトリガーを設定している場合
            // doorAnimator.SetTrigger("Open"); 
        }
        
        // ドアのColliderを無効化し、プレイヤーが通れるようにする
        Collider doorCollider = GetComponent<Collider>();
        if (doorCollider != null)
        {
            doorCollider.enabled = false;
        }

        Debug.Log("ドアが開きました！");
        
        // 脱出・ゲーム終了ロジックを実行
        playerInventory.EndGame();
    }
}