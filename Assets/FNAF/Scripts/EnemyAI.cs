using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public float walkSpeed = 1.5f;
    private NavMeshAgent m_navMeshAgent;
    private GameObject player;
    public Flashlight flashlight;
    public bool inPlayerViewArea = false;
    private bool isAlived = false;
    public Clock clock;
    public int revivalHour = 1;
    private Animator m_animator;
    public GameObject[] onAliveLights;
    public bool wakeupOnPlayerFront = false;
    public CharacterCameraSwitch camSwitcher;
    private string walkingAnimName;
    private string jumpscareAnimName;
    public Characters m_character;
    public CanvasGroupSwitcher CGSwitcher;
    public float jumpscareDistance = 2;
    private bool isJumpscareDone = false;
    private bool isDead = false;
    public bool isFlashlightSensitive = false;
    private AudioSource m_audioSourceJumpScare;

    public DeadScreen deadScreen;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSourceJumpScare = GetComponents<AudioSource>()[0];

        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navMeshAgent.speed = walkSpeed;

        if (GameObject.FindGameObjectsWithTag("Player").Length > 0)
            player = GameObject.FindGameObjectsWithTag("Player")[0];

        m_animator = GetComponent<Animator>();

        EnemyAnimaitonNames enemyAnimations = EnemyAnimations.getAnimationNamesByCharacter(m_character);
        walkingAnimName = enemyAnimations.walking;
        jumpscareAnimName = enemyAnimations.jumpscare;
    }


    void checkDoorAndOpen(GameObject door)
    {
        float distanceToDoor = Vector3.Distance(transform.position, door.transform.position);
        if (distanceToDoor < 10)
        {
            Door dcomp;
            if (door.TryGetComponent<Door>(out dcomp))
            {
                dcomp.interactWithDoor(m_character);
            }
            else
            {
                door.GetComponent<DoubleDoor>().interactWithDoor(m_character);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;



        bool isWakeupOnPlayerFrontOk = false;
        if (wakeupOnPlayerFront)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 4))
                if (hit.collider.CompareTag("Player"))
                    isWakeupOnPlayerFrontOk = true;



        }

        /*Debug.Log(gameObject.name + "|" + clock.hour + "|" + revivalHour);
        Debug.Log(clock.hour >= revivalHour);
        Debug.Log(isAlived);*/
        if (clock.hour >= revivalHour)
        {

            if (wakeupOnPlayerFront)
                if (!isWakeupOnPlayerFrontOk)
                    goto awakeStuff;
            
            isAlived = true;

            foreach(var aLight in onAliveLights)
            {
                aLight.SetActive(true);
            }

            // wake up
            m_animator.SetBool("isAlived", isAlived);
        }

        awakeStuff:

        if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(walkingAnimName))
            if (isAlived && (!inPlayerViewArea || !flashlight.isLightActive))
            {

                m_navMeshAgent.destination = player.transform.position;

                Vector3 direction = (player.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2);


                foreach (var aDoor in GameObject.FindGameObjectsWithTag("weakDoor"))
                {
                    checkDoorAndOpen(aDoor);
                }

                foreach (var aDoor in GameObject.FindGameObjectsWithTag("strongDoor"))
                {
                    checkDoorAndOpen(aDoor);
                }



                // transform.LookAt(player.transform);

                // Jumpscare
                float playerDistance = Vector3.Distance(player.transform.position, transform.position);

                if (playerDistance < jumpscareDistance)
                {
                    m_animator.Play(jumpscareAnimName);
                    // CGSwitcher.hideHUD();
                    CGSwitcher.setUI(UserInterfaces.None);
                    player.SetActive(false);
                    camSwitcher.switchCamera(m_character);
                    isJumpscareDone = true;
                }

                m_animator.speed = 1;
                m_navMeshAgent.isStopped = false;
            }
            else
            {
                m_animator.speed = 0;
                m_navMeshAgent.velocity = Vector3.zero;
                m_navMeshAgent.isStopped = true;
            }

        if (isJumpscareDone)
        {
            m_audioSourceJumpScare.enabled = true;
            StartCoroutine("jumpscareEndDeadCounter");
        }



    }


    IEnumerator jumpscareEndDeadCounter()
    {
        yield return new WaitForSeconds(.5f);

        if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName(jumpscareAnimName))
        {
            // When the jumpscare animation end
            if (!isDead)
            {
                deadScreen.onDead();
                isDead = true;
            }
        }

    }



}
