using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public AudioClip footstepSound;
    public float walkingSpeed = 5f;
    public float sprintingSpeed = 7.5f;

    private AudioSource audioSource;
    private Vector3 lastPosition;
    private float lastFootstepTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastPosition = transform.position;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.y = lastPosition.y; 
        float distanceMoved = Vector3.Distance(currentPosition, lastPosition);

        if (distanceMoved > 0f)
        {
            Debug.Log("Should be playing sound");
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintingSpeed : walkingSpeed;
            if (Time.time - lastFootstepTime > 1f / (currentSpeed / 3f))
            {
                PlayFootstepSound();
                lastFootstepTime = Time.time;
            }
        }

        lastPosition = currentPosition;
    }

    private void PlayFootstepSound()
    {
        if (footstepSound != null)
        {
            audioSource.clip = footstepSound;
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
        }
    }
}
