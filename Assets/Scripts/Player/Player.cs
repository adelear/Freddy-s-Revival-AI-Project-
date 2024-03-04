using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private Rigidbody rb;
    [SerializeField] private Camera playerCamera;
    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (GameManager.Instance.GetGameState() == GameManager.GameState.GAME)
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

            playerCamera.transform.rotation = Quaternion.Euler(desiredX, cameraRotation.y, cameraRotation.z);
            rb.MovePosition(rb.position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
        }
    }
}
