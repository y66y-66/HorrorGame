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
        Attack      // 攻撃中
    }

    public AIState currentState = AIState.Patrol;

    // --- コンポーネント ---
    private NavMeshAgent agent;
    private Transform playerTransform; // プレイヤーのTransform
    private Animator animator; // Animatorコンポーネント

    // --- 追跡設定 ---
    public float sightRange = 15f; 
    public float searchDuration = 5f;
    private Vector3 lastKnownPlayerPosition;
    private float timeSinceLostPlayer;
    
    // --- 攻撃設定 ---
    [Header("Attack Settings")]
    public float attackRange = 2f;      // 攻撃が届く最大距離
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
        
        // Animatorコンポーネントの取得（子オブジェクトから）
        animator = GetComponentInChildren<Animator>(); 

        // プレイヤーオブジェクトをタグで検索して取得
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        
        // ★修正点 1: playerObj の宣言後に Debug.Log を移動し、CS0103エラーを解消
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            Debug.Log("Player found: " + (playerObj != null)); 
        }
        else
        {
            Debug.LogError("Player object with 'Player' tag not found!");
        }

        // 攻撃クールダウンをリセット
        timeSinceLastAttack = attackCooldown;

        ApplyDifficulty();

    }

    // --- 難易度による敵の強さ設定 ---
void ApplyDifficulty()
{
    switch (DifficultyManager.Instance.currentDifficulty)
    {
        case DifficultyManager.Difficulty.Easy:
            patrolSpeed = 4.5f;
            chaseSpeed = 7.5f;
            sightRange = 15f;
            searchDuration = 3f;
            break;

        case DifficultyManager.Difficulty.Normal:
            patrolSpeed = 4.5f;
            chaseSpeed = 9f;
            sightRange = 20f;
            searchDuration = 7f;
            break;

        case DifficultyManager.Difficulty.Hard:
            patrolSpeed = 6.5f;
            chaseSpeed = 12.0f;
            sightRange = 25f;
            searchDuration = 10f;
            break;
    }

    Debug.Log("Enemy difficulty applied: " +
        DifficultyManager.Instance.currentDifficulty);
}


    // --- メインループ ---
    void Update()
    {
        // プレイヤーTransformが取得できていない場合は処理を中断
        if (playerTransform == null) return; 

        // プレイヤーとの距離を確認
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        // 攻撃タイマーを更新
        timeSinceLastAttack += Time.deltaTime; 

        // --- ステート決定ロジック ---
        if (CanSeePlayer(distanceToPlayer)) // プレイヤーを視認
        {
            lastKnownPlayerPosition = playerTransform.position;
            timeSinceLostPlayer = 0f;
            
            // 攻撃範囲内ならAttackステートに移行
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
            // SetDestination(lastKnownPlayerPosition)は既にUpdate()の外で処理済み
        }
        
        // 現在のステートに応じた行動を実行
        HandleState();
    }

    // --- ステート処理 ---
    void HandleState()
    {
        if (agent == null || animator == null) return; // コンポーネントが欠落している場合はここで終了

        // 移動速度をAnimatorに渡す
        float currentSpeed = agent.velocity.magnitude;
        animator.SetFloat("Speed", currentSpeed); 

        // 攻撃状態をリセット
        if (currentState != AIState.Attack)
        {
            animator.SetBool("IsAttacking", false);
        }

        switch (currentState)
        {
            case AIState.Chase:
            agent.isStopped = false;

            // 本気追跡モード
            agent.speed = chaseSpeed;
            agent.acceleration = 50f;
            agent.angularSpeed = 720f;
            agent.stoppingDistance = 0.3f;

            // 毎フレーム、プレイヤーの現在位置へ
            agent.SetDestination(playerTransform.position);
            break;

            case AIState.Searching:
                agent.speed = patrolSpeed;
                agent.isStopped = false;
                HandleSearching(); // ★修正点 2: 探索の継続ロジックを実行
                break;

            case AIState.Patrol:
                agent.speed = patrolSpeed;
                agent.isStopped = false;
                HandlePatrol(); // ★修正点 2: 徘徊の継続ロジックを実行
                break;

            case AIState.Attack:
                agent.isStopped = true; 
                HandleAttack();
                break;
            
            case AIState.Idle:
                agent.isStopped = true;
                break;
        }
    }

    // EnemyAI.cs の HandleAttack() 関数 (変更なし)
    void HandleAttack()
    {
        RotateTowardsTarget(playerTransform.position); // ターゲットの方向へ回転

        // クールダウンが終了しているかチェック
        if (timeSinceLastAttack >= attackCooldown)
        {
            // 1. 攻撃アニメーションをトリガー
            animator.SetBool("IsAttacking", true); 

            // 2. 攻撃を実行
            PerformAttack();
            
            // クールダウンタイマーをリセット
            timeSinceLastAttack = 0f;
        }
    }

    // 攻撃実行（ダメージ処理）の関数 (変更なし)
    void PerformAttack()
    {
        Debug.Log(gameObject.name + " が攻撃を実行しました！");
    }

    // ターゲットの方向へ滑らかに回転する関数 (変更なし)
    void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // --- 探索モードのロジック --- (変更なし)
    void HandleSearching()
    {
        timeSinceLostPlayer += Time.deltaTime; 
        
        if (timeSinceLostPlayer >= searchDuration)
        {
            currentState = AIState.Patrol;
            currentWaitTime = patrolWaitTime; 
        }
    }

    // --- 徘徊モードのロジック --- (変更なし)
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
    
    // プレイヤーの音源情報を受け取る関数 (変更なし)
    public void OnPlayerMadeNoise(Vector3 noisePosition, float noiseVolume)
    {
        float distanceToNoise = Vector3.Distance(transform.position, noisePosition);
        
        if (distanceToNoise <= hearingRange * noiseVolume) 
        {
            if (currentState != AIState.Chase && currentState != AIState.Attack)
            {
                lastKnownPlayerPosition = noisePosition;
                currentState = AIState.Searching;
                timeSinceLostPlayer = 0f;
                
                agent.SetDestination(noisePosition);
            }
        }
    }

    // --- ヘルパー関数 ---

    // プレイヤーが視認範囲（距離）内にいるかチェックする関数 (変更なし)
    bool CanSeePlayer(float distance)
    {
        if (distance <= sightRange)
        {
            return true;
        }
        return false;
    }

    // NavMesh Agentが目的地に到達したかチェック (変更なし)
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
    
    // NavMesh上のランダムなポイントを検索する関数 (変更なし)
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