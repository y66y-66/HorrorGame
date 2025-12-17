using UnityEngine;

public class KeyItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 接触したのがプレイヤーか確認 (プレイヤーオブジェクトに "Player" タグが必要です)
        if (other.CompareTag("Player"))
        {
            // プレイヤーからインベントリコンポーネントを取得
            PlayerKeyInventory inventory = other.GetComponent<PlayerKeyInventory>();

            if (inventory != null)
            {
                // 鍵をインベントリに追加
                inventory.AddKey();
                
                // 鍵アイテムをシーンから削除
                Destroy(gameObject); 
            }
        }
    }
}