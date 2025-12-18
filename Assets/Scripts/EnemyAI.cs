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
    // GANTI FUNGSI IsVisibleToPlayer() YANG LAMA DENGAN INI:
    bool IsVisibleToPlayer()
    {
        if (playerCamera == null) return false;

        // 1. Tentukan titik target (Dada/Mata) LEBIH DULU
        // Kita pakai ini untuk cek layar DAN raycast, supaya konsisten.
        Vector3 targetPosition = transform.position + (Vector3.up * 1.5f); 

        // 2. Cek apakah TITIK TARGET (Dada) ada di layar kamera?
        // (Sebelumnya Anda menggunakan 'transform.position' yaitu kaki, yang sering hilang kalau terlalu dekat)
        Vector3 viewPos = playerCamera.GetComponent<Camera>().WorldToViewportPoint(targetPosition);
        
        bool onScreen = viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && viewPos.z > 0;

        if (onScreen)
        {
            // Debugging Visual: Garis merah dari kamera ke dada Angel
            Debug.DrawLine(playerCamera.position, targetPosition, Color.red);

            RaycastHit hit;

            // 3. Cek Fisik (Apakah terhalang tembok?)
            // Menggunakan Linecast dari Kamera -> Dada Angel
            if (Physics.Linecast(playerCamera.position, targetPosition, out hit))
            {
                // Jika yang kena laser adalah Angel sendiri -> Terlihat
                if (hit.transform == transform) return true;
                
                // Atau jika yang kena adalah bagian tubuh Angel (Child)
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
