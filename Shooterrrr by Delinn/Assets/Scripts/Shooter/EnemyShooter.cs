using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public int magazineSize = 5;
    public float fireRate = 1.0f;
    public float reloadTime = 2.0f;
    public float shootingRange = 50f;
    public float damage = 5f;
    [Range(0, 100)] public float hitChance = 70f;

    [Header("References")]
    public Transform playerTarget;  
    public Transform firePoint;
    public AudioSource shootSound;
    public AudioSource reloadSound;
    public LayerMask obstacleLayer;

    private float lastFireTime = 0f;
    private int currentAmmo;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = magazineSize;
    }

    void Update()
    {
        if (isReloading || playerTarget == null) return;

        if (Vector3.Distance(transform.position, playerTarget.position) <= shootingRange && CanSeePlayer())
        {

            if (Time.time - lastFireTime >= fireRate)
            {
                if (currentAmmo > 0)
                {
                    Shoot();
                }
                else
                {
                    StartCoroutine(Reload());
                }
            }
        }
    }

    bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (playerTarget.position - firePoint.position).normalized;

        if (Physics.Raycast(firePoint.position, dirToPlayer, out RaycastHit hit, shootingRange, obstacleLayer))
        {
            return false; 
        }

        return true;
    }

    void Shoot()
    {
        lastFireTime = Time.time;
        currentAmmo--;

        shootSound.Play();

        Vector3 direction = (playerTarget.position - firePoint.position).normalized;

                float roll = Random.Range(0f, 100f);
                if (roll <= hitChance)
                {
                    playerTarget.GetComponent<HealthComponent>()?.TakeDamage(damage);
                }
            
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        reloadSound.Play();
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        isReloading = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
