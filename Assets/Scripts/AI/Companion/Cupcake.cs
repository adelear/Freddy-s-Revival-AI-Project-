using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cupcake : MonoBehaviour
{
    public event System.Action<CupcakeStates> OnStateChanged;
    private Transform player;
    public List<Transform> targetPoints; 
    private int currentTarget = 0;
    private float MinDistanceToRun = 15f;
    private float MinDistanceToIdle = 3f; 
    NavMeshAgent agent;
    Animator anim;

    public enum CupcakeStates
    {
        Idle,
        Run
    }
    private CupcakeStates currentState = CupcakeStates.Idle;
    public CupcakeStates CurrentState
    {
        get => currentState;
        set
        {
            if (currentState == value) return;
            currentState = value;
            OnStateChanged?.Invoke(currentState);

            switch (currentState)
            {
                case CupcakeStates.Idle:
                    Idle(); 
                    break;
                case CupcakeStates.Run:
                    RunToNextPoint();
                    break;
                default:
                    break; 
            }
        }
    }

    float DistanceToPlayer()
    {
        return Vector3.Distance(player.position, transform.position); 
    }

    void Start()
    {
        anim = transform.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Idle(); 
    }

    void RunToNextPoint()
    {
        Debug.Log("Cupcake is running to the next point");
        anim.Play("JumpWalk");
        agent.enabled = true;
        agent.SetDestination(targetPoints[currentTarget].position);
    }


    void Idle()
    {
        Debug.Log("Cupcake is Idling");
        anim.Play("Idle"); 
        agent.enabled = false;
    }
    void Update()
    {
        if (GameManager.Instance.GetGameState() != GameManager.GameState.GAME) return;
        if (CurrentState == CupcakeStates.Run && agent.remainingDistance < 0.5f && targetPoints.Count != 0)
        {
            currentTarget = (currentTarget + 1) % targetPoints.Count;
            RunToNextPoint();
        } 
        float distance = DistanceToPlayer();

        if (distance < MinDistanceToIdle) CurrentState = CupcakeStates.Idle;
        else if (distance < MinDistanceToRun) CurrentState = CupcakeStates.Run;
        else CurrentState = CupcakeStates.Idle;

        if (currentState == CupcakeStates.Run) RunToNextPoint(); 
        
    } 
}
