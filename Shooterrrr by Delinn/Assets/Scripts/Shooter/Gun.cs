using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [Header("Настройки пушки")]
    [SerializeField] private GunSettings _gunSettings;

    [Header("Настройки отверстий")]
    [SerializeField] private BulletHolePoolSettings _bulletHoleSettings;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _ammoText;

    [Header("Audio")]
    [SerializeField] private AudioSource _shoot;
    [SerializeField] private AudioSource _hitEnemy;
    [SerializeField] private AudioSource _hitWall;
    [SerializeField] private AudioSource _reload;

    private BulletHolePool _bulletHolePool;

    private float _nextShotTime;
    private int _currentAmmo;
    private bool _isReloading;
    private bool _isShooting;

    private void Start()
    {
        _currentAmmo = _gunSettings.MagazineSize;

        _bulletHolePool = new BulletHolePool(
            _bulletHoleSettings.Prefab,
            _bulletHoleSettings.PoolSize,
            _bulletHoleSettings.Lifetime,
            transform,
            this
        );

        UpdateUI();
    }

    private void Update()
    {
        if (_playerInput.actions["Fire"].WasPressedThisFrame())
        {
            TryFire();
        }

        if (_playerInput.actions["Reload"].WasPressedThisFrame())
        {
            TryReload();
        }
    }

    void TryFire()
    {
        if (Time.time < _nextShotTime || _isReloading || _currentAmmo <= 0)
        {
            if (_currentAmmo <= 0) TryReload();
            return;
        }

        Shoot();
        _nextShotTime = Time.time + _gunSettings.FireRate;
    }

    void TryReload()
    {
        if (!_isReloading && _currentAmmo < _gunSettings.MagazineSize)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        if (_isShooting) return;
        _isShooting = true; 

        _currentAmmo--;
        UpdateUI();

        PlaySound(_shoot);

        StartCoroutine(ShootBullets());
    }

    private IEnumerator ShootBullets()
    {
        for (int i = 0; i < _gunSettings.BulletsPerShot; i++)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, _gunSettings.Range))
            {
                var health = hit.transform.GetComponent<HealthComponent>();
                if (health != null)
                {
                    health.TakeDamage(_gunSettings.Damage);
                    PlaySound(_hitEnemy);
                }
                else
                {
                    PlaySound(_hitWall);
                }

                _bulletHolePool?.Spawn(
                    hit.point + hit.normal * 0.01f,
                    Quaternion.LookRotation(hit.normal),
                    hit.transform
                );
            }
            yield return new WaitForSeconds(0.2f);
        }
        _isShooting = false;
    }

    System.Collections.IEnumerator Reload()
    {
        _isReloading = true;
        PlaySound(_reload);
        yield return new WaitForSeconds(_gunSettings.ReloadTime);
        _currentAmmo = _gunSettings.MagazineSize;
        _isReloading = false;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (_ammoText != null)
        {
            _ammoText.text = $"Ammo: {_currentAmmo} / {_gunSettings.MagazineSize}";
        }
    }

    void PlaySound(AudioSource clip)
    {
        if (clip != null)
        {
            clip.PlayOneShot(clip.clip);
        }
    }
}
