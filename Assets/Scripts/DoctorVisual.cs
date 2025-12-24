using UnityEngine;

public class DoctorVisual : MonoBehaviour
{
    [Header("Masukkan Object Anak ke Sini")]
    public GameObject visualIdle;
    public GameObject visualAttack;
    public GameObject visualPoint;

    // Fungsi ini akan dipanggil oleh EnemyAI terus menerus
    public void GantiPose(string pose)
    {
        // Matikan semua dulu
        if(visualIdle) visualIdle.SetActive(false);
        if(visualAttack) visualAttack.SetActive(false);
        if(visualPoint) visualPoint.SetActive(false);

        // Nyalakan sesuai perintah
        switch (pose)
        {
            case "idle":
                if(visualIdle) visualIdle.SetActive(true);
                break;
            case "point":
                if(visualPoint) visualPoint.SetActive(true);
                break;
            case "attack":
                if(visualAttack) visualAttack.SetActive(true);
                break;
        }
    }
}