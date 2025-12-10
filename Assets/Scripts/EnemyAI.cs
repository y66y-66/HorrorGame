using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        System.Console.WriteLine("hogeeee");
        Debug.Log("AAA");
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            System.Console.WriteLine("hoge");
            Debug.Log("abc");
        }
    }
}
