using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField]private NavMeshAgent agent;

    [FormerlySerializedAs("WhatIsGround")] [SerializeField]public LayerMask Ground;
    [FormerlySerializedAs("WhatIsPlayer")] [SerializeField]public LayerMask Player;

    
    [SerializeField] private DogStats _playerStats;
    [SerializeField] private Dialogue _dogDialogue;

    [SerializeField] private Animator enemyAnimator;

    //Enemy start position
    private Vector3 enemyStartPos;
    
    //Patrolling
    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private EnemyAIParameters _AIParameters;
    
    
    //Attacking
    private bool alreadyAttacked;
    private int engagedEnemies;

    private AudioSource enemyAudio;
    
    //player stats
    [SerializeField]
    private EnemyStats _enemyStats;

    public AudioClip playerDetectedClip;
    public AudioClip attackClip;


    //In combat
    
    public bool inCombat;
    
   
    //States
    private bool playerInSightRange, playerInAttackRange; 
    
    [SerializeField] private float leashDistance = 5f;

    public delegate void tryAttackPlayer();

    public static event tryAttackPlayer OnTryAttackPlayer;

    private void Awake()
   {
       player = GameObject.FindWithTag("Player").transform;
       agent = GetComponent<NavMeshAgent>();
       enemyAudio = GetComponent<AudioSource>();

       inCombat = false;
       alreadyAttacked = false;
       engagedEnemies = 0;
       enemyStartPos = transform.position;
   }


    private void Update()
    {
        //Check if player is in sight or attack range

        var position = transform.position;
        playerInSightRange = Physics.CheckSphere(position, _AIParameters.sightRange, Player);
        playerInAttackRange = Physics.CheckSphere(position, _AIParameters.attackRange, Player);
        

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling();
        }
        
        if (!playerInSightRange && !playerInAttackRange && inCombat)
        {
            inCombat = false;
            Patrolling();
        }

      
        if (playerInSightRange && !playerInAttackRange && !inCombat)
        {
            ChasePlayer();
        }
        
        
        if (playerInSightRange && playerInAttackRange && engagedEnemies < _AIParameters.maxEngagedEnemies)
        {
            inCombat = true;
            AttackPlayer();

        }
        if(!playerInSightRange && !playerInAttackRange && engagedEnemies > 0)
        {
            engagedEnemies -= 1;
        }
        

        if (inCombat && playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        
        if (Vector3.Distance(enemyStartPos, transform.position) > leashDistance)
        {
            walkPointSet = false;
            agent.SetDestination(enemyStartPos);
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        } 
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            enemyAnimator.SetBool("isChasing", false);
            enemyAnimator.SetBool("isRun", true);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1)
        {
            walkPointSet = false;
            enemyAnimator.SetBool("isChasing", false);
            enemyAnimator.SetBool("isRun", false);
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-_AIParameters.walkPointRange, _AIParameters.walkPointRange);
        float randomX = Random.Range(-_AIParameters.walkPointRange, _AIParameters.walkPointRange);

        var position = transform.position;
        walkPoint = new Vector3(position.x + randomX, position.y , position.z + randomZ);

        var eyeHeight = position + Vector3.up;

        Vector3 dirToWalkPoint = walkPoint - eyeHeight;
        
        if (!Physics.Raycast(eyeHeight, dirToWalkPoint, _AIParameters.sightRange, Ground))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        enemyAnimator.SetBool("isChasing", true);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //make sure enemy doesn't move
        var selfPosition = transform.position;
        agent.SetDestination(selfPosition);

        Vector3 playerDirection = Vector3.ProjectOnPlane(player.position - selfPosition, Vector3.up);
        
        //makes the enemy look at the player
        transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        enemyAnimator.SetBool("isRun", false);
        enemyAnimator.SetBool("isChasing", false);

        if (!alreadyAttacked)
        {
            TryDamagePlayer();
            alreadyAttacked = true;
            enemyAnimator.SetTrigger("isAttack");
           
           //invokes the reset attack method with a set delay time.
           Invoke(nameof(ResetAttack), _AIParameters.timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    

    public void TryDamagePlayer()
    {
        OnTryAttackPlayer?.Invoke();
    }

   

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        //Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _AIParameters.attackRange);
        
        //Sight range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _AIParameters.sightRange);
        
        //Patrolling range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _AIParameters.walkPointRange);

        if (walkPointSet)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(walkPoint, Vector3.up * 10f);
        }
    }
    

#endif
}
