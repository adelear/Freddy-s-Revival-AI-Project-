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
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent; 
    public event System.Action<EnemyStates> OnStateChanged;
    public List<Transform> patrolPoints;

    [Header("Animation Components")]
    public Enemy enemyType;   
    [SerializeField] private Animator anim;
    [SerializeField] private string idleAnim; 
    [SerializeField] private string walkAnim;
    [SerializeField] private string chargeAnim;
    [SerializeField] private string foundPlayerAnim;
    [SerializeField] private string jumpscareAnim; 
    

    //Attack Components
    private float attackRange = 5f;
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
                break;
            case Enemy.Chica:
                break;
            case Enemy.Bonnie:
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
        agent.ResetPath(); 
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
        agent.SetDestination(player.position);
        agent.speed = sprintSpeed;
        if (DistanceToPlayer() > detectedRange) CurrentState = EnemyStates.Patrol;
    }

    private void HandleStates()
    {
        if (CurrentState == EnemyStates.Patrol && agent.remainingDistance < 0.5f)
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
        agent.ResetPath();
        agent.enabled = false;
        anim.Play(idleAnim); 

        yield return new WaitForSeconds(3.0f); 

        agent.enabled= true;
        CurrentState = EnemyStates.Patrol;
    }

    void Update()
    {
        HandleStates(); 
        if (DistanceToPlayer() <= detectedRange && CurrentState == EnemyStates.Patrol) CurrentState = EnemyStates.FoundPlayer; // Found calls player chase 
        if (DistanceToPlayer() <= attackRange && CurrentState == EnemyStates.Chase) CurrentState = EnemyStates.Jumpscare;
        if (DistanceToPlayer() > detectedRange && CurrentState == EnemyStates.Chase) CurrentState = EnemyStates.Idle;
        if (CurrentState == EnemyStates.Chase && DistanceToPlayer() > attackRange && agent.velocity == Vector3.zero) agent.SetDestination(player.position);
    }
}
