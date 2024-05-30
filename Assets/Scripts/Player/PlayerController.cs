using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float moveSpeed;

    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    [SerializeField] private float lookSensitivity;

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooldown;

    [Header("Weapon Hold Point")]
    [SerializeField] public Transform weaponHoldPoint;

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector3 dashDirection;

    private bool canLook;

    private Vector2 currentMovementInput;
    private Vector2 mouseDelta;
    private float cameraCurrentRotation;

    private Rigidbody rigbody;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
        rigbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        canLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        isDashing = false;
        dashTimer = 0;
        dashCooldownTimer = 0;


    }

    private void Update()
    {
        HandleInput();
        HandleDashCooldown();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
            Move();
        else
            rigbody.velocity = dashDirection * dashSpeed;

        if (canLook)
            CameraLook();
    }

    private void HandleInput()
    {
        currentMovementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
            Dash();
    }

    private void Move()
    {
        Vector3 moveDirection = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x;
        moveDirection *= moveSpeed * Time.fixedDeltaTime;

        rigbody.MovePosition(rigbody.position + moveDirection);
    }

    private void Dash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        dashDirection = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x;
        if (dashDirection == Vector3.zero)
            dashDirection = transform.forward;

        dashDirection.Normalize();

        rigbody.velocity = dashDirection * dashSpeed;
    }

    private void HandleDashCooldown()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
                isDashing = false;
                rigbody.velocity = Vector3.zero;
        }
        else if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
    }

    private void CameraLook()
    {
        cameraCurrentRotation -= mouseDelta.y * lookSensitivity;
        cameraCurrentRotation = Mathf.Clamp(cameraCurrentRotation, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(cameraCurrentRotation, 0, 0);

        transform.Rotate(Vector3.up * mouseDelta.x * lookSensitivity);
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
