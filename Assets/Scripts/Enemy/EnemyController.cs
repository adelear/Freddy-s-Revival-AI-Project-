using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    [Header("General Components")]
    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    [SerializeField] private Player playerController; 
    private bool isIdle = false; 
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent; 
    public event System.Action<EnemyStates> OnStateChanged;
    public List<Transform> patrolPoints;

    [Header("Animation Components")]
    public Enemy enemyType;   
    [SerializeField] private Animator anim;
    private string idleAnim; 
    private string walkAnim;
    private string chargeAnim;
    private string foundPlayerAnim;
    private string jumpscareAnim;

    //Attack Components
    [SerializeField] float attackRange;
    private float detectedRange = 15.0f; 
    private Transform player;

    [Header("Jumpscare Components")]
    [SerializeField] Camera jumpscareCam;
    [SerializeField] GameObject jumpscareLight; 

    private EnemyStates currentState = EnemyStates.Patrol; 
    public EnemyStates CurrentState
    {
        get => currentState;
        set
        {
            
            if (currentState == value) return;
            currentState = value;
            OnStateChanged?.Invoke(currentState);
            Debug.Log($"Change to state {currentState}"); 
            switch (currentState)
            {
                case EnemyStates.Idle:
                    StartCoroutine(Idle());
                    break;
                case EnemyStates.Chase:
                    ChasePlayer();
                    break;
                case EnemyStates.Patrol:
                    agent.speed = walkSpeed; 
                    Patrol();
                    break;
                case EnemyStates.Jumpscare:
                    JumpScarePlayer();
                    break;
                case EnemyStates.FoundPlayer:
                    StartCoroutine(FoundPlayer());
                    break;
                default:
                    break;
            }
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = player.gameObject.GetComponent<Player>(); 
        CurrentState = EnemyStates.Patrol;
        jumpscareLight.SetActive(false); 

        switch (enemyType)
        {
            case Enemy.Freddy:
                idleAnim = "Freddy--Idle"; 
                walkAnim = "Freddy--Walk";
                chargeAnim = "Freddy--Charge";
                foundPlayerAnim = "Freddy--CPU_Revive";
                jumpscareAnim = "Freddy--Jumpscare";

                attackRange = 5f;

                break;
            case Enemy.Chica:
                break;
            case Enemy.Bonnie:
                idleAnim = "Bonnie--Idle";
                walkAnim = "Bonnie--Walk";
                chargeAnim = "Bonnie--Charge";
                foundPlayerAnim = "Bonnie--Idle";
                jumpscareAnim = "bonnie_jumpscare";

                attackRange = 4f;

                break;
            case Enemy.Foxy:
                break;
            default:
                break; 
        }
    }

    private void JumpScarePlayer()
    {
        CurrentState = EnemyStates.Jumpscare;
        GameManager.Instance.SwitchState(GameManager.GameState.DEFEAT);
        anim.Play(jumpscareAnim);
        agent.velocity = Vector3.zero;
        Rigidbody playerRB = player.gameObject.GetComponent<Rigidbody>();
        playerRB.velocity = Vector3.zero;
    }

    private void JumpScare()
    {
        Debug.Log("Jumpscare"); 
        //agent.ResetPath(); 
        agent.enabled = false;
        jumpscareCam.depth = 1f;
        jumpscareLight.SetActive(true);
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }
    

    void Patrol()
    {
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void ChasePlayer()
    {
        if (!playerController.isHidden)
        {
            agent.SetDestination(player.position);
            agent.speed = sprintSpeed;
            if (DistanceToPlayer() > detectedRange) CurrentState = EnemyStates.Patrol;
        }
    }

    private void HandleStates()
    {
        if (CurrentState == EnemyStates.Patrol && agent.remainingDistance < 0.5f && patrolPoints.Count != 0)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            Patrol();
        }
        if (agent.velocity != Vector3.zero && CurrentState == EnemyStates.Patrol)  anim.Play(walkAnim);
        if (agent.velocity != Vector3.zero && CurrentState == EnemyStates.Chase) anim.Play(chargeAnim);
    }

    IEnumerator FoundPlayer()
    {
        agent.enabled = false;
        anim.Play(foundPlayerAnim);

        yield return new WaitForSeconds(3.0f);

        agent.enabled = true;
        CurrentState = EnemyStates.Chase; 
    }

    IEnumerator Idle()
    {
        isIdle = true; 
        agent.ResetPath();
        agent.enabled = false;
        anim.Play(idleAnim); 

        yield return new WaitForSeconds(3f); 

        isIdle = false;
        agent.enabled= true;
        CurrentState = EnemyStates.Patrol;
    }

    void Update()
    {
        if (GameManager.Instance.GetGameState() == GameManager.GameState.GAME)
        {
            HandleStates();
            if (DistanceToPlayer() <= detectedRange && !isIdle && CurrentState == EnemyStates.Patrol && !playerController.isHidden) CurrentState = EnemyStates.FoundPlayer; 
            if (DistanceToPlayer() <= attackRange && CurrentState == EnemyStates.Chase) CurrentState = EnemyStates.Jumpscare;
            if (DistanceToPlayer() > detectedRange && CurrentState == EnemyStates.Chase || playerController.isHidden && CurrentState == EnemyStates.Chase) CurrentState = EnemyStates.Idle;
            if (CurrentState == EnemyStates.Chase && DistanceToPlayer() > attackRange && agent.velocity == Vector3.zero) agent.SetDestination(player.position);
        }
    }
}
