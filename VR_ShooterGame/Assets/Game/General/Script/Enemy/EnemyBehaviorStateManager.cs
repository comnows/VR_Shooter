using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Normal.Realtime;
public class EnemyBehaviorStateManager : MonoBehaviour
{
    private string currentState;
    public Animator animator;
    public NavMeshAgent nav;
    public GameObject player;
    public LayerMask whatIsGround, whatIsPlayer;
    private float sightRange, attackRange;
    public float fieldOfViewAngle;
    public bool playerInAttackRange;
    private float timeBetweenAttacks;
    private bool alreadyAttacked;
    private GameObject[] playersInSight;
    private EnemySyncData enemySyncData;
    private RealtimeTransform _realtimeTransform;
    private RealtimeView _realtimeView;
    private Vector3 enemyRayPos;
    
    void Start()
    {
        _realtimeView = GetComponent<RealtimeView>();
        _realtimeTransform = GetComponent<RealtimeTransform>();
        enemyRayPos = new Vector3(0,2,0);
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        enemySyncData = GetComponent<EnemySyncData>();
        timeBetweenAttacks = 1.5f;
        sightRange = 10f;
        attackRange = 2f;
        fieldOfViewAngle = 120f;
        currentState = "Idle";
    }

    void Update()
    {
        currentState = enemySyncData._enemyBehaviorState;
        
        // if(enemySyncData._enemyTarget != null)
        // {
        player = GameObject.Find(enemySyncData._enemyTarget);
        // }
       CheckState();
    }

    private void CheckState()
    {
        switch(currentState)
        {
            case "Idle":
            Idle();
            break;

            case "Chase":
            if (enemySyncData._enemyTarget != "")
            {
                ChasePlayer();
            }
            else 
            {
                enemySyncData.ChangeBehaviorState("Idle");
            }
            break;
            
            case "Attack":
            if (enemySyncData._enemyTarget != "")
            {
            AttackPlayer();
            }
            else 
            {
                enemySyncData.ChangeBehaviorState("Idle");
            }
            break;

            case "Die":
            Death();
            break;

        } 
    }

    private void Idle()
    {
        animator.SetBool("isChase",false);

        CheckPlayerInSigthRange();

        if (enemySyncData._enemyTarget != "")
        {
            animator.SetTrigger("PrepareToAttack");

            StartCoroutine(DeleyChangeIdleToChaseState());
        }
    }

    private void CheckPlayerInSigthRange()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, sightRange, whatIsPlayer);

        foreach (Collider target in targets)
        {
            Vector3 direction = (target.transform.position - transform.position + enemyRayPos).normalized;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle <= fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + enemyRayPos, direction, out hit, sightRange))
                {
                    Debug.DrawLine(transform.position + enemyRayPos, hit.point, Color.green);
                }
                else
                {
                    Debug.DrawLine(transform.position + enemyRayPos, transform.position + direction * sightRange, Color.red);
                    if (target.gameObject.GetComponent<PlayerSyncData>()._playerHP > 0)
                    {
                        SetTarget(target.gameObject);
                    }
                }
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        if (enemySyncData._enemyTarget == "")
        {
            enemySyncData.ChangeEnemyTarget(target.name);
            //player = target;
        }
        else
        {
            float distanceBetweenPlayer1 = Vector3.Distance(player.transform.position, transform.position);
            float distanceBetweenPlayer2 = Vector3.Distance(target.transform.position, transform.position);
            if (distanceBetweenPlayer1 > distanceBetweenPlayer2)
            {
                enemySyncData.ChangeEnemyTarget(target.name);
                //player = target;
            }
        }
    }

    IEnumerator DeleyChangeIdleToChaseState()
    {
        yield return new WaitForSeconds (0.8f);
        //currentState = "Chase";
        enemySyncData.ChangeBehaviorState("Chase");
    }

    private void ChasePlayer()
    {
        animator.SetBool("isChase",true);

        CheckPlayerInSigthRange();

        if (enemySyncData._enemyTarget != "")
        {
        nav.SetDestination(player.transform.position);
        }

        float distanceBetweenTarget = Vector3.Distance(player.transform.position, transform.position);
        playerInAttackRange = distanceBetweenTarget <= attackRange;

        if (playerInAttackRange) 
        {
            if (player.GetComponent<PlayerSyncData>()._playerHP > 0)
            {
                //currentState = "Attack";
                enemySyncData.ChangeBehaviorState("Attack");
            }
            else 
            {
                enemySyncData.ChangeEnemyTarget("");
                //player = null;
                //currentState = "Idle";
                enemySyncData.ChangeBehaviorState("Idle");
            }
        }
    }

    private void AttackPlayer()
    {
        animator.SetBool("isChase",false);
        nav.SetDestination(transform.position);
        transform.LookAt(player.transform);
        if (!alreadyAttacked)
        {
            if (player.GetComponent<PlayerSyncData>()._playerHP > 0)
            {
                animator.SetTrigger("Attack");
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }

        float distanceBetweenTarget = Vector3.Distance(player.transform.position, transform.position);
        playerInAttackRange = distanceBetweenTarget <= attackRange;

        if (!playerInAttackRange)
        {
            if (player.GetComponent<PlayerSyncData>()._playerHP > 0)
            {
                //currentState = "Chase";
                enemySyncData.ChangeBehaviorState("Chase");
            }
            else 
            {
                enemySyncData.ChangeEnemyTarget("");
                //player = null;
                //currentState = "Idle";
                enemySyncData.ChangeBehaviorState("Idle");
            }
        }
        else if (playerInAttackRange)
        {
            if (player.GetComponent<PlayerSyncData>()._playerHP <= 0)
            {
                enemySyncData.ChangeEnemyTarget("");
                //player = null;
                //currentState = "Idle";
                enemySyncData.ChangeBehaviorState("Idle");
            }
        }
    }

    private void ResetAttack()
    {
        Debug.Log("Reset Attack");
        animator.SetBool("isChase",false);
        alreadyAttacked = false;
    }

    private void Death()
    {
        animator.SetTrigger("Death");
        StartCoroutine(RemoveBody());
    }

    public void Die()
    {
        currentState = enemySyncData._enemyBehaviorState;
    }
    
    private IEnumerator RemoveBody()
    {
        if (_realtimeView.isUnownedInHierarchy)
        {
            _realtimeView.RequestOwnership();
        }

        yield return new WaitForSeconds(5);

        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            Realtime.Destroy(gameObject);
        }
    }
}
