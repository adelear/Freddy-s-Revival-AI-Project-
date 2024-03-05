using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public event System.Action<EnemyStates> OnStateChanged;
    private EnemyStates currentState = EnemyStates.Patrol;
    public List<Transform> patrolPoints;
    private NavMeshAgent agent;
    [SerializeField] GameObject jumpscareLight; 
    [SerializeField] private Animator anim;
    private int currentPatrolIndex = 0;

    private float attackRange = 5f;
    private float detectedRange = 25.0f; 
    private Transform player;

    [SerializeField] Camera jumpscareCam;

    public EnemyStates CurrentState
    {
        get => currentState;
        set
        {
            
            if (currentState == value) return;
            currentState = value;
            OnStateChanged?.Invoke(currentState);
            Debug.Log("Change to state"); 
            switch (currentState)
            {
                case EnemyStates.Idle:
                    StartCoroutine(Idle());
                    break;
                case EnemyStates.Chase:
                    ChasePlayer();
                    break;
                case EnemyStates.Patrol:
                    agent.speed = 1f; 
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
        agent.speed = 1f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        CurrentState = EnemyStates.Patrol;
        jumpscareLight.SetActive(false); 
    }

    private void JumpScarePlayer()
    {
        CurrentState = EnemyStates.Jumpscare;
        GameManager.Instance.SwitchState(GameManager.GameState.DEFEAT);
        anim.Play("Freddy--Jumpscare");
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
        agent.speed = 10f;

        if (DistanceToPlayer() > detectedRange)
        {
            CurrentState = EnemyStates.Patrol;
        }
    }

    private void HandleStates()
    {
        if (CurrentState == EnemyStates.Patrol && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
            Patrol();
        }
        if (agent.velocity != Vector3.zero && CurrentState == EnemyStates.Patrol)  anim.Play("Freddy--Walk");
        if (agent.velocity != Vector3.zero && CurrentState == EnemyStates.Chase) anim.Play("Freddy--Charge");
    }

    IEnumerator FoundPlayer()
    {
        agent.enabled = false;
        anim.Play("Freddy--CPU_Revive");

        yield return new WaitForSeconds(3.0f);

        agent.enabled = true;
        CurrentState = EnemyStates.Chase; 
    }

    IEnumerator Idle()
    {
        agent.ResetPath();
        agent.enabled = false;
        anim.Play("Freddy--Idle"); 

        yield return new WaitForSeconds(3.0f); 

        agent.enabled= true;
        CurrentState = EnemyStates.Patrol;
    }


    void Update()
    {
        HandleStates(); 
        if (DistanceToPlayer() <= detectedRange && CurrentState == EnemyStates.Patrol)
        {
            CurrentState = EnemyStates.FoundPlayer; // Found calls player chase 
        }
        if (DistanceToPlayer() <= attackRange && CurrentState == EnemyStates.Chase)
        {
            CurrentState = EnemyStates.Jumpscare;
        }
        if (DistanceToPlayer() > detectedRange && CurrentState == EnemyStates.Chase) // Lost the player 
        {
            CurrentState = EnemyStates.Idle;
        }
        if (CurrentState == EnemyStates.Chase && DistanceToPlayer() > attackRange && agent.velocity == Vector3.zero)
        {
            agent.SetDestination(player.position);
        }
    }
}
