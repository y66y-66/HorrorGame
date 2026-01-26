using UnityEngine;

public class EnemyKillPlayer : MonoBehaviour
{
    private bool alreadyKilled = false;

    void OnTriggerEnter(Collider other)
    {
        if (alreadyKilled) return;
        if (!other.CompareTag("Player")) return;

        alreadyKilled = true;

        PlayerDeathEffect effect =
            other.GetComponent<PlayerDeathEffect>();

        if (effect != null)
        {
            effect.PlayDeathEffect();
        }
    }
}
