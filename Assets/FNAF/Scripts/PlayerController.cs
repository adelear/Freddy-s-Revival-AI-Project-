using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody playerRigid;
    public float walkSpeed = 2;

    public Animator lHandAnim;
    public Flashlight flashlight;

    public Animator rHandAnim;
    public float interactDistance = 5;
    public GameObject pVisibles;
    public ProgressBar interactProgressBar;

    public float holdInteractCountdownSeconds = 5;
    private float holdInteractCountdown = 0;

    public Inventory inventory;
    public NotificationPresenter notificationPresenter;

    public bool lockPlayerController = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!lockPlayerController)
        {
            // Movement
            float pWS = Input.GetAxis("Horizontal");
            float pAD = Input.GetAxis("Vertical");
            pWS = Mathf.Clamp(pWS, -1, 1);
            pAD = Mathf.Clamp(pAD, -1, 1);



            float walkSpeedMultiplier = 1;
            if (Input.GetKey(KeyCode.LeftShift))
                walkSpeedMultiplier = 2;



            var pVel = playerRigid.velocity;
            pVel.x = pWS * walkSpeed * walkSpeedMultiplier;
            pVel.z = pAD * walkSpeed * walkSpeedMultiplier;

            playerRigid.velocity = transform.TransformDirection(pVel);

            // Hand
            bool lHandFire = Input.GetButtonDown("Fire2");
            bool rHandFire = Input.GetButtonDown("Fire1");

            if (lHandFire)
            {
                lHandAnim.Play("flashlight_on_off", 0, 0);
                // lHandAnim.SetTrigger("lightOnOff");
                flashlight.triggerFlashlight();
                // flashlightLightSource.SetActive(!flashlightLightSource.activeSelf);
            }

            if (rHandFire)
            {
                RaycastHit hit;
                if (Physics.Raycast(pVisibles.transform.position, pVisibles.transform.forward, out hit, interactDistance))
                {

                    if(hit.collider.CompareTag("doorButton"))
                    {
                        rHandAnim.Play("hand_click", 0, 0);
                        // rHandAnim.SetTrigger("click");
                        hit.collider.GetComponent<DoorButton>().clickButton();
                    }
                    else if (hit.collider.CompareTag("strongDoor") || hit.collider.CompareTag("weakDoor"))
                    {
                        rHandAnim.Play("hand_door_open_close", 0, 0);
                        // rHandAnim.SetTrigger("doorOpenClose");
                        Door dcomp;
                        if (hit.collider.TryGetComponent<Door>(out dcomp))
                        {
                            dcomp.interactWithDoor(Characters.player);
                        }
                        else
                        {
                            hit.collider.GetComponent<DoubleDoor>().interactWithDoor(Characters.player);
                        }

                    }
                    else if (hit.collider.CompareTag("pickableBattery"))
                    {
                        rHandAnim.Play("hand_pick_item", 0, 0);
                        // rHandAnim.SetTrigger("pickItem");
                        hit.collider.GetComponent<PickableBattery>().pick();
                    }
                    else if (hit.collider.CompareTag("fanController"))
                    {
                        rHandAnim.Play("hand_click", 0, 0);
                        hit.collider.GetComponent<FanController>().triggerOnOff();
                    }
                    else if (hit.collider.CompareTag("lightButton"))
                    {
                        rHandAnim.Play("hand_click", 0, 0);
                        hit.collider.GetComponent<LightButton>().clickButton();
                    }
                    else if (hit.collider.CompareTag("tapePlayer"))
                    {
                        var tapePlayer = hit.collider.GetComponent<TapePlayer>();
                        if (!tapePlayer.m_audioSource.isPlaying)
                        {
                            rHandAnim.Play("hand_click", 0, 0);
                            tapePlayer.playerInteractPlay();
                        }
                        else
                        {
                            tapePlayer.playerInteractStopOrMissclick();
                        }
                    }
                    else if (hit.collider.CompareTag("lockedDoor"))
                    {
                        rHandAnim.Play("hand_door_open_close", 0, 0);
                        hit.collider.GetComponent<LockedDoor>().interactWithDoor(Characters.player);
                    }
                    else if (hit.collider.CompareTag("refrigeratorDoor"))
                    {
                        rHandAnim.Play("hand_door_open_close", 0, 0);
                        hit.collider.GetComponent<RefrigeratorDoor>().interactWithDoor();
                    }
                    else if (hit.collider.CompareTag("ovenDoor"))
                    {
                        rHandAnim.Play("hand_door_open_close", 0, 0);
                        hit.collider.GetComponent<OvenDoor>().interactWithDoor();
                    }
                    else if (hit.collider.CompareTag("stablet"))
                    {
                        rHandAnim.Play("hand_click", 0, 0);
                        hit.collider.GetComponent<SecurityCameraTablet>().playerInteract();
                    }
                }
            }


            bool rHandFireHold = Input.GetButton("Fire1");
            if (rHandFireHold)
            {
                RaycastHit hit;

                if (Physics.Raycast(pVisibles.transform.position, pVisibles.transform.forward, out hit, interactDistance))
                {
                    if (hit.collider.CompareTag("giftBox") && !inventory.isInventoryFull)
                    {
                        if (holdInteractCountdown <= holdInteractCountdownSeconds)
                        {
                            interactProgressBar.setProgressBarValue(holdInteractCountdown * 100 / holdInteractCountdownSeconds);
                            holdInteractCountdown += Time.deltaTime;
                        }
                        else
                        {
                            holdInteractCountdown = 0;
                            interactProgressBar.resetProgressBar();
                            hit.collider.GetComponent<GiftBox>().open();

                        }
                    }
                    else if (hit.collider.CompareTag("tape") && !inventory.isInventoryFull)
                    {
                        if (holdInteractCountdown <= holdInteractCountdownSeconds)
                        {
                            interactProgressBar.setProgressBarValue(holdInteractCountdown * 100 / holdInteractCountdownSeconds);
                            holdInteractCountdown += Time.deltaTime;
                        }
                        else
                        {
                            holdInteractCountdown = 0;
                            interactProgressBar.resetProgressBar();
                            // Pick the tape
                            hit.collider.GetComponent<Tape>().pickIt();


                            if (hit.collider.GetComponent<Rigidbody>() == null)
                            {
                                hit.collider.gameObject.AddComponent<Rigidbody>();
                            }

                            var notify = new Notification();
                            notify.topic = "Tape Picked Up";
                            notify.description = "You can play tape on a tape player.";
                            notificationPresenter.addNotification(notify);
                        }
                    }
                    else if (hit.collider.CompareTag("doorKey") && !inventory.isInventoryFull)
                    {
                        if (holdInteractCountdown <= holdInteractCountdownSeconds)
                        {
                            interactProgressBar.setProgressBarValue(holdInteractCountdown * 100 / holdInteractCountdownSeconds);
                            holdInteractCountdown += Time.deltaTime;
                        }
                        else
                        {
                            holdInteractCountdown = 0;
                            interactProgressBar.resetProgressBar();
                            // Pick the door key
                            hit.collider.GetComponent<DoorKey>().pickIt();


                            if (hit.collider.GetComponent<Rigidbody>() == null)
                            {
                                hit.collider.gameObject.AddComponent<Rigidbody>();
                            }

                            var notify = new Notification();
                            notify.topic = "Door Key Picked Up";
                            notify.description = string.Format("You can unlock {0} key required doors.", hit.collider.GetComponent<DoorKey>().keyColor.ToString());
                            notificationPresenter.addNotification(notify);
                        }
                    }
                    else if (hit.collider.CompareTag("drink") && !inventory.isInventoryFull)
                    {
                        if (holdInteractCountdown <= holdInteractCountdownSeconds)
                        {
                            interactProgressBar.setProgressBarValue(holdInteractCountdown * 100 / holdInteractCountdownSeconds);
                            holdInteractCountdown += Time.deltaTime;
                        }
                        else
                        {
                            holdInteractCountdown = 0;
                            interactProgressBar.resetProgressBar();
                            // Pick the door key
                            hit.collider.GetComponent<Drink>().pickIt();


                            if (hit.collider.GetComponent<Rigidbody>() == null)
                            {
                                hit.collider.gameObject.AddComponent<Rigidbody>();
                            }

                            var notify = new Notification();
                            notify.topic = "Drink Picked Up";
                            notify.description = "Doesn't does much. Just a drink you can throw.";
                            notificationPresenter.addNotification(notify);
                        }
                    }
                    else if (hit.collider.CompareTag("pizza") && !inventory.isInventoryFull)
                    {
                        if (holdInteractCountdown <= holdInteractCountdownSeconds)
                        {
                            interactProgressBar.setProgressBarValue(holdInteractCountdown * 100 / holdInteractCountdownSeconds);
                            holdInteractCountdown += Time.deltaTime;
                        }
                        else
                        {
                            holdInteractCountdown = 0;
                            interactProgressBar.resetProgressBar();
                            // Pick the door key
                            hit.collider.GetComponent<Pizza>().pickIt();


                            if (hit.collider.GetComponent<Rigidbody>() == null)
                            {
                                hit.collider.gameObject.AddComponent<Rigidbody>();
                            }

                            var notify = new Notification();
                            notify.topic = "Pizza Picked Up";
                            notify.description = "Mmmmhhmm. Yummy.";
                            notificationPresenter.addNotification(notify);
                        }
                    }
                    else if (hit.collider.CompareTag("plasticPlate") && !inventory.isInventoryFull)
                    {
                        if (holdInteractCountdown <= holdInteractCountdownSeconds)
                        {
                            interactProgressBar.setProgressBarValue(holdInteractCountdown * 100 / holdInteractCountdownSeconds);
                            holdInteractCountdown += Time.deltaTime;
                        }
                        else
                        {
                            holdInteractCountdown = 0;
                            interactProgressBar.resetProgressBar();
                            // Pick the door key
                            hit.collider.GetComponent<PlasticPlate>().pickIt();


                            if (hit.collider.GetComponent<Rigidbody>() == null)
                            {
                                hit.collider.gameObject.AddComponent<Rigidbody>();
                            }

                            var notify = new Notification();
                            notify.topic = "Plastic Plate Picked Up";
                            notify.description = "Who eats his meal over the public pizza box?";
                            notificationPresenter.addNotification(notify);
                        }
                    }
                    else if (hit.collider.CompareTag("plasticFork") && !inventory.isInventoryFull)
                    {
                        if (holdInteractCountdown <= holdInteractCountdownSeconds)
                        {
                            interactProgressBar.setProgressBarValue(holdInteractCountdown * 100 / holdInteractCountdownSeconds);
                            holdInteractCountdown += Time.deltaTime;
                        }
                        else
                        {
                            holdInteractCountdown = 0;
                            interactProgressBar.resetProgressBar();
                            // Pick the door key
                            hit.collider.GetComponent<PlasticFork>().pickIt();


                            if (hit.collider.GetComponent<Rigidbody>() == null)
                            {
                                hit.collider.gameObject.AddComponent<Rigidbody>();
                            }

                            var notify = new Notification();
                            notify.topic = "Plastic Fork Picked Up";
                            notify.description = "Three spikes with holder called fork.";
                            notificationPresenter.addNotification(notify);
                        }
                    }
                    else if (hit.collider.CompareTag("partyHat") && !inventory.isInventoryFull)
                    {
                        if (holdInteractCountdown <= holdInteractCountdownSeconds)
                        {
                            interactProgressBar.setProgressBarValue(holdInteractCountdown * 100 / holdInteractCountdownSeconds);
                            holdInteractCountdown += Time.deltaTime;
                        }
                        else
                        {
                            holdInteractCountdown = 0;
                            interactProgressBar.resetProgressBar();
                            // Pick the door key
                            hit.collider.GetComponent<PartyHat>().pickIt();


                            if (hit.collider.GetComponent<Rigidbody>() == null)
                            {
                                hit.collider.gameObject.AddComponent<Rigidbody>();
                            }

                            var notify = new Notification();
                            notify.topic = "Party Hat Picked Up";
                            notify.description = "Party Time!";
                            notificationPresenter.addNotification(notify);
                        }
                    }

                }
                else
                {
                    holdInteractCountdown = 0;
                    interactProgressBar.resetProgressBar();
                }
                

            }
            else
            {
                holdInteractCountdown = 0;
                interactProgressBar.resetProgressBar();
            }
        }




    }
}
