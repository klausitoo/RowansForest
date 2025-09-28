using UnityEngine;

public class shoot : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject stonePrefab;   
    [SerializeField] private Transform shootPoint;     
    [SerializeField] private float shootForce = 60f;   
    [SerializeField] private float upwardAngle = 30f;   
    [SerializeField] private float damage;

    [Header("Targeting")]
    [SerializeField] private EnemySelector enemySelector; //  referencia al selector de enemigos

    public float Damage => damage;
    private float shootLogic;

    void Start()
    {
        shootLogic = GetComponentInParent<thirdPersonShooting>().LastShootTime;
    }

    public void Shoot()
    {
        shootLogic = Time.time;

        // Instancia la piedra
        GameObject stone = Instantiate(stonePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = stone.GetComponent<Rigidbody>();        
        Transform target = enemySelector != null ? enemySelector.GetSelectedEnemy() : null;
        Vector3 shootDir;

        if (target != null)
        {
            // Dirección hacia el enemigo
            shootDir = (target.position - shootPoint.position).normalized;
        }
        else
        {
            
            shootDir = shootPoint.forward + shootPoint.up * upwardAngle;
        }       
        rb.AddForce(shootDir * shootForce, ForceMode.Impulse);        
        Destroy(stone, 5f);
    }
}
