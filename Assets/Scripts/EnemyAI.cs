using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Transform playerCamera; 
    
    // Settingan Angel
    public float normalSpeed = 3.5f;
    public float weepingSpeed = 10f; 
    public bool isWeepingMode = false;

    private NavMeshAgent agent;
    private Renderer angelRenderer; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        angelRenderer = GetComponent<Renderer>();
        
        // Auto-detect player kalau lupa drag-drop
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerCamera == null) playerCamera = Camera.main.transform;

        // PENTING: Matikan auto-braking biar ga ngerem mendadak
        agent.autoBraking = false; 
    }

    void Update()
    {
        if (player == null) return;

        // Pastikan NavMeshAgent aktif dan terpasang di NavMesh
        if (!agent.isOnNavMesh) return;

        // FASE 1: MODE NORMAL (KEJAR TERUS)
        if (!isWeepingMode)
        {
            agent.speed = normalSpeed;
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        // FASE 2: MODE WEEPING (MARAH)
        else
        {
            bool terlihat = IsVisibleToPlayer();

            if (terlihat)
            {
                // Dilihat -> MEMBATU
                agent.isStopped = true;
                agent.velocity = Vector3.zero; // Paksa berhenti total fisik
                angelRenderer.material.color = Color.gray; 
            }
            else
            {
                // Tidak Dilihat -> LARI KENCANG
                agent.isStopped = false;
                agent.speed = weepingSpeed;
                agent.SetDestination(player.position);
                angelRenderer.material.color = Color.red; 
            }
        }
    }

    bool IsVisibleToPlayer()
    {
        // Cek apakah masuk layar
        Vector3 viewPos = playerCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
        bool onScreen = viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && viewPos.z > 0;

        if (onScreen)
        {
            // Cek Raycast (Apakah terhalang tembok?)
            RaycastHit hit;
            // Tembak dari kamera ke titik tengah badan Angel
            Vector3 direction = transform.position - playerCamera.position;
            
            // LayerMask: Abaikan layer "TransparentFX" atau koin biar ga salah hitung
            if (Physics.Raycast(playerCamera.position, direction, out hit))
            {
                // Kalau yang kena adalah Angel (diri sendiri), berarti terlihat
                if (hit.transform == transform) return true;
                
                // ATAU kalau yang kena adalah anak-anak objek Angel (misal mata/tangan)
                if (hit.transform.IsChildOf(transform)) return true;
            }
        }
        return false;
    }

    public void ActivateWeepingMode()
    {
        isWeepingMode = true;
    }
}