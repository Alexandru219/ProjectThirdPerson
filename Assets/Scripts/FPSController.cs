using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 9f;
    [SerializeField] private float groundAcceleration = 15f;

    [Header("Jump & Gravity")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private float groundCheckDistance = 1.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Look")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float gamepadSensitivity = 2f;
    [SerializeField] private float verticalClamp = 85f;

    [Header("Head Bob")]
    [SerializeField] private bool enableHeadBob = true;
    [SerializeField] private float bobFrequency = 2.5f;
    [SerializeField] private float bobAmplitude = 0.05f;

    [Header("References")]
    [SerializeField] private Transform playerBody;   // rotația pe orizontală
    [SerializeField] private Transform cameraPivot;  // rotația pe verticală

    [Header("Input")]
    [SerializeField] private InputActionReference lookAction;

    [Header("Settings")]
    [SerializeField] private float sensitivity = 180f;
    [SerializeField] private bool invertY = false;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;
    [SerializeField] private float gamepadLookSpeed = 180f;
    
    private CharacterController _cc;

    // Movement state
    private Vector2 _moveCommand;
    private Vector3 _velocity;           
    private Vector3 _smoothMoveVelocity; 
    private Vector3 _currentXZ;
    private float _currentSpeed;
    private bool _isSprintingCommand;

    // Look state
    private float _xRotation;           

    // Jump state
    private bool _jumpCommand;
    public bool _isGrounded;

    // Head Bob state
    private float _bobTimer;
    private Vector3 _bobOffset;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _currentSpeed = walkSpeed;

        if (cameraHolder == null)
        {
            Debug.LogWarning("[FPSController] CameraHolder nu este setat!");
        }
    }

    private void Update()
    {
        CheckGround();
        ApplyMovement();
        ApplyGravityAndJump();
        ApplyHeadBob();
        
        if (TrackerDeviceManager.Instance.CurrentDevice == TrackerDeviceManager.DeviceType.Gamepad)
        {
            ExecuteGamepadLook(InputReaderNew.Instance.LookValue);
        }
    }


    public void SetMovementCommand(Vector2 moveInput)
    {
        _moveCommand = moveInput;
    }

    public void SetSprintCommand(bool isSprinting)
    {
        _isSprintingCommand = isSprinting;
    }

    public void ExecuteJump()
    {
        if (_isGrounded)
        {
            _jumpCommand = true;
        }
    }

    public void ExecuteLook(Vector2 lookDelta)
    {
        if (cameraHolder == null) return;

        if (TrackerDeviceManager.Instance.CurrentDevice != TrackerDeviceManager.DeviceType.KeyboardMouse) return;
        float mouseX = lookDelta.x * mouseSensitivity;
        float mouseY = lookDelta.y * mouseSensitivity;

        // Rotație orizontală
        transform.Rotate(Vector3.up * mouseX);

        // Rotație verticală (camera)
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -verticalClamp, verticalClamp);
        cameraHolder.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }
    

    private float pitch;
    private void OnEnable()
    {
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        lookAction.action.Disable();
    }
    public void ExecuteGamepadLook(Vector2 lookInput)
    {
        if (cameraHolder == null) return;

        if (TrackerDeviceManager.Instance.CurrentDevice != TrackerDeviceManager.DeviceType.Gamepad)
            return;

        float stickX = lookInput.x * gamepadLookSpeed * Time.deltaTime;
        float stickY = lookInput.y * gamepadLookSpeed * Time.deltaTime;

        // rotație orizontală
        transform.Rotate(Vector3.up * stickX);

        // rotație verticală
        _xRotation -= stickY;
        _xRotation = Mathf.Clamp(_xRotation, -verticalClamp, verticalClamp);

        cameraHolder.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    public void ResetMovementState()
    {
        _moveCommand = Vector2.zero;
        _isSprintingCommand = false;
        _jumpCommand = false;
    }



    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );

        if (_isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;
    }

    private void ApplyMovement()
    {
        _currentSpeed = _isSprintingCommand ? sprintSpeed : walkSpeed;

        Vector3 targetMove = transform.right * _moveCommand.x + transform.forward * _moveCommand.y;

        if (targetMove.magnitude > 1f)
            targetMove.Normalize();

        targetMove *= _currentSpeed;

        float smoothTime = 1f / groundAcceleration;
        _currentXZ = Vector3.SmoothDamp(_currentXZ, targetMove, ref _smoothMoveVelocity, smoothTime);

        _velocity.x = _currentXZ.x;
        _velocity.z = _currentXZ.z;
    }

    private void ApplyGravityAndJump()
    {
        if (_jumpCommand && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight);
            _jumpCommand = false; // Resetăm comanda
        }

        _velocity.y += gravity * Time.deltaTime;
        _cc.Move(_velocity * Time.deltaTime);
    }

    private void ApplyHeadBob()
    {
        if (!enableHeadBob || cameraHolder == null) return;

        bool isMoving = _moveCommand.sqrMagnitude > 0.01f && _isGrounded;

        if (isMoving)
        {
            float speed = _isSprintingCommand ? sprintSpeed : walkSpeed;
            _bobTimer += Time.deltaTime * bobFrequency * (speed / walkSpeed);

            _bobOffset = new Vector3(
                Mathf.Sin(_bobTimer * 2f) * bobAmplitude * 0.5f,
                Mathf.Abs(Mathf.Sin(_bobTimer)) * bobAmplitude,
                0f
            );
        }
        else
        {
            _bobTimer = 0f;
            _bobOffset = Vector3.Lerp(_bobOffset, Vector3.zero, Time.deltaTime * 8f);
        }

        Vector3 basePos = new Vector3(0f, _cc.height * 0.4f, 0f); 
        cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, basePos + _bobOffset, Time.deltaTime * 15f);
    }

    private void OnDrawGizmosSelected()
    {
        if (_cc == null) _cc = GetComponent<CharacterController>();
        if (_cc == null) return;
        
        Gizmos.color = Color.green;
        Vector3 spherePos = transform.position + Vector3.down * (_cc.height * 0.5f - groundCheckRadius);
        Gizmos.DrawWireSphere(spherePos, groundCheckRadius);
    }
}