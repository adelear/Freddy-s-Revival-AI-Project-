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
    private int currentPatrolIndex = 0;
    private NavMeshAgent agent; 
    public event System.Action<EnemyStates> OnStateChanged;
    public List<Transform> patrolPoints;
    public bool staysIdleUntilShock; 

    [Header("Animation Components")]
    public Enemy enemyType;   
    [SerializeField] private Animator anim;
    private string idleAnim; 
    private string walkAnim;
    private string chargeAnim;
    private string foundPlayerAnim;
    private string jumpscareAnim;
    private string stunAnim;
    private string deadAnim;

    //Attack Components
    [Header("Attack Components")]
    [SerializeField] private float attackRange;
    [SerializeField] private float detectedRange = 15.0f;
    [SerializeField] private int stunsNeededToDie;
    [SerializeField] private int currentStuns = 0;
    private Transform player;

    [Header("Jumpscare Components")]
    [SerializeField] Camera jumpscareCam;
    [SerializeField] GameObject jumpscareLight;
    [SerializeField] AudioClip jumpScareNoise;
    [SerializeField] AudioClip footStep;
    [SerializeField] AudioClip playerFound;
    [SerializeField] AudioClip chaseSound; 
    private AudioSource audioSource; 


    public bool isIdle;
    public bool isStunned = false;
    public bool isDead = false; 

    private EnemyStates currentState = EnemyStates.Idle; 
    public EnemyStates CurrentState
    {
        get => currentState;
        set
        {
            
            if (currentState == value) return;
            currentState = value;
            OnStateChanged?.Invoke(currentState);
            //Debug.Log($"Change to state {currentState}"); 
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
                case EnemyStates.Stun:
                    StartCoroutine(StunEnemy()); 
                    break;
                case EnemyStates.Shutdown:
                    Dead(); 
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
        CurrentState = EnemyStates.Idle;
        audioSource = GetComponent<AudioSource>(); 
        StartCoroutine(Idle()); 
        jumpscareLight.SetActive(false); 

        switch (enemyType)
        {
            case Enemy.Freddy:
                idleAnim = "Freddy--Idle"; 
                walkAnim = "Freddy--Walk";
                chargeAnim = "Freddy--Charge";
                foundPlayerAnim = "Freddy--CPU_Revive";
                jumpscareAnim = "Freddy--Jumpscare";
                stunAnim = "Freddy--Shocked";
                deadAnim = "Freddy--Shutdown"; 

                attackRange = 5f;
                staysIdleUntilShock = false;
                isIdle = false; 
                break;

            case Enemy.Chica:
                idleAnim = "Chica--Idle";
                walkAnim = "Chica--Walk";
                chargeAnim = "Chica--Charge";
                foundPlayerAnim = "Chicae--Idle";
                jumpscareAnim = "Chica--Jumpscare";
                stunAnim = "Chica--Shocked";
                deadAnim = "Chica--Shutdown";

                attackRange = 4f;
                staysIdleUntilShock = true;
                isIdle = true;
                break;
            case Enemy.Bonnie:
                idleAnim = "Bonnie--Idle";
                walkAnim = "Bonnie--Walk";
                chargeAnim = "Bonnie--Charge";
                foundPlayerAnim = "Bonnie--Idle";
                jumpscareAnim = "bonnie_jumpscare";
                stunAnim = "bonnie_stunned";
                deadAnim = "Bonnie--Shutdown"; 

                attackRange = 4f;
                staysIdleUntilShock = true;
                isIdle = true; 
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
        //Debug.Log("Jumpscare"); 
        //agent.ResetPath(); 
        agent.enabled = false;
        jumpscareCam.depth = 1f;
        AudioManager.Instance.PlayOneShot(jumpScareNoise, false); 
        jumpscareLight.SetActive(true);
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }
    

    void Patrol()
    {
        //Debug.Log("Patrolling");
        agent.enabled = true; 
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
        audioSource.clip = playerFound; 
        audioSource.Play(); 

        yield return new WaitForSeconds(3.0f);

        agent.enabled = true;
        audioSource.clip = chaseSound;
        audioSource.Play();
        CurrentState = EnemyStates.Chase; 
    }

    IEnumerator Idle()
    {
        isIdle = true;
        agent.enabled = false;
        anim.Play(idleAnim);

        if (staysIdleUntilShock) 
        {
            while (!isStunned)
            { 
                yield return null;
            } 
        }

        //Debug.Log("Done staying idle"); 
        yield return new WaitForSeconds(3f); 
        isIdle = false;
        CurrentState = EnemyStates.Patrol;
    }


    IEnumerator StunEnemy()
    {
        isStunned = true;
        currentStuns++; 
        agent.enabled = false;
        anim.Play(stunAnim); 
        staysIdleUntilShock = false; 

        agent.velocity = Vector3.zero; 

        yield return new WaitForSeconds(5f);

        if (currentStuns >= stunsNeededToDie) CurrentState = EnemyStates.Shutdown; 
        else
        {
            isStunned = false;
            CurrentState = EnemyStates.Idle;
        }
    }

    private void Dead()
    {
        if (enemyType == Enemy.Freddy) TaskManager.Instance.CompleteTask(TaskManager.TaskType.ImmobilizeFreddy); 
        isDead = true;
        agent.enabled = false;
        anim.Play(deadAnim); 
    }

    private void PlaySound()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = footStep;
            audioSource.pitch = Random.Range(0.8f, 1f);
            //audioSource.volume = Random.Range(0.5f, 1f);
            audioSource.Play();
        }

    }

    void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0); 
        if (isDead) return; // :(  
        if (curPlayingClips[0].clip.name == stunAnim) agent.enabled = false; 
        if (Input.GetKeyDown(KeyCode.T)) CurrentState = EnemyStates.Stun; // Testing
        if (GameManager.Instance.GetGameState() == GameManager.GameState.GAME && !isStunned)
        {
            HandleStates();
            if (DistanceToPlayer() <= detectedRange && !isIdle && CurrentState == EnemyStates.Patrol && !playerController.isHidden) CurrentState = EnemyStates.FoundPlayer; 
            if (DistanceToPlayer() <= attackRange && CurrentState == EnemyStates.Chase) CurrentState = EnemyStates.Jumpscare;
            if (DistanceToPlayer() > detectedRange && CurrentState == EnemyStates.Chase || playerController.isHidden && CurrentState == EnemyStates.Chase) CurrentState = EnemyStates.Idle;
            if (CurrentState == EnemyStates.Chase && DistanceToPlayer() > attackRange && agent.velocity == Vector3.zero) agent.SetDestination(player.position);

        }
    }
}
