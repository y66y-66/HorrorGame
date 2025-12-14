using UnityEngine;
using UnityEngine.UI;

public class EnemyUIIndicator : MonoBehaviour
{
    // Inspectorから設定するUI Imageコンポーネント
    private Image statusIndicator; 
    
    // シーン内の敵AIへの参照を保持する配列
    public EnemyAI[] enemies; 

    // 色の設定
    public Color color_Patrol = Color.green;     // 徘徊中
    public Color color_Chase = Color.red;        // 追跡中（最も警戒すべき状態）
    public Color color_Searching = Color.yellow;  // 探索中

    void Awake()
    {
        // アタッチされているImageコンポーネントを取得
        statusIndicator = GetComponent<Image>();

        // シーン内のすべてのEnemyAIを取得
        enemies = FindObjectsOfType<EnemyAI>();

        // 初期カラーを設定
        statusIndicator.color = color_Patrol;
    }

    void Update()
    {
        if (enemies == null || enemies.Length == 0) return;

        // --- 最も優先度の高い状態を決定するロジック ---
        // 優先度順: 赤 (Chase) > 黄 (Searching) > 緑 (Patrol)
        
        bool isChasing = false;
        bool isSearching = false;

        // すべての敵AIの状態をチェック
        foreach (EnemyAI enemy in enemies)
        {
            if (enemy.currentState == EnemyAI.AIState.Chase)
            {
                isChasing = true; // 1体でも追跡中なら赤
                break; // 最優先なので、ここでチェックを終了
            }
            if (enemy.currentState == EnemyAI.AIState.Searching)
            {
                isSearching = true; // 探索中の敵がいるかチェック
            }
        }

        // --- UIの色を更新 ---
        if (isChasing)
        {
            // 敵が1体でもプレイヤーを追跡している場合
            statusIndicator.color = color_Chase;
        }
        else if (isSearching)
        {
            // 追跡中の敵はいないが、探索中の敵がいる場合
            statusIndicator.color = color_Searching;
        }
        else
        {
            // 追跡中も探索中もいない場合 (全員が徘徊または待機)
            statusIndicator.color = color_Patrol;
        }
    }
}