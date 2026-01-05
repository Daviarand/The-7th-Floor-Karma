// using UnityEngine;
// using UnityEngine.AI;

// public class EnemyAI : MonoBehaviour
// {
//     public Transform player;
//     public Transform playerCamera;
    
//     [Header("Pengaturan Angel")]
//     public float normalSpeed = 3.5f;
//     public float weepingSpeed = 10f; 
//     public bool isWeepingMode = false; 

//     [Header("Pengaturan Jarak Animasi")]
//     public float jarakPoint = 8.0f;   
//     public float jarakAttack = 2.5f;  
    
//     // --- TAMBAHAN COOLDOWN DAMAGE ---
//     private float nextDamageTime = 0f;
//     public float damageCooldown = 2.0f;

//     [Header("Komponen Visual")]
//     public DoctorVisual visualController; 

//     private NavMeshAgent agent;

//     void Start()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         if (player == null) 
//         {
//             GameObject p = GameObject.FindGameObjectWithTag("Player");
//             if (p != null) player = p.transform;
//         }
//         if (playerCamera == null) playerCamera = Camera.main.transform;
//         if (visualController == null) visualController = GetComponentInChildren<DoctorVisual>();

//         agent.autoBraking = false; 
//         agent.stoppingDistance = 0f; 
//         agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
//     }

//     void Update()
//     {
//         if (player == null) return;
//         if (!agent.isOnNavMesh) return;

//         AturAnimasiJarak();

//         if (!isWeepingMode)
//         {
//             agent.speed = normalSpeed;
//             agent.isStopped = false;
//             agent.SetDestination(player.position);
//         }
//         else
//         {
//             bool terlihat = IsVisibleToPlayer();
//             if (terlihat)
//             {
//                 agent.isStopped = true;       
//                 agent.velocity = Vector3.zero; 
//             }
//             else
//             {
//                 agent.isStopped = false;
//                 agent.speed = weepingSpeed;
//                 agent.SetDestination(player.position);
//             }
//         }
//     }

//     void AturAnimasiJarak()
//     {
//         if (visualController == null) return;
//         float jarak = Vector3.Distance(transform.position, player.position);

//         if (jarak <= 1.5f) 
//         {
//             visualController.GantiPose("attack");
//             // --- SISTEM NYAWA DENGAN COOLDOWN ---
//             if (Time.time >= nextDamageTime)
//             {
//                 if (GameManager.instance != null)
//                 {
//                     GameManager.instance.KurangiNyawa();
//                     nextDamageTime = Time.time + damageCooldown;
//                 }
//             }
//         }
//         else if (jarak <= jarakAttack)
//         {
//             visualController.GantiPose("attack");
//         }
//         else if (jarak <= jarakPoint)
//         {
//             visualController.GantiPose("point");
//         }
//         else
//         {
//             visualController.GantiPose("idle");
//         }
//     }

//     bool IsVisibleToPlayer()
//     {
//         if (playerCamera == null) return false;
//         Vector3 targetPosition = transform.position + (Vector3.up * 1.5f); 
//         Vector3 viewPos = playerCamera.GetComponent<Camera>().WorldToViewportPoint(targetPosition);
//         bool onScreen = viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && viewPos.z > 0;

//         if (onScreen)
//         {
//             RaycastHit hit;
//             if (Physics.Linecast(playerCamera.position, targetPosition, out hit))
//             {
//                 if (hit.transform == transform) return true;
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
    public bool isWeepingMode = false; 

    [Header("Pengaturan Jarak Animasi")]
    public float jarakPoint = 8.0f;   
    public float jarakAttack = 2.5f;  
    
    private float nextDamageTime = 0f;
    public float damageCooldown = 2.0f;

    [Header("Komponen Visual")]
    public DoctorVisual visualController; 

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null) 
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
        if (playerCamera == null) playerCamera = Camera.main.transform;
        if (visualController == null) visualController = GetComponentInChildren<DoctorVisual>();

        agent.autoBraking = false; 
        agent.stoppingDistance = 0f; 
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }

    void Update()
    {
        if (player == null) return;
        if (!agent.isOnNavMesh) return;

        AturAnimasiJarak();

        if (!isWeepingMode)
        {
            agent.speed = normalSpeed;
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            bool terlihat = IsVisibleToPlayer();
            if (terlihat)
            {
                agent.isStopped = true;       
                agent.velocity = Vector3.zero; 
            }
            else
            {
                agent.isStopped = false;
                agent.speed = weepingSpeed;
                agent.SetDestination(player.position);
            }
        }
    }

    void AturAnimasiJarak()
    {
        if (visualController == null) return;
        float jarak = Vector3.Distance(transform.position, player.position);

        if (jarak <= 1.5f) 
        {
            visualController.GantiPose("attack");
            if (Time.time >= nextDamageTime)
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.KurangiNyawa();
                    nextDamageTime = Time.time + damageCooldown;
                }
            }
        }
        else if (jarak <= jarakAttack)
        {
            visualController.GantiPose("attack");
        }
        else if (jarak <= jarakPoint)
        {
            visualController.GantiPose("point");
        }
        else
        {
            visualController.GantiPose("idle");
        }
    }

    bool IsVisibleToPlayer()
    {
        if (playerCamera == null) return false;
        Vector3 targetPosition = transform.position + (Vector3.up * 1.5f); 
        Vector3 viewPos = playerCamera.GetComponent<Camera>().WorldToViewportPoint(targetPosition);
        bool onScreen = viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && viewPos.z > 0;

        if (onScreen)
        {
            RaycastHit hit;
            if (Physics.Linecast(playerCamera.position, targetPosition, out hit))
            {
                if (hit.transform == transform) return true;
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