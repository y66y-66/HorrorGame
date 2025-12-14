using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    // --- AIの状態管理 ---
    public enum AIState
    {
        Idle,       // 待機中
        Chase,      // 追跡中
        Patrol,     // 徘徊中 (ランダムな目的地を巡回)
        Searching,  // 最後にプレイヤーを見た場所を探している
        Attack      // ★ NEW: 攻撃中
    }

    public AIState currentState = AIState.Patrol;

    // --- コンポーネント ---
    private NavMeshAgent agent;
    private Transform playerTransform; // プレイヤーのTransform
    private Animator animator; // ★ NEW: Animatorコンポーネント

    // --- 追跡設定 ---
    public float sightRange = 15f; 
    public float searchDuration = 5f;
    private Vector3 lastKnownPlayerPosition;
    private float timeSinceLostPlayer;
    
    // --- 攻撃設定 ---
    [Header("Attack Settings")]
    public float attackRange = 2f;         // 攻撃が届く最大距離
    public float attackCooldown = 2f;      // 攻撃間のクールダウン時間
    public float damageAmount = 10f;       // プレイヤーに与えるダメージ量
    private float timeSinceLastAttack;     // 最後に攻撃してからの経過時間

    // --- 移動速度設定 (Animator連動のため追加) ---
    [Header("Movement Speeds")]
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5.5f;

    // --- 【知覚の完成】追加: レイキャストの衝突対象レイヤー ---
    [Header("Perception Settings")]
    public LayerMask sightDetectionMask; 

    [Header("Hearing Settings")]
    public float hearingRange = 10f; 
    
    // --- 徘徊設定 ---
    public float patrolRadius = 20f;
    public float patrolWaitTime = 3f;
    private float currentWaitTime = 0f;
    
    // --- 初期化 ---
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // ★ NEW: Animatorコンポーネントの取得（子オブジェクトから） ★
        animator = GetComponentInChildren<Animator>(); 

        // プレイヤーオブジェクトをタグで検索して取得
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        
        // ★ NEW: 攻撃クールダウンをリセット ★
        timeSinceLastAttack = attackCooldown;
    }

    // --- メインループ ---
    void Update()
    {
        // プレイヤーとの距離を確認
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        // ★ UPDATE: 攻撃タイマーを更新 ★
        timeSinceLastAttack += Time.deltaTime; 

        // --- ステート決定ロジック ---
        if (CanSeePlayer(distanceToPlayer)) // プレイヤーを視認
        {
            lastKnownPlayerPosition = playerTransform.position;
            timeSinceLostPlayer = 0f;
            
            // ★ UPDATE: 攻撃範囲内ならAttackステートに移行 ★
            if (distanceToPlayer <= attackRange)
            {
                currentState = AIState.Attack;
            }
            else // 視認したが攻撃範囲外なら追跡を継続
            {
                currentState = AIState.Chase;
            }
        }
        else if (currentState == AIState.Chase)
        {
            // 追跡中に見失った場合 (Searchingステートに移行)
            currentState = AIState.Searching;
            agent.SetDestination(lastKnownPlayerPosition);
        }
        
        // 現在のステートに応じた行動を実行
        HandleState();
    }

    // --- ステート処理 ---
    void HandleState()
    {
        // ★ NEW: Animator制御ロジックをここで実行 ★
        if (animator != null)
        {
            float currentSpeed = agent.velocity.magnitude;
            animator.SetFloat("Speed", currentSpeed); 
            
            // 攻撃ステートでない限り、アニメーターの攻撃フラグを解除
            if (currentState != AIState.Attack)
            {
                animator.SetBool("IsAttacking", false);
            }
        }
        
        // NavMeshAgentの移動速度設定
        switch (currentState)
        {
            case AIState.Chase:
                agent.isStopped = false; 
                agent.speed = chaseSpeed; // 追跡速度を設定
                agent.SetDestination(playerTransform.position);
                break;

            case AIState.Attack:
                agent.isStopped = true; // 停止して攻撃
                HandleAttack(); // 攻撃処理を実行
                break; // Attackステートではこれ以上移動しない

            case AIState.Searching:
                agent.isStopped = false;
                agent.speed = patrolSpeed; // 探索中は徘徊速度
                HandleSearching();
                break;

            case AIState.Patrol:
                agent.isStopped = false;
                agent.speed = patrolSpeed; // 徘徊速度を設定
                HandlePatrol();
                break;
        }
    }

    // --- 攻撃モードのロジック ---
    void HandleAttack()
    {
        // 攻撃時、プレイヤーの方向に向き直る
        RotateTowardsTarget(playerTransform.position);

        // クールダウンが終了しているかチェック
        if (timeSinceLastAttack >= attackCooldown)
        {
            // ★ NEW: 攻撃アニメーションをトリガー ★
            if (animator != null)
            {
                animator.SetBool("IsAttacking", true); 
            }

            // 攻撃を実行し、プレイヤーにダメージを与える
            PerformAttack();
            
            // クールダウンタイマーをリセット
            timeSinceLastAttack = 0f;
        }
    }

    // ★ NEW: 攻撃実行（ダメージ処理）の関数 ★
    void PerformAttack()
    {
        // プレイヤーにダメージを与えるロジックを実装
        
        // プレイヤーのHealth/Damageableコンポーネントを取得する例
        // (プレイヤー側に HealthSystem.cs のようなスクリプトが必要)
        /*
        PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
            Debug.Log(gameObject.name + " がプレイヤーに " + damageAmount + " ダメージを与えました。");
        }
        */
        
        // 現時点ではログ出力のみ
        Debug.Log(gameObject.name + " が攻撃を実行しました！");
    }

    // ★ NEW: ターゲットの方向へ滑らかに回転する関数 ★
    void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // --- 探索モードのロジック ---
    void HandleSearching()
    {
        timeSinceLostPlayer += Time.deltaTime; 
        
        if (timeSinceLostPlayer >= searchDuration)
        {
            currentState = AIState.Patrol;
            currentWaitTime = patrolWaitTime; 
        }
    }

    // --- 徘徊モードのロジック ---
    void HandlePatrol()
    {
        if (IsAtDestination())
        {
            currentWaitTime += Time.deltaTime;

            if (currentWaitTime >= patrolWaitTime)
            {
                Vector3 newDestination;
                if (GetRandomPoint(transform.position, patrolRadius, out newDestination))
                {
                    agent.SetDestination(newDestination);
                    currentWaitTime = 0f;
                }
            }
        }
        else
        {
            currentWaitTime = 0f;
        }
    }
    
    // プレイヤーの音源情報を受け取る関数
    public void OnPlayerMadeNoise(Vector3 noisePosition, float noiseVolume)
    {
        float distanceToNoise = Vector3.Distance(transform.position, noisePosition);
        
        if (distanceToNoise <= hearingRange * noiseVolume) 
        {
            if (currentState != AIState.Chase && currentState != AIState.Attack) // Attack中も無視
            {
                lastKnownPlayerPosition = noisePosition;
                currentState = AIState.Searching;
                timeSinceLostPlayer = 0f;
                
                agent.SetDestination(noisePosition);
            }
        }
    }

    // --- ヘルパー関数 ---

    // プレイヤーが視認範囲（距離）内にいるかチェックする関数
    bool CanSeePlayer(float distance)
    {
        if (distance <= sightRange)
        {
            return true;
        }
        return false;
    }

    // NavMesh Agentが目的地に到達したかチェック
    bool IsAtDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    // NavMesh上のランダムなポイントを検索する関数
    bool GetRandomPoint(Vector3 center, float radius, out Vector3 result)
    {
        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * radius;

        NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, radius, UnityEngine.AI.NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}