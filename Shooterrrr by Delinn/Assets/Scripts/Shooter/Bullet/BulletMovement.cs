using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidB;
    [SerializeField] private float speed;


    
    void Update()
    {
        rigidB.AddForce(transform.up * speed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            HealthComponent healh = other.transform.GetComponent<HealthComponent>();
            healh.TakeDamage(5);


        }
        Destroy(gameObject);


    }
}
