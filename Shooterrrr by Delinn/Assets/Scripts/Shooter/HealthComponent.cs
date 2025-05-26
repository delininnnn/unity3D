using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private Slider _healthSlider;

    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        UpdateSlider();
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        UpdateSlider();

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateSlider()
    {
        if (_healthSlider != null)
            _healthSlider.value = _currentHealth / _maxHealth;
    }

    private void Die()
    {
        RemoveChildren();
        Destroy(gameObject); 
    }

    private void RemoveChildren()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "BulletHole")
            {
                child.parent = transform.parent;
                child.gameObject.SetActive(false);
            }
        }
    }
}