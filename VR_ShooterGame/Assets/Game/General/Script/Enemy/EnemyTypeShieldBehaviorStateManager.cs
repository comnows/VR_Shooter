using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Normal.Realtime;
public class EnemyTypeShieldBehaviorStateManager : MonoBehaviour
{
    public string currentState;
    public Animator animator;
    public NavMeshAgent nav;
    public GameObject player;
    public LayerMask whatIsGround, whatIsPlayer;
    private float sightRange, attackRange, defenceRange;
    public float fieldOfViewAngle;
    public bool playerInAttackRange,enemyInDefenceState;
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
        defenceRange = 5f;
        fieldOfViewAngle = 120f;
        enemyInDefenceState = false;
        currentState = "Idle";
    }

    void Update()
    {
        currentState = enemySyncData._enemyBehaviorState;
        player = GameObject.Find(enemySyncData._enemyTarget);
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

            case "Defence":
            if (enemySyncData._enemyTarget != "")
            {
                Defence();
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

        if (player != null)
        {
            animator.SetTrigger("PrepareToAttack");

            Invoke(nameof(DeleyChangeIdleToChaseState), 0.8f);
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
            }
        }
    }

    private void DeleyChangeIdleToChaseState()
    {
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
                //currentState = "Idle";
                enemySyncData.ChangeBehaviorState("Idle");
            }
        }
    }

    public void CheckCanDefence()
    {
        float distanceBetweenTarget = Vector3.Distance(player.transform.position, transform.position);
        
        if (distanceBetweenTarget > defenceRange)
        {
            //currentState = "Defence";
            enemySyncData.ChangeBehaviorState("Defence");
        }
    }

    private void Defence()
    {
        animator.SetBool("isChase",false);
        nav.SetDestination(transform.position);
        transform.LookAt(player.transform);
        animator.SetBool("isShieldActivated",true);
        if (!enemyInDefenceState)
        {
            enemyInDefenceState = true;
            Invoke(nameof(DeactivedShield), 3f);
        }
    }

    private void DeactivedShield()
    {
        animator.SetBool("isShieldActivated",false);
        enemyInDefenceState = false;
        //currentState = "Chase";
        enemySyncData.ChangeBehaviorState("Chase");
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
                //currentState = "Idle";
                enemySyncData.ChangeBehaviorState("Idle");
            }
        }
        else if (playerInAttackRange)
        {
            if (player.GetComponent<PlayerSyncData>()._playerHP <= 0)
            {
                enemySyncData.ChangeEnemyTarget("");
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

