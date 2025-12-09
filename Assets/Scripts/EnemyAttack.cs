using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;        // Player
    public float attackRange = 1.5f; // 攻撃距離
    public int attackDamage = 20;    // 与えるダメージ
    public float attackCooldown = 1.5f; // 次の攻撃までの時間

    private float lastAttackTime = 0f;

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        // 攻撃できる距離なら
        if (distance <= attackRange)
        {
            if (Time.time > lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack()
    {
        Debug.Log("敵が攻撃した！");

        PlayerHealth ph = target.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(attackDamage);
        }
    }
}
