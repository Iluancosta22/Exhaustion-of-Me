using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    #region Fields

    [Header("Status")]
    public bool alive = true;

    [Header("Movement")]
    [SerializeField] private float walkSpeed  = 4f;
    [SerializeField] private float runSpeed   = 8f;
    [SerializeField] private float gravity    = -19.62f;

    [Header("Look")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float     mouseSensitivity = 2f;
    [SerializeField] private float     raycastDistance;

    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standHeight  = 2f;
    [SerializeField] private float crouchSpeed  = 2f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Canvas")]
    [SerializeField] private GameObject DoorInteract;

    private CharacterController controller;
    private Vector3 velocity;
    private float   xRotation;
    private bool    isCrouching;
    private bool    isRunning;
    private bool    isWalking;
    private bool    isInteract;
    private RaycastHit hit;

    public bool Running => isRunning;
    public bool Walking => isWalking;


    private GameInput                input;
    private GameInput.PlayerActions  player;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        input  = new GameInput();
        player = input.Player;

        player.Crouch.performed += _ => isCrouching = !isCrouching;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;

        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
    }

    private void OnEnable()  => player.Enable();
    private void OnDisable() => player.Disable();

    private void Update()
    {
        if (!alive) return;

        HandleLook();
        HandleMovement();
        HandleCrouch();
        HandleRaycast();
        HandleInteract();
    }

    #endregion

    #region Look

    private void HandleLook()
    {
        Vector2 lookInput = player.Look.ReadValue<Vector2>() * mouseSensitivity;

        xRotation = Mathf.Clamp(xRotation - lookInput.y, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookInput.x);
    }

    #endregion

    #region Movement

    private void HandleMovement()
    {
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        Vector2 moveInput = player.Move.ReadValue<Vector2>();
        
        isWalking = moveInput.magnitude > 0.1f;
        
        isRunning = player.Sprint.IsPressed() && !isCrouching && moveInput.y > 0.1f;
        
        float speed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    #endregion

    #region Crouch

    private void HandleCrouch()
    {
        float targetHeight = isCrouching ? crouchHeight : standHeight;
        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * 10f);

        controller.center = new Vector3(0f, isCrouching ? 0.5f : 0f, 0f);
    }

    #endregion

    #region Interaction

    private void HandleRaycast()
    {
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * raycastDistance, Color.red);

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, raycastDistance))
        {
            if(hit.collider.GetComponent<IInteract>() != null)
            {
                DoorInteract.SetActive(true);
                isInteract = true;
            }
        }
        else
        {
            isInteract = false;
            DoorInteract.SetActive(false);
        }
    }

    private void HandleInteract()
    {
        if (!isInteract || !player.Interact.WasPressedThisFrame()) return;

        hit.collider.GetComponent<IInteract>()?.Interact();
        AnimationTake();
    }

    #endregion

    #region Animation

    private void AnimationTake()
    {
        animator?.SetTrigger("Take");
    }

    #endregion

    #region Public API

    public void Die()
    {
        alive = false;
        animator?.SetTrigger("Dead");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }

    #endregion
}