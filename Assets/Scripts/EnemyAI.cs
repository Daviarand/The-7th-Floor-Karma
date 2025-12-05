// using UnityEngine;
// using UnityEngine.AI;

// public class EnemyAI : MonoBehaviour
// {
//     public Transform player;
//     public Transform playerCamera;
    
//     // Settingan Angel
//     public float normalSpeed = 3.5f;
//     public float weepingSpeed = 10f; 
//     public bool isWeepingMode = false;

//     private NavMeshAgent agent;
//     public Renderer angelRenderer; 

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
        
//         // --- UBAH BARIS INI ---
//         // Ganti dari GetComponent<Renderer>() menjadi:
//         angelRenderer = GetComponentInChildren<Renderer>(); 
//         // ----------------------

//         // (Sisa kodingan biarkan sama)
//         if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
//         if (playerCamera == null) playerCamera = Camera.main.transform;
//         agent.autoBraking = false; 
//     }

//     void Update()
//     {
//         if (player == null) return;

//         // Pastikan NavMeshAgent aktif dan terpasang di NavMesh
//         if (!agent.isOnNavMesh) return;

//         // FASE 1: MODE NORMAL (KEJAR TERUS)
//         if (!isWeepingMode)
//         {
//             agent.speed = normalSpeed;
//             agent.isStopped = false;
//             agent.SetDestination(player.position);
//         }
//         // FASE 2: MODE WEEPING (MARAH)
//         else
//         {
//             bool terlihat = IsVisibleToPlayer();

//             if (terlihat)
//             {
//                 // Dilihat -> MEMBATU
//                 agent.isStopped = true;
//                 agent.velocity = Vector3.zero; // Paksa berhenti total fisik
//                 angelRenderer.material.color = Color.gray; 
//             }
//             else
//             {
//                 // Tidak Dilihat -> LARI KENCANG
//                 agent.isStopped = false;
//                 agent.speed = weepingSpeed;
//                 agent.SetDestination(player.position);
//                 angelRenderer.material.color = Color.red; 
//             }
//         }
//     }

//     bool IsVisibleToPlayer()
//     {
//         // Cek apakah masuk layar
//         Vector3 viewPos = playerCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
//         bool onScreen = viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && viewPos.z > 0;

//         if (onScreen)
//         {
//             // Cek Raycast (Apakah terhalang tembok?)
//             RaycastHit hit;
//             // Tembak dari kamera ke titik tengah badan Angel
//             Vector3 direction = transform.position - playerCamera.position;
            
//             // LayerMask: Abaikan layer "TransparentFX" atau koin biar ga salah hitung
//             if (Physics.Raycast(playerCamera.position, direction, out hit))
//             {
//                 // Kalau yang kena adalah Angel (diri sendiri), berarti terlihat
//                 if (hit.transform == transform) return true;
                
//                 // ATAU kalau yang kena adalah anak-anak objek Angel (misal mata/tangan)
//                 if (hit.transform.IsChildOf(transform)) return true;
//             }
//         }
//         return false;
//     }

//     public void ActivateWeepingMode()
//     {
//         isWeepingMode = true;
//     }
// }









// using UnityEngine;
// using UnityEngine.AI;

// public class EnemyAI : MonoBehaviour
// {
//     [Header("Targeting")]
//     public Transform player;
//     public Transform playerCamera;

//     [Header("Settings Angel")]
//     public float normalSpeed = 3.5f;
//     public float weepingSpeed = 10f; // Kecepatan saat mengejar (merah)
//     public bool isWeepingMode = false;
    
//     [Tooltip("Tinggi titik mata/dada Angel untuk deteksi Raycast")]
//     public float eyeHeight = 1.5f; 

//     private NavMeshAgent agent;
//     private Renderer angelRenderer; 

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
        
//         // Ambil renderer dari anak objek (karena model visual ada di child)
//         angelRenderer = GetComponentInChildren<Renderer>(); 

//         // Auto-Detect Player & Camera jika lupa diisi di Inspector
//         if (player == null) 
//         {
//             GameObject p = GameObject.FindGameObjectWithTag("Player");
//             if (p != null) player = p.transform;
//         }
//         if (playerCamera == null) playerCamera = Camera.main.transform;

//         // --- SETTINGAN FISIK AGAR TIDAK NABRAK TEMBOK (PENTING) ---
//         // Kita paksa settingan ini lewat kodingan agar pergerakannya responsif
//         agent.autoBraking = true;
//         agent.acceleration = 60f;    // Biar bisa ngerem mendadak & gas instan
//         agent.angularSpeed = 1000f;  // Biar putar badannya kilat (tidak nge-drift)
//     }

//     void Update()
//     {
//         if (player == null) return;
//         if (!agent.isOnNavMesh) return; // Mencegah error jika Angel belum di-bake

//         // FASE 1: MODE NORMAL (JALAN SANTAI)
//         if (!isWeepingMode)
//         {
//             agent.speed = normalSpeed;
//             agent.isStopped = false;
//             agent.SetDestination(player.position);
            
//             // Warna Normal (jika renderer ada)
//             if(angelRenderer) angelRenderer.material.color = Color.white;
//         }
//         // FASE 2: MODE WEEPING (AGRESIF)
//         else
//         {
//             bool terlihat = IsVisibleToPlayer();

//             if (terlihat)
//             {
//                 // Dilihat -> MEMBATU
//                 agent.isStopped = true;
//                 agent.velocity = Vector3.zero; // Paksa berhenti total
                
//                 if(angelRenderer) angelRenderer.material.color = Color.gray; 
//             }
//             else
//             {
//                 // Tidak Dilihat -> LARI KENCANG
//                 agent.isStopped = false;
//                 agent.speed = weepingSpeed;
//                 agent.SetDestination(player.position);
                
//                 if(angelRenderer) angelRenderer.material.color = Color.red; 
//             }
//         }

//         // --- DEBUGGING JALUR (Lihat Tab SCENE saat Play) ---
//         // Ini akan menggambar garis merah mengikuti jalur yang dipikirkan AI
//         if (agent.hasPath)
//         {
//             for (int i = 0; i < agent.path.corners.Length - 1; i++)
//             {
//                 Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
//             }
//         }
//     }

//     bool IsVisibleToPlayer()
//     {
//         if (playerCamera == null) return false;

//         // 1. Cek apakah Angel ada di dalam kotak layar kamera
//         Vector3 viewPos = playerCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
//         bool onScreen = viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && viewPos.z > 0;

//         if (onScreen)
//         {
//             RaycastHit hit;

//             // 2. Tentukan titik target (Dada/Mata), bukan kaki
//             Vector3 targetPosition = transform.position + (Vector3.up * eyeHeight); 
            
//             // Visualisasi Garis Pandang (Laser dari kamera ke dada Angel)
//             Debug.DrawLine(playerCamera.position, targetPosition, Color.yellow);

//             // 3. Cek Fisik (Apakah terhalang tembok?)
//             // Menggunakan Linecast dari Kamera -> Dada Angel
//             if (Physics.Linecast(playerCamera.position, targetPosition, out hit))
//             {
//                 // Jika yang kena laser adalah Angel sendiri -> Terlihat
//                 if (hit.transform == transform) return true;
                
//                 // Atau jika yang kena adalah bagian tubuh Angel (Child)
//                 if (hit.transform.IsChildOf(transform)) return true;
//             }
//         }
//         return false;
//     }

//     public void ActivateWeepingMode()
//     {
//         isWeepingMode = true;
//     }
// }






using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Transform playerCamera;
    
    [Header("Pengaturan Angel")]
    public float normalSpeed = 3.5f;
    public float weepingSpeed = 10f; 
    public bool isWeepingMode = false; // Biarkan false, nanti GameManager yang ubah

    [Header("Visual")]
    public Renderer angelRenderer; 

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Cari Renderer di anak-anak objek (Child)
        if (angelRenderer == null)
            angelRenderer = GetComponentInChildren<Renderer>(); 

        // Auto-Detect Player & Camera
        if (player == null) 
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
        if (playerCamera == null) playerCamera = Camera.main.transform;

        // --- PENTING: Settingan Fisik Standar (Agar tidak nyangkut) ---
        agent.autoBraking = false; 
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }

    void Update()
    {
        if (player == null) return;
        if (!agent.isOnNavMesh) return;

        // --- LOGIKA UTAMA ---

        // KONDISI 1: MODE NORMAL (JALAN SANTAI SEPERTI ZOMBIE)
        if (!isWeepingMode)
        {
            agent.speed = normalSpeed;
            agent.isStopped = false;
            agent.SetDestination(player.position);
            
            // Warna Putih Biasa
            if(angelRenderer) angelRenderer.material.color = Color.white;
        }
        
        // KONDISI 2: MODE WEEPING (MARAH)
        else
        {
            bool terlihat = IsVisibleToPlayer();

            if (terlihat)
            {
                // >>> DILIHAT PLAYER = MEMBATU <<<
                agent.isStopped = true;       // Stop NavMesh
                agent.velocity = Vector3.zero; // Stop Fisik total
                
                // Warna Abu-abu (Batu)
                if(angelRenderer) angelRenderer.material.color = Color.gray; 
            }
            else
            {
                // >>> TIDAK DILIHAT = LARI KENCANG <<<
                agent.isStopped = false;
                agent.speed = weepingSpeed;
                agent.SetDestination(player.position);
                
                // Warna Merah (Marah)
                if(angelRenderer) angelRenderer.material.color = Color.red; 
            }
        }
    }

    // --- FUNGSI DETEKSI PENGLIHATAN (INI KUNCINYA) ---
    bool IsVisibleToPlayer()
    {
        if (playerCamera == null) return false;

        // 1. Cek apakah ada di layar kamera?
        Vector3 viewPos = playerCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
        bool onScreen = viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && viewPos.z > 0;

        if (onScreen)
        {
            RaycastHit hit;

            // 2. TENTUKAN TARGET TITIK: JANGAN KAKI (0), TAPI DADA/KEPALA (1.5)
            // Ini yang bikin dia membeku dengan benar
            Vector3 targetPosition = transform.position + (Vector3.up * 1.5f); 

            // Debugging: Muncul garis merah di Scene View buat ngecek
            Debug.DrawLine(playerCamera.position, targetPosition, Color.red);

            // 3. Cek Garis Pandang (Apakah terhalang tembok?)
            if (Physics.Linecast(playerCamera.position, targetPosition, out hit))
            {
                // Kalau sinar nabrak diri sendiri = Terlihat
                if (hit.transform == transform) return true;
                
                // Kalau sinar nabrak anak objek (sayap/tangan) = Terlihat
                if (hit.transform.IsChildOf(transform)) return true;
            }
        }
        return false;
    }

    // Fungsi ini dipanggil oleh GameManager
    public void ActivateWeepingMode()
    {
        isWeepingMode = true;
    }
}