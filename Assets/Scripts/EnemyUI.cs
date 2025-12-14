using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    // UIの色を変えるためのImageコンポーネメント
    public Image statusIndicator; 
    
    // AIの現在の状態を参照するためのスクリプト
    public EnemyAI enemyAI;

    // 定義した色の設定
    // 見られていない時
    public Color color_Patrol = Color.green; 
    // 見られている時
    public Color color_Chase = Color.red;    
    // 探索中（逃げて視界から消えた時）
    public Color color_Searching = Color.yellow; 

    void Update()
    {
        // AIの状態に応じて色を切り替える
        if (enemyAI != null && statusIndicator != null)
        {
            switch (enemyAI.currentState)
            {
                case EnemyAI.AIState.Chase:
                    // 見られている時 -> 赤
                    statusIndicator.color = color_Chase;
                    break;
                
                case EnemyAI.AIState.Searching:
                    // 探索中（逃げて視界から消えた時） -> 黄色
                    statusIndicator.color = color_Searching;
                    break;
                
                case EnemyAI.AIState.Patrol:
                    // 見られていない時 -> 緑
                    statusIndicator.color = color_Patrol;
                    break;

                default:
                    // その他の状態は緑（またはカスタムカラー）
                    statusIndicator.color = color_Patrol;
                    break;
            }
        }
    }
}