using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timer : MonoBehaviour
{


    [SerializeField] private float _waitTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Timer()
    {
     
      yield return new WaitForSeconds(_waitTime);
    }

}
