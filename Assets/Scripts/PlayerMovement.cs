// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     public float speed = 5f;
//     public float mouseSensitivity = 2f;
    
//     private CharacterController controller;
//     private Transform cameraTransform;
//     private float verticalRotation = 0f;

//     void Start()
//     {
//         controller = GetComponent<CharacterController>();
//         // Mengunci kursor mouse di tengah layar saat main
//         Cursor.lockState = CursorLockMode.Locked; 
        
//         // Mengambil referensi kamera (anak dari player)
//         cameraTransform = GetComponentInChildren<Camera>().transform;
//     }

//     void Update()
//     {
//         // 1. Logika Putar Kepala (Mouse Look)
//         float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
//         float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

//         verticalRotation -= mouseY;
//         verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // Batasi nunduk/ndangak

//         cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
//         transform.Rotate(Vector3.up * mouseX);

//         // 2. Logika Jalan (WASD)
//         float x = Input.GetAxis("Horizontal"); // A & D
//         float z = Input.GetAxis("Vertical");   // W & S

//         Vector3 move = transform.right * x + transform.forward * z;
//         controller.Move(move * speed * Time.deltaTime);
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
    
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaDrain = 20f; // Cepat capek
    public float staminaRegen = 10f; // Cepat pulih
    public Slider staminaBar; 
    
    [Header("Exhaustion Logic")]
    // Jika true, player sedang ngos-ngosan dan tidak bisa lari
    private bool isExhausted = false; 

    private float currentStamina;
    private CharacterController controller;
    private Transform cameraTransform;
    private float verticalRotation = 0f;
    
    // Warna Bar biar kelihatan kapan boleh lari
    private Image staminaFillImage; 

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
            
            // Cari gambar pengisi slider untuk diubah warnanya nanti
            if (staminaBar.fillRect != null)
                staminaFillImage = staminaBar.fillRect.GetComponent<Image>();
        }
    }

    void Update()
    {
        // 1. Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // 2. Logic Movement & Stamina
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical"); 
        bool isMoving = (x != 0 || z != 0);

        // --- LOGIKA BARU: KELELAHAN ---
        
        // Cek apakah lari? (Hanya boleh jika Shift ditekan, bergerak, punya stamina, DAN TIDAK CAPEK)
        bool isRunningInput = Input.GetKey(KeyCode.LeftShift);
        bool canRun = currentStamina > 0 && !isExhausted; 
        
        bool isSprinting = isMoving && isRunningInput && canRun;

        float currentSpeed = walkSpeed;

        if (isSprinting)
        {
            currentSpeed = runSpeed;
            currentStamina -= staminaDrain * Time.deltaTime;

            // Jika stamina habis total -> Masuk mode Exhausted (Kecapekan)
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isExhausted = true; 
                // Opsional: Play sound effect napas berat disini
            }
        }
        else
        {
            // Regen Stamina
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegen * Time.deltaTime;
            }

            // Kapan sembuh dari capek? Kalau stamina sudah isi 25%
            if (isExhausted && currentStamina > (maxStamina * 0.25f))
            {
                isExhausted = false;
            }
        }

        // 3. Update Visual Bar
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina;
            
            // Ubah warna bar: Merah kalau kecapekan, Putih kalau sehat
            if (staminaFillImage != null)
            {
                staminaFillImage.color = isExhausted ? Color.red : Color.white;
            }
        }

        // 4. Move Character
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }
}