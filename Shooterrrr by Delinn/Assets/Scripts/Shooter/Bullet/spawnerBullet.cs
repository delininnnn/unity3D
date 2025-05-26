using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class spawnerBullet : MonoBehaviour
{

    [SerializeField] private  BulletMovement bulletMovement;
    [SerializeField] private float fireRate = 1;
    

   // Start is called before the first frame update
   void Start()
    {
        StartCoroutine(Shoot());
    }


    private IEnumerator Shoot()
    {
        while (true)
        {
            Instantiate(bulletMovement, transform.position, transform.rotation);
            yield return new WaitForSeconds(fireRate);


        }
    }
}
