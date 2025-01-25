using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;

    public float walkBobSpeed = 10f;
    public float walkBobAmount = 0.05f;

    public AudioClip[] footstepSounds;
    public float stepInterval = 0.5f;

    private Rigidbody rb;
    private float rotationX = 0f;
    private float bobTimer = 0f;
    private Vector3 initialCameraPosition;
    private AudioSource audioSource;
    private float stepTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        initialCameraPosition = Camera.main.transform.localPosition;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleCameraBob();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed;
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f;
            }
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    private void HandleCameraBob()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f)
        {
            bobTimer += Time.deltaTime * walkBobSpeed;
            float offsetY = Mathf.Sin(bobTimer) * walkBobAmount;
            Camera.main.transform.localPosition = initialCameraPosition + new Vector3(0f, offsetY, 0f);
        }
        else
        {
            bobTimer = 0f;
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, initialCameraPosition, Time.deltaTime * walkBobSpeed);
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
