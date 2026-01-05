// using UnityEngine;
// using UnityEngine.UI;

// public class PlayerMovement : MonoBehaviour
// {
//     [Header("Movement Settings")]
//     public float walkSpeed = 5f;
//     public float runSpeed = 10f; 
//     public float mouseSensitivity = 2f;
//     public float gravity = -9.81f; 
    
//     [Header("Stamina Settings")]
//     public float maxStamina = 100f;
//     public float staminaDrain = 20f; 
//     public float staminaRegen = 10f; 
//     public Slider staminaBar; 
    
//     [Header("Audio Langkah Kaki")]
//     public AudioSource footstepSource; 
//     public AudioClip footstepClip;     

//     private bool isExhausted = false; 
//     private float currentStamina;
//     private CharacterController controller;
//     private Transform cameraTransform;
//     private float verticalRotation = 0f;
//     private Image staminaFillImage; 
//     private Vector3 velocity; 

//     void Start()
//     {
//         controller = GetComponent<CharacterController>();
//         Cursor.lockState = CursorLockMode.Locked; 
//         cameraTransform = GetComponentInChildren<Camera>().transform;
        
//         currentStamina = maxStamina;
//         if (staminaBar != null)
//         {
//             staminaBar.maxValue = maxStamina;
//             staminaBar.value = currentStamina;
//             if (staminaBar.fillRect != null)
//                 staminaFillImage = staminaBar.fillRect.GetComponent<Image>();
//         }

//         if (footstepSource == null) footstepSource = GetComponent<AudioSource>();
//         if (footstepSource != null && footstepClip != null)
//         {
//             footstepSource.clip = footstepClip;
//             footstepSource.loop = true; 
//             footstepSource.Stop();      
//         }
//     }

//     void Update()
//     {
//         float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
//         float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
//         verticalRotation -= mouseY;
//         verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
//         cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
//         transform.Rotate(Vector3.up * mouseX);

//         float x = Input.GetAxis("Horizontal"); 
//         float z = Input.GetAxis("Vertical"); 
//         bool isMoving = (x != 0 || z != 0);
//         bool isRunningInput = Input.GetKey(KeyCode.LeftShift);
//         bool canRun = currentStamina > 0 && !isExhausted; 
//         bool isSprinting = isMoving && isRunningInput && canRun;
//         float currentSpeed = isSprinting ? runSpeed : walkSpeed;

//         Vector3 move = transform.right * x + transform.forward * z;
//         move *= currentSpeed; 

//         if (controller.isGrounded && velocity.y < 0) velocity.y = -2f; 
//         velocity.y += gravity * Time.deltaTime;
//         move.y = velocity.y; 
//         controller.Move(move * Time.deltaTime);

//         if (isSprinting)
//         {
//             currentStamina -= staminaDrain * Time.deltaTime;
//             if (currentStamina <= 0) { currentStamina = 0; isExhausted = true; }
//         }
//         else
//         {
//             if (currentStamina < maxStamina) currentStamina += staminaRegen * Time.deltaTime;
//             if (isExhausted && currentStamina > (maxStamina * 0.25f)) isExhausted = false;
//         }

//         if (staminaBar != null)
//         {
//             staminaBar.value = currentStamina;
//             if (staminaFillImage != null) staminaFillImage.color = isExhausted ? Color.red : Color.white;
//         }

//         if (footstepSource != null && footstepClip != null)
//         {
//             if (isMoving && controller.isGrounded)
//             {
//                 if (!footstepSource.isPlaying) footstepSource.Play();
//                 footstepSource.pitch = isSprinting ? 1.5f : 1.0f;
//             }
//             else { if (footstepSource.isPlaying) footstepSource.Stop(); }
//         }
//     }
// }










using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f; 
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f; 
    
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaDrain = 20f; 
    public float staminaRegen = 10f; 
    public Slider staminaBar; 
    
    [Header("Audio Langkah Kaki")]
    public AudioSource footstepSource; 
    public AudioClip footstepClip;     

    private bool isExhausted = false; 
    private float currentStamina;
    private CharacterController controller;
    private Transform cameraTransform;
    private float verticalRotation = 0f;
    private Image staminaFillImage; 
    private Vector3 velocity; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; 
        cameraTransform = GetComponentInChildren<Camera>().transform;
        
        currentStamina = maxStamina;
        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
            if (staminaBar.fillRect != null)
                staminaFillImage = staminaBar.fillRect.GetComponent<Image>();
        }

        if (footstepSource == null) footstepSource = GetComponent<AudioSource>();
        if (footstepSource != null && footstepClip != null)
        {
            footstepSource.clip = footstepClip;
            footstepSource.loop = true; 
            footstepSource.Stop();      
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical"); 
        bool isMoving = (x != 0 || z != 0);
        bool isRunningInput = Input.GetKey(KeyCode.LeftShift);
        bool canRun = currentStamina > 0 && !isExhausted; 
        bool isSprinting = isMoving && isRunningInput && canRun;
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        move *= currentSpeed; 

        if (controller.isGrounded && velocity.y < 0) velocity.y = -2f; 
        velocity.y += gravity * Time.deltaTime;
        move.y = velocity.y; 
        controller.Move(move * Time.deltaTime);

        if (isSprinting)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            if (currentStamina <= 0) { currentStamina = 0; isExhausted = true; }
        }
        else
        {
            if (currentStamina < maxStamina) currentStamina += staminaRegen * Time.deltaTime;
            if (isExhausted && currentStamina > (maxStamina * 0.25f)) isExhausted = false;
        }

        if (staminaBar != null)
        {
            staminaBar.value = currentStamina;
            if (staminaFillImage != null) staminaFillImage.color = isExhausted ? Color.red : Color.white;
        }

        if (footstepSource != null && footstepClip != null)
        {
            if (isMoving && controller.isGrounded)
            {
                if (!footstepSource.isPlaying) footstepSource.Play();
                footstepSource.pitch = isSprinting ? 1.5f : 1.0f;
            }
            else { if (footstepSource.isPlaying) footstepSource.Stop(); }
        }
    }
}