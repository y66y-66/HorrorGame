using UnityEngine;

public class EnemyKillPlayer : MonoBehaviour
{
    private bool hasKilled = false;

    void OnTriggerEnter(Collider other)
    {
        // 二重発動防止
        if (hasKilled) return;

        // Player タグ以外は無視
        if (!other.CompareTag("Player")) return;

        hasKilled = true;

        Debug.Log("プレイヤー捕獲");

        // 親オブジェクトから PlayerDeath を探す
        PlayerDeath death =
            other.GetComponentInParent<PlayerDeath>();

        if (death != null)
        {
            death.Die();
        }
        else
        {
            Debug.LogError("PlayerDeath が Player に付いていない！");
        }

        // 念のため敵の移動を止める
        if (TryGetComponent(out EnemyAI ai))
        {
            ai.enabled = false;
        }

        if (TryGetComponent(out UnityEngine.AI.NavMeshAgent agent))
        {
            agent.isStopped = true;
        }
    }
}
