// using UnityEngine;
// using UnityEngine.UI;

// public class PlayerMovement : MonoBehaviour
// {
//     [Header("Movement Settings")]
//     public float walkSpeed = 5f;
//     public float runSpeed = 10f; 
//     public float mouseSensitivity = 2f;
    
//     [Header("Stamina Settings")]
//     public float maxStamina = 100f;
//     public float staminaDrain = 20f; // Cepat capek
//     public float staminaRegen = 10f; // Cepat pulih
//     public Slider staminaBar; 
    
//     [Header("Exhaustion Logic")]
//     // Jika true, player sedang ngos-ngosan dan tidak bisa lari
//     private bool isExhausted = false; 

//     private float currentStamina;
//     private CharacterController controller;
//     private Transform cameraTransform;
//     private float verticalRotation = 0f;
    
//     // Warna Bar biar kelihatan kapan boleh lari
//     private Image staminaFillImage; 

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
            
//             // Cari gambar pengisi slider untuk diubah warnanya nanti
//             if (staminaBar.fillRect != null)
//                 staminaFillImage = staminaBar.fillRect.GetComponent<Image>();
//         }
//     }

//     void Update()
//     {
//         // 1. Mouse Look
//         float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
//         float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
//         verticalRotation -= mouseY;
//         verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
//         cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
//         transform.Rotate(Vector3.up * mouseX);

//         // 2. Logic Movement & Stamina
//         float x = Input.GetAxis("Horizontal"); 
//         float z = Input.GetAxis("Vertical"); 
//         bool isMoving = (x != 0 || z != 0);

//         // --- LOGIKA BARU: KELELAHAN ---
        
//         // Cek apakah lari? (Hanya boleh jika Shift ditekan, bergerak, punya stamina, DAN TIDAK CAPEK)
//         bool isRunningInput = Input.GetKey(KeyCode.LeftShift);
//         bool canRun = currentStamina > 0 && !isExhausted; 
        
//         bool isSprinting = isMoving && isRunningInput && canRun;

//         float currentSpeed = walkSpeed;

//         if (isSprinting)
//         {
//             currentSpeed = runSpeed;
//             currentStamina -= staminaDrain * Time.deltaTime;

//             // Jika stamina habis total -> Masuk mode Exhausted (Kecapekan)
//             if (currentStamina <= 0)
//             {
//                 currentStamina = 0;
//                 isExhausted = true; 
//                 // Opsional: Play sound effect napas berat disini
//             }
//         }
//         else
//         {
//             // Regen Stamina
//             if (currentStamina < maxStamina)
//             {
//                 currentStamina += staminaRegen * Time.deltaTime;
//             }

//             // Kapan sembuh dari capek? Kalau stamina sudah isi 25%
//             if (isExhausted && currentStamina > (maxStamina * 0.25f))
//             {
//                 isExhausted = false;
//             }
//         }

//         // 3. Update Visual Bar
//         if (staminaBar != null)
//         {
//             staminaBar.value = currentStamina;
            
//             // Ubah warna bar: Merah kalau kecapekan, Putih kalau sehat
//             if (staminaFillImage != null)
//             {
//                 staminaFillImage.color = isExhausted ? Color.red : Color.white;
//             }
//         }

//         // 4. Move Character
//         Vector3 move = transform.right * x + transform.forward * z;
//         controller.Move(move * currentSpeed * Time.deltaTime);
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
    
    // Variabel Gravitasi
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
        // 1. Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // --- 2. LOGIKA PERGERAKAN (DIGABUNG) ---
        
        // A. Cek Input Horizontal
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical"); 
        
        // B. Cek Lari/Jalan
        bool isMoving = (x != 0 || z != 0);
        bool isRunningInput = Input.GetKey(KeyCode.LeftShift);
        bool canRun = currentStamina > 0 && !isExhausted; 
        bool isSprinting = isMoving && isRunningInput && canRun;
        float currentSpeed = isSprinting ? runSpeed : walkSpeed;

        // C. Hitung Gerakan Maju/Samping
        Vector3 move = transform.right * x + transform.forward * z;
        move *= currentSpeed; // Kalikan kecepatan

        // D. Hitung Gravitasi (Jatuh)
        // Reset kecepatan jatuh kalau sudah di lantai
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Tekan sedikit ke lantai agar nempel
        }
        velocity.y += gravity * Time.deltaTime;

        // E. GABUNGKAN (X, Z + Y Gravitasi)
        move.y = velocity.y; 

        // F. EKSEKUSI BERGERAK (Hanya 1 kali panggil agar isGrounded akurat)
        controller.Move(move * Time.deltaTime);


        // --- 3. LOGIKA STAMINA ---
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


        // --- 4. LOGIKA AUDIO ---
        if (footstepSource != null && footstepClip != null)
        {
            // Sekarang kita cek: Apakah Player Bergerak DAN (Ada di Tanah ATAU Kecepatannya lambat - asumsi di lantai)
            if (isMoving && controller.isGrounded)
            {
                if (!footstepSource.isPlaying) footstepSource.Play();
                footstepSource.pitch = isSprinting ? 1.5f : 1.0f;
            }
            else
            {
                // Stop jika diam atau melompat/jatuh
                if (footstepSource.isPlaying) footstepSource.Stop();
            }
        }
    }
}