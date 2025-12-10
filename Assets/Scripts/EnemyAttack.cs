using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;          
    public float attackRange = 1.5f;  
    public int attackDamage = 20;     
    public float attackCooldown = 1.5f;

    private float lastAttackTime = 0f;

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

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
