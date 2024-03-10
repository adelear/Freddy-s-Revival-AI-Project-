using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private Rigidbody rb;
    private Camera playerCamera;
    private Vector3 moveDirection = Vector3.zero;

    [Header("Sprint Components")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float sprintSpeed = 7.5f; 
    public float walkSpeed = 5f;
    public float exhaustionDuration = 3f;
    public bool isExhausted = false;
    private float staminaDepletioRate = 10f;
    private float staminaRechargeRate = 10f;
    
    [Header("Crouch Components")]
    private bool isCrouched;
    public float regularHeight = 1.6f;
    public float crouchedHeight = 0.5f;

    public bool isHidden;  

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        currentStamina = maxStamina;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void MovementAndLook()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        Vector3 cameraRotation = playerCamera.transform.rotation.eulerAngles;
        float desiredX = cameraRotation.x - mouseY;

        if (desiredX > 180f)
            desiredX -= 360f;

        desiredX = Mathf.Clamp(desiredX, -90f, 90f);
        if (horizontalInput == 0) rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);
        if (verticalInput == 0) rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        moveSpeed = Input.GetKey(KeyCode.LeftShift) && !isExhausted ? sprintSpeed : walkSpeed;
        playerCamera.transform.rotation = Quaternion.Euler(desiredX, cameraRotation.y, cameraRotation.z);
        rb.MovePosition(rb.position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
    }

    private void HandleStamina()
    {
        if (isExhausted) return;

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            currentStamina -= Time.deltaTime * staminaDepletioRate; 
            if (currentStamina <= 0) StartCoroutine(ExhaustionCoroutine());
            
        }
        else if (currentStamina < maxStamina) currentStamina += Time.deltaTime * staminaRechargeRate; 
    }

    IEnumerator ExhaustionCoroutine()
    {
        isExhausted = true;
        yield return new WaitForSeconds(exhaustionDuration);
        isExhausted = false;
    }

    private IEnumerator AdjustHeight(float startHeight, float targetHeight, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float newHeight = Mathf.Lerp(startHeight, targetHeight, t);
            transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
    }

    private void HandleCrouched()
    {
        if (isHidden) return; 
        if (!isCrouched && Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouched = true;
            StartCoroutine(AdjustHeight(regularHeight, crouchedHeight, 0.15f)); 
        }
        else if (isCrouched && Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouched = false;
            StartCoroutine(AdjustHeight(crouchedHeight, regularHeight, 0.15f)); 
        }
    }


    private void Update()
    {
        if (GameManager.Instance.GetGameState() == GameManager.GameState.GAME)
        {
            Cursor.lockState = CursorLockMode.Locked;
            MovementAndLook();
            HandleStamina();
            HandleCrouched();
        }
        else Cursor.lockState = CursorLockMode.Confined;
        //Debug.Log($"{currentStamina} / {maxStamina}");
    }
}

